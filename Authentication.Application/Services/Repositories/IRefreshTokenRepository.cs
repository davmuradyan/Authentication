using Authentication.Domain.Entities.Auth;

namespace Authentication.Application.Services.Repositories;

public interface IRefreshTokenRepository
{
    Task<RefreshToken> AddAsync(RefreshToken refreshToken);
    Task<RefreshToken?> GetByTokenAsync(string hashedToken);
    Task RevokeAsync(RefreshToken token);
    /// <summary>
    /// Revokes all refresh tokens of a user. (Logs out from all devices.)
    /// </summary>
    /// <param name="userId">The user Id</param>
    Task RevokeAllAsync(Guid userId);
}