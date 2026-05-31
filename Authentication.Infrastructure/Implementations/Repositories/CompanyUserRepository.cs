using Authentication.Application.Services.Repositories;
using Authentication.Domain.Entities;
using Authentication.Domain.Entities.Company;
using Authentication.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Infrastructure.Implementations.Repositories;

public class CompanyUserRepository(MainDbContext context) : ICompanyUserRepository
{
    public async Task<CompanyUser> Create(CompanyUser companyUser)
    {
        try
        {
            await context.CompanyUsers.AddAsync(companyUser);
            await context.SaveChangesAsync();
            return companyUser;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating company user: {ex.Message}");
            throw;
        }
    }

    public async Task<CompanyUser> GetById(Guid companyUserId)
    {
        try
        {
            var companyUser = await context.CompanyUsers.FirstOrDefaultAsync(cu => cu.Id == companyUserId);
            return companyUser;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving company user by id: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<CompanyUser>> GetAll()
    {
        try
        {
            return await context.CompanyUsers.ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving all company users: {ex.Message}");
            return new List<CompanyUser>();
        }
    }

    public async Task Update(CompanyUser companyUser)
    {
        try
        {
            context.CompanyUsers.Update(companyUser);
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            Console.WriteLine($"Database error updating company user: {ex.InnerException?.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error updating company user: {ex.Message}");
        }
    }

    public async Task Delete(CompanyUser companyUser)
    {
        try
        {
            context.CompanyUsers.Remove(companyUser);
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            Console.WriteLine($"Database error deleting company user: {ex.InnerException?.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error deleting company user: {ex.Message}");
        }
    }
}

