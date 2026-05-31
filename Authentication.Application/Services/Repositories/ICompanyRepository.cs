namespace Authentication.Application.Services.Repositories;

public interface ICompanyRepository
{
    /// <summary>
    /// Creates new company.
    /// </summary>
    /// <param name="Name">The company name.</param>
    /// <returns>Returns the newly created company in case of success.</returns>
    Task<Domain.Entities.Company.Company> Create(Domain.Entities.Company.Company company);
    
    /// <summary>
    /// Retrieves a company by companyId.
    /// </summary>
    /// <param name="CompanyId">The company id.</param>
    /// <returns>Returns the company if found.</returns>
    Task<Domain.Entities.Company.Company> GetById(Guid CompanyId);
    
    /// <summary>
    /// Retrieves all existing companies.
    /// </summary>
    /// <returns>Returns collection of all companies.</returns>
    Task<ICollection<Domain.Entities.Company.Company>> GetAll();
    
    /// <summary>
    /// Checks if there is a company with the provided name. 
    /// </summary>
    /// <param name="name">The company name.</param>
    /// <returns>Returns true if company exist otherwise returns false.</returns>
    Task<bool>  ExistsWithName(string name);
    
    /// <summary>
    /// Checks if there is a company with the provided id. 
    /// </summary>
    /// <param name="id">The company id.</param>
    /// <returns>Returns true if company exist otherwise returns false.</returns>
    Task<bool> ExistsWithId(Guid id);
    
    /// <summary>
    /// Updates an existing company in the database.
    /// </summary>
    /// <param name="company">The company entity with updated values.</param>
    /// <returns></returns>
    Task Update(Domain.Entities.Company.Company company);
    
    /// <summary>
    /// Deletes a company from the database.
    /// </summary>
    /// <param name="company">The company entity to delete.</param>
    /// <returns></returns>
    Task Delete(Domain.Entities.Company.Company company);
}