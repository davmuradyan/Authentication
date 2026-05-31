using Authentication.Domain.Entities.Company;

namespace Authentication.Application.Services.Repositories.Company;

public interface ICompanyUserRepository
{
    /// <summary>
    /// Creates a new CompanyUser in the database.
    /// </summary>
    /// <param name="companyUser">The CompanyUser entity to create.</param>
    /// <returns>Returns the created CompanyUser.</returns>
    Task<CompanyUser> Create(CompanyUser companyUser);

    /// <summary>
    /// Retrieves a CompanyUser by its unique identifier.
    /// </summary>
    /// <param name="companyUserId">The id of the CompanyUser to retrieve.</param>
    /// <returns>Returns the CompanyUser if found.</returns>
    Task<CompanyUser> GetById(Guid companyUserId);

    /// <summary>
    /// Retrieves all CompanyUsers from the database.
    /// </summary>
    /// <returns>Returns a collection of all CompanyUsers.</returns>
    Task<IEnumerable<CompanyUser>> GetAll();

    /// <summary>
    /// Updates an existing CompanyUser in the database.
    /// </summary>
    /// <param name="companyUser">The CompanyUser entity with updated values.</param>
    /// <returns></returns>
    Task Update(CompanyUser companyUser);

    /// <summary>
    /// Deletes a CompanyUser from the database.
    /// </summary>
    /// <param name="companyUser">The CompanyUser entity to delete.</param>
    /// <returns></returns>
    Task Delete(CompanyUser companyUser);
}

