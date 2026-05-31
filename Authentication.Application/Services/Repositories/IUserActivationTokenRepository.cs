using Authentication.Domain.Entities.Auth;

namespace Authentication.Application.Services.Repositories;

public interface IUserActivationTokenRepository
{
    /// <summary>
    /// Creates a new user activation token.
    /// </summary>
    /// <param name="token">The activation token entity to add.</param>
    /// <returns>Returns the newly created activation token in case of success.</returns>
    Task<UserActivationTokens> Create(UserActivationTokens token);
    
    /// <summary>
    /// Retrieves an activation token by its token string value.
    /// </summary>
    /// <param name="token">The token string value to search for.</param>
    /// <returns>Returns the activation token if found.</returns>
    Task<UserActivationTokens> GetByToken(string token);
    
    /// <summary>
    /// Retrieves an activation token by the user id.
    /// </summary>
    /// <param name="userId">The user id to search for.</param>
    /// <returns>Returns the activation token associated with the user.</returns>
    Task<UserActivationTokens> GetByUserId(Guid userId);
    
    /// <summary>
    /// Updates an existing activation token.
    /// </summary>
    /// <param name="token">The activation token entity with updated values.</param>
    /// <returns></returns>
    Task Update(UserActivationTokens token);
    
    /// <summary>
    /// Deletes an activation token.
    /// </summary>
    /// <param name="token">The activation token entity to delete.</param>
    /// <returns></returns>
    Task Delete(UserActivationTokens token);
}

