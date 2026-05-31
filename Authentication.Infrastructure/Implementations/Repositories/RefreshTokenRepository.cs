using Authentication.Application.Services.Repositories;
using Authentication.Domain.Entities.Auth;
using Authentication.Infrastructure.Database;
using Authentication.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Infrastructure.Implementations.Repositories;

public class RefreshTokenRepository(MainDbContext context) : IRefreshTokenRepository
{
    public async Task<RefreshToken> Create(RefreshToken refreshToken)
    {
        await context.RefreshTokens.AddAsync(refreshToken);
        await context.SaveChangesAsync();
        return refreshToken;
    }
    
    public async Task<RefreshToken?> GetByToken(string hashedToken)
    {
        return await context.RefreshTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(rt => rt.Token == hashedToken);
    }

    public async Task<RefreshToken> GetById(Guid refreshTokenId)
    {
        try
        {
            var refreshToken = await context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Id == refreshTokenId);
            return refreshToken;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving refresh token by id: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<RefreshToken>> GetAll()
    {
        try
        {
            return await context.RefreshTokens.ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving all refresh tokens: {ex.Message}");
            return new List<RefreshToken>();
        }
    }

    public async Task Update(RefreshToken refreshToken)
    {
        try
        {
            context.RefreshTokens.Update(refreshToken);
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            Console.WriteLine($"Database error updating refresh token: {ex.InnerException?.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error updating refresh token: {ex.Message}");
        }
    }

    public async Task Delete(RefreshToken refreshToken)
    {
        try
        {
            context.RefreshTokens.Remove(refreshToken);
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            Console.WriteLine($"Database error deleting refresh token: {ex.InnerException?.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error deleting refresh token: {ex.Message}");
        }
    }

    public async Task Revoke(RefreshToken token)
    {
        context.RefreshTokens.Attach(token);
        token.Revoke();
        await context.SaveChangesAsync();
    }

    public async Task RevokeAll(Guid userId)
    {
        await context.RefreshTokens
            .Where(rt => rt.UserId == userId && !rt.IsRevoked)
            .ExecuteUpdateAsync(rt => rt.SetProperty(x => x.IsRevoked, true));
    }
}