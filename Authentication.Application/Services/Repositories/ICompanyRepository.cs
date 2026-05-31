namespace Authentication.Application.Services.Repositories;

public interface ICompanyRepository
{
    /// <summary>
    /// Creates new company.
    /// </summary>
    /// <param name="Name">The company name.</param>
    /// <returns>Returns the newly created company in case of success otherwise returns null.</returns>
    Task<Authentication.Domain.Entities.Auth.Company?> Create(Authentication.Domain.Entities.Auth.Company company);
    
    /// <summary>
    /// Gets all existing companies.
    /// </summary>
    /// <returns>Returns collection of all companies.</returns>
    Task<ICollection<Authentication.Domain.Entities.Auth.Company>> GetAll();
    
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
}