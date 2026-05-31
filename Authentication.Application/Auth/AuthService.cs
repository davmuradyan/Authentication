using System.Security.Claims;
using Authentication.Application.Services.Email;
using Authentication.Application.Services.Repositories;
using Authentication.Contracts.Auth.Dtos;
using Authentication.Contracts.Auth.Results;
using Authentication.Application.Services.HashFunctions;
using Authentication.Application.Services.Tokens;
using Authentication.Contracts.Auth.Dtos.Creation;
using Authentication.Contracts.Auth.Dtos.Login;
using Authentication.Domain.Entities;
using Authentication.Domain.Entities.Auth;
using Microsoft.AspNetCore.Http;

namespace Authentication.Application.Auth;

public class AuthService(IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IUserActivationTokenRepository userActivationTokenRepository,
    ICompanyUserRepository companyUserRepository,
    ICompanyRepository companyRepository,
    IPasswordHashService passwordHashService,
    ITokenService tokenService,
    ITokenHashService tokenHashService,
    IEmailService emailService,
    IEmailTemplateService emailTemplateService,
    IHttpContextAccessor httpContextAccessor) : IAuthService
{
    public async Task<LoginResult> Login(LoginDto loginDto)
    {
        // Check dto fields
        if (!HelperMethods.IsEmail(loginDto.Email))
        {
            var result = new LoginResult
            {
                Success = false,
                Message = "Wrong email format."
            };
            
            return result;
        }
        
        if (string.IsNullOrEmpty(loginDto.Password) ||  loginDto.Password.Length < 8 ||  loginDto.Password.Length > 20)
        {
            var result = new LoginResult
            {
                Success = false,
                Message = "Wrong password format."
            };
            
            return result;
        }
        
        // Authentication
        var user = await userRepository.GetByEmail(loginDto.Email);
        if (user is null || !passwordHashService.Verify(loginDto.Password, user.PasswordHash))
        {
            var result = new LoginResult
            {
                Success = false,
                Message = "Wrong username or password."
            };
            
            return result;
        }

        // Check account status
        if (!user.IsActive)
        {
            var result = new LoginResult
            {
                Success = false,
                Message = "Your account has been deactivated. Contact your admin."
            };
            
            return result;
        }

        if (!user.IsEmailConfirmed)
        {
            var result = new LoginResult
            {
                Success = false,
                Message = "Please confirm your email first."
            };
            
            return result;
        }
        
        // Load roles
        var roles = await userRepository.GetUserRoles(user.Id);
        var permissions = await userRepository.GetUserPermissions(user.Id);

        // Generate Access Token
        var accessToken = tokenService.GenerateAccessToken(user, roles, permissions);

        // Generate Refresh Token
        var rawRefreshToken = tokenService.GenerateRawRefreshToken(); // random 64 bytes → base64
        var hashedRefreshToken = tokenHashService.Hash(rawRefreshToken);

        await refreshTokenRepository.AddAsync(RefreshToken.Create(
            user.Id, 
            hashedRefreshToken, 
            DateTime.UtcNow.AddDays(7),
            httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString()
            )
        );

        return new LoginResult
        {
            Success = true,
            AccessToken = accessToken,
            RefreshToken = rawRefreshToken // return raw, never the hash
        };
    }

    public async Task<RefreshResult> Refresh(RefreshDto refreshDto)
    {
        // Extract principal from expired access token
        var principal = tokenService.ExtractPrincipalFromExpiredToken(refreshDto.AccessToken);
        if (principal is null)
        {
            return new RefreshResult { Success = false, Message = "Invalid access token." };
        }

        // Get UserId from claims
        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return new RefreshResult { Success = false, Message = "Invalid access token claims." };
        }

        // Hash the incoming refresh token and look it up
        var hashedRefreshToken = tokenHashService.Hash(refreshDto.RefreshToken);
        var storedToken = await refreshTokenRepository.GetByTokenAsync(hashedRefreshToken);

        // Validate stored token
        if (storedToken is null)
        {
            return new RefreshResult { Success = false, Message = "Invalid refresh token." };
        }

        if (storedToken.IsRevoked)
        {
            return new RefreshResult { Success = false, Message = "Refresh token has been revoked." };
        }

        if (storedToken.ExpiresAt < DateTime.UtcNow)
        {
            return new RefreshResult { Success = false, Message = "Refresh token has expired. Please log in again." };
        }

        // Validate UserId matches
        if (storedToken.UserId != userId)
        {
            return new RefreshResult { Success = false, Message = "Invalid refresh token." };
        }

        // Load user, roles and permissions for new access token
        var user = await userRepository.GetByIdAsync(userId);
        if (user is null || !user.IsActive)
        {
            return new RefreshResult { Success = false, Message = "User not found or deactivated." };
        }

        var roles = await userRepository.GetUserRoles(userId);
        var permissions = await userRepository.GetUserPermissions(userId);

        // Revoke old refresh token
        await refreshTokenRepository.RevokeAsync(storedToken);

        // Generate new tokens
        var newAccessToken = tokenService.GenerateAccessToken(user, roles, permissions);
        var newRawRefreshToken = tokenService.GenerateRawRefreshToken();
        var newHashedRefreshToken = tokenHashService.Hash(newRawRefreshToken);

        // Save new refresh token
        await refreshTokenRepository.AddAsync(RefreshToken.Create(
            userId, 
            newHashedRefreshToken,
            DateTime.UtcNow.AddDays(7), 
            httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString()));

        return new RefreshResult
        {
            Success = true,
            AccessToken = newAccessToken,
            RefreshToken = newRawRefreshToken
        };
    }
    
    public async Task<bool> Logout(LogoutDto logoutDto)
    {
        var hashedToken = tokenHashService.Hash(logoutDto.RefreshToken);
        var storedToken = await refreshTokenRepository.GetByTokenAsync(hashedToken);

        if (storedToken is null || storedToken.IsRevoked)
            return false;

        await refreshTokenRepository.RevokeAsync(storedToken);
        return true;
    }

    public async Task<bool> LogoutAll(Guid userId)
    {
        var user = await userRepository.GetByIdReadOnlyAsync(userId);

        if (user is null)
            return false;

        await refreshTokenRepository.RevokeAllAsync(userId);
        return true;
    }

    public async Task<InvitationResult> Invite(InvitationDto invitationDto)
    {
        var result = new InvitationResult() { Success = false };
        
        // Check dto fields
        if (string.IsNullOrWhiteSpace(invitationDto.CompanyId) ||
            string.IsNullOrWhiteSpace(invitationDto.Name) ||
            string.IsNullOrWhiteSpace(invitationDto.Surname) ||
            string.IsNullOrWhiteSpace(invitationDto.Email) ||
            string.IsNullOrWhiteSpace(invitationDto.RoleId))
        {
            result.Message = "All fields except middle name are required.";
            return result;
        }

        if (!HelperMethods.IsEmail(invitationDto.Email))
        {
            result.Message = "Email format is not valid.";
            return result;
        }
        
        var existingUser = await userRepository.GetByEmail(invitationDto.Email);
        
        // Check if user already exists
        if (existingUser is not null)
        {
            // Case when the user is deactivated.
            if (!existingUser.IsActive)
            {
                result.Message = "Account with this email is deactivated. Contact the administrator.";
                return result;
            }
            
            // Case when user is active and email is confirmed.
            if (existingUser.IsEmailConfirmed)
            {
                result.Message = "User with this email already exists.";
                return result;
            }
            
            try
            {
                var token = await userActivationTokenRepository.GetByUserIdAsync(existingUser.Id);
                
                // Case when token already used.
                if (token.IsUsed)
                {
                    result.Message = "This activation link is already used.";
                    return result;
                }

                // Case when token is not expired yet.
                if (token.ExpiresAt > DateTime.UtcNow)
                {
                    result.Message = "The last invitation is not expired yet.";
                    return result;
                }
                
                // Case when token is expired.
                // Remove old data and just continue the process.
                // Just removing User is enough.
                // With cascade the activation token, company user and user roles will also be deleted.
                await userRepository.DeleteAsync(existingUser);
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                result.Message = $"Something went wrong.\n{e.Message}";
                return result;
            }
        }

        // Validate GUIDs
        if (!Guid.TryParse(invitationDto.CompanyId, out var companyId) ||
            !Guid.TryParse(invitationDto.RoleId, out var roleId))
        {
            result.Message = "Invalid company ID or role ID format.";
            return result;
        }

        try
        {
            // Check if there is a company with provided companyId
            if (!await companyRepository.ExistsWithId(Guid.Parse(invitationDto.CompanyId)))
            {
                result.Message = "Company with this id already exists.";
                return result;
            }
            
            // Create temporary user (no password yet - empty string)
            var newUser = User.Create(
                email: invitationDto.Email,
                passwordHash: "" // Will be set during activation
            );
            
            // Save user to database
            await userRepository.AddAsync(newUser);
            
            // Create CompanyUser with provided data
            var companyUser = CompanyUser.Create(
                id: Guid.NewGuid(),
                userId: newUser.Id,
                name: invitationDto.Name,
                surname: invitationDto.Surname,
                middleName: invitationDto.MiddleName ?? "",
                companyId: companyId
            );
            
            await companyUserRepository.AddAsync(companyUser);
            
            // Generate activation token
            var token = Guid.NewGuid().ToString("N");
            var activationToken = UserActivationTokens.Create(
                userId: newUser.Id,
                token: token,
                expiresAt: DateTime.UtcNow.AddHours(24) // 24 hour expiry
            );
            
            await userActivationTokenRepository.AddAsync(activationToken);
            
            // Send activation email
            var activationLink = $"https://yourfrontend.com/activate?token={token}";
            var emailBody = emailTemplateService.GetActivationEmailBody(activationLink);
            
            await emailService.SendAsync(new EmailMessage
            {
                To = invitationDto.Email,
                Subject = "Activate Your Account",
                Body = emailBody
            });
            
            result.Success = true;
            result.Message = "Invitation sent successfully. User will receive activation email.";
            return result;
        }
        catch (Exception ex)
        {
            result.Message = $"Failed to send invitation: {ex.Message}";
            return result;
        }
    }

    public async Task<ActivationResult> Activate(ActivationDto activationDto)
    {
        var result = new ActivationResult() { Success = false };

        // Validate dto fields
        if (string.IsNullOrWhiteSpace(activationDto.Token) || string.IsNullOrWhiteSpace(activationDto.Password))
        {
            result.Message = "Token and password are required.";
            return result;
        }

        if (activationDto.Password.Length < 14 || activationDto.Password.Length > 30)
        {
            result.Message = "Password must be between 14 and 30 characters.";
            return result;
        }

        try
        {
            // Get activation token from database
            var activationToken = await userActivationTokenRepository.GetByTokenAsync(activationDto.Token);
            
            if (activationToken is null)
            {
                result.Message = "Invalid activation token.";
                return result;
            }

            // Check if token is already used
            if (activationToken.IsUsed)
            {
                result.Message = "This activation token has already been used.";
                return result;
            }

            // Check if token is expired
            if (activationToken.ExpiresAt < DateTime.UtcNow)
            {
                result.Message = "Activation token has expired. Please request a new invitation.";
                return result;
            }

            // Get the user
            var user = await userRepository.GetByIdAsync(activationToken.UserId);
            
            if (user is null)
            {
                result.Message = "User not found.";
                return result;
            }

            // Check if email is already confirmed
            if (user.IsEmailConfirmed)
            {
                result.Message = "This account is already activated.";
                return result;
            }

            // Hash the password
            var hashedPassword = passwordHashService.Hash(activationDto.Password);

            // Update user
            user.SetPassword(hashedPassword);
            user.ConfirmEmail();

            // Update activation token as used
            activationToken.MarkAsUsed();

            // Save changes
            await userRepository.UpdateAsync(user);
            await userActivationTokenRepository.UpdateAsync(activationToken);

            result.Success = true;
            result.Message = "Account activated successfully. You can now log in.";
            return result;
        }
        catch (Exception ex)
        {
            result.Message = $"Failed to activate account: {ex.Message}";
            return result;
        }
    }
}