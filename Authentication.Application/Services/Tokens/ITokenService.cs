using System.Security.Claims;
using Authentication.Domain.Entities.Auth;

namespace Authentication.Application.Services.Tokens;

public interface ITokenService
{
    string GenerateAccessToken(User user, ICollection<Role> roles, ICollection<Permission> permissions);
    string GenerateRawRefreshToken();
    ClaimsPrincipal? ExtractPrincipalFromExpiredToken(string accessToken);
}