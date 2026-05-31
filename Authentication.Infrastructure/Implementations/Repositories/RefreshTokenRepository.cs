using Authentication.Application.Services.Repositories;
using Authentication.Domain.Entities.Auth;
using Authentication.Infrastructure.Database;
using Authentication.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Infrastructure.Implementations.Repositories;

public class RefreshTokenRepository(MainDbContext context) : IRefreshTokenRepository
{
    public async Task<RefreshToken> AddAsync(RefreshToken refreshToken)
    {
        await context.RefreshTokens.AddAsync(refreshToken);
        await context.SaveChangesAsync();
        return refreshToken;
    }
    
    public async Task<RefreshToken?> GetByTokenAsync(string hashedToken)
    {
        return await context.RefreshTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(rt => rt.Token == hashedToken);
    }

    public async Task RevokeAsync(RefreshToken token)
    {
        context.RefreshTokens.Attach(token);
        token.Revoke();
        await context.SaveChangesAsync();
    }

    public async Task RevokeAllAsync(Guid userId)
    {
        await context.RefreshTokens
            .Where(rt => rt.UserId == userId && !rt.IsRevoked)
            .ExecuteUpdateAsync(rt => rt.SetProperty(x => x.IsRevoked, true));
    }
}