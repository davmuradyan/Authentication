using Authentication.Application.Services.Repositories;
using Authentication.Domain.Entities.Auth;
using Authentication.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Infrastructure.Implementations.Repositories;

public class UserActivationTokenRepository(MainDbContext context) : IUserActivationTokenRepository
{
    public async Task<UserActivationTokens> AddAsync(UserActivationTokens token)
    {
        try
        {
            await context.UserActivationTokens.AddAsync(token);
            await context.SaveChangesAsync();
            return token;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding activation token: {ex.Message}");
            throw;
        }
    }

    public async Task<UserActivationTokens?> GetByTokenAsync(string token)
    {
        try
        {
            return await context.UserActivationTokens.FirstOrDefaultAsync(t => t.Token == token);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting activation token: {ex.Message}");
            throw;
        }
    }

    public async Task<UserActivationTokens> GetByUserIdAsync(Guid userId)
    {
        try
        {
            var token = await context.UserActivationTokens.FirstOrDefaultAsync(t => t.UserId == userId);
            return token;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Task<bool> MarkAsUsedAsync(Guid tokenId)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(UserActivationTokens token)
    {
        try
        {
            context.UserActivationTokens.Remove(token);
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            Console.WriteLine("Error deleting activation token: " + e.Message);
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task UpdateAsync(UserActivationTokens token)
    {
        try
        {
            context.UserActivationTokens.Update(token);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating activation token: {ex.Message}");
            throw;
        }
    }
}

