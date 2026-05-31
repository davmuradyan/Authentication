using System.Security.Claims;
using Authentication.Domain.Entities.Auth;
using Authentication.Domain.Entities.RolePermission;

namespace Authentication.Application.Services.Tokens;

public interface ITokenService
{
    string GenerateAccessToken(User user, ICollection<Role> roles, ICollection<Permission> permissions);
    string GenerateRawRefreshToken();
    ClaimsPrincipal? ExtractPrincipalFromExpiredToken(string accessToken);
}