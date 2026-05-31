using Authentication.Application.Services.Repositories;
using Authentication.Domain.Entities.Auth;
using Authentication.Domain.Entities.RolePermission;
using Authentication.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Infrastructure.Implementations.Repositories;

public class UserRepository(MainDbContext context) : IUserRepository
{
    public async Task<User> Create(User user)
    {
        try
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return user;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating user: {ex.Message}");
            throw;
        }
    }
    
    public async Task<User?> GetById(Guid userId)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
    }
    
    public async Task<User?> GetByIdReadOnly(Guid userId)
    {
        return await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<ICollection<Role>> GetUserRoles(Guid userId)
    {
        return await context.UserRoles
            .AsNoTracking()
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.Role)
            .ToListAsync();
    }
    
    public async Task<ICollection<Permission>> GetUserPermissions(Guid userId)
    {
        return await context.UserRoles
            .AsNoTracking()
            .Where(ur => ur.UserId == userId)
            .SelectMany(ur => ur.Role.RolePermissions)
            .Select(rp => rp.Permission)
            .Distinct()
            .ToListAsync();
    }

    public async Task Delete(User user)
    {
        try
        {
            context.Users.Remove(user);
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            Console.WriteLine("Error updating database: " + e.Message);
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine("Unknown Error: " + e.Message);
        }
    }

    public async Task Update(User user)
    {
        try
        {
            context.Users.Update(user);
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine("Error updating user: " + ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Unknown Error: " + ex.Message);
            throw;
        }
    }
}