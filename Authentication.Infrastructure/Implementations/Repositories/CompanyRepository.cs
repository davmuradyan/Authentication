using Authentication.Application.Services.Repositories;
using Authentication.Application.Services.Repositories.Company;
using Authentication.Domain.Entities.Auth;
using Authentication.Domain.Entities.Company;
using Authentication.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Infrastructure.Implementations.Repositories;

public class CompanyRepository(MainDbContext context) : ICompanyRepository
{
    public async Task<Company> Create(Company company)
    {
        try
        {
            await context.Companies.AddAsync(company);
            await context.SaveChangesAsync();
            
            return company;
        }
        catch (DbUpdateException ex)
        {
            // Log the specific database error
            Console.WriteLine($"Database error creating company: {ex.InnerException?.Message}");
            return null;
        }
        catch (Exception ex)
        {
            // Log unexpected errors
            Console.WriteLine($"Unexpected error creating company: {ex.Message}");
            return null;
        }
    }

    public async Task<Company> GetById(Guid companyId)
    {
        try
        {
            var company = await context.Companies.FirstOrDefaultAsync(c => c.Id == companyId);
            return company;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ICollection<Company>> GetAll()
    {
        try
        {
            return await context.Companies
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving companies: {ex.Message}");
            return new List<Company>();
        }
    }

    public async Task<bool> ExistsWithName(string name)
    {
        try
        {
            return await context.Companies
                .AnyAsync(c => c.Name == name && c.IsActive);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error checking company existence: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> ExistsWithId(Guid id)
    {
        try
        {
            return await context.Companies
                .AnyAsync(c => c.Id == id && c.IsActive);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error checking company existence: {ex.Message}");
            throw;
        }
    }

    public async Task Update(Company company)
    {
        try
        {
            context.Companies.Update(company);
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            Console.WriteLine($"Database error updating company: {ex.InnerException?.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error updating company: {ex.Message}");
        }
    }

    public async Task Delete(Company company)
    {
        try
        {
            context.Companies.Remove(company);
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            Console.WriteLine($"Database error deleting company: {ex.InnerException?.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error deleting company: {ex.Message}");
        }
    }
}