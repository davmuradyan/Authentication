using Authentication.Domain.Entities.Auth;

namespace Authentication.Application.Services.Repositories;

public interface IUserActivationTokenRepository
{
    Task<UserActivationTokens> AddAsync(UserActivationTokens token);
    Task<UserActivationTokens?> GetByTokenAsync(string token);
    Task<UserActivationTokens> GetByUserIdAsync(Guid userId);
    Task<bool> MarkAsUsedAsync(Guid tokenId);
    Task DeleteAsync(UserActivationTokens token);
    Task UpdateAsync(UserActivationTokens token);
}

