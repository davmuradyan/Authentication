using Authentication.Contracts.Auth.Results;
using Authentication.Contracts.Auth.Dtos;
using Authentication.Contracts.Auth.Dtos.Creation;
using Authentication.Contracts.Auth.Dtos.Login;

namespace Authentication.Application.Auth;

public interface IAuthService
{
    Task<LoginResult> Login(LoginDto loginDto);
    Task<RefreshResult> Refresh(RefreshDto refreshDto);
    Task<bool> Logout(LogoutDto logoutDto);
    Task<bool> LogoutAll(Guid userId);
    Task<InvitationResult>  Invite(InvitationDto invitationDto);
    Task<ActivationResult> Activate(ActivationDto activationDto);
}