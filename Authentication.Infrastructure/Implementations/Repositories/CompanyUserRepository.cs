using Authentication.Application.Services.Repositories;
using Authentication.Domain.Entities;
using Authentication.Infrastructure.Database;

namespace Authentication.Infrastructure.Implementations.Repositories;

public class CompanyUserRepository(MainDbContext context) : ICompanyUserRepository
{
    public async Task<CompanyUser> AddAsync(CompanyUser companyUser)
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
}

