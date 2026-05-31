using Authentication.Domain.Entities;

namespace Authentication.Application.Services.Repositories;

public interface ICompanyUserRepository
{
    /// <summary>
    /// Creates a new company user.
    /// </summary>
    /// <param name="companyUser">The company user to create.</param>
    /// <returns>Returns the created company user.</returns>
    Task<CompanyUser> AddAsync(CompanyUser companyUser);
}

