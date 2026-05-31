using Authentication.Domain.Entities.Auth;
using Authentication.Domain.Entities.RolePermission;

namespace Authentication.Application.Services.Repositories.Auth;

public interface IUserRepository
{
    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="user">The user to create.</param>
    /// <returns>Returns the created user.</returns>
    Task<User> Create(User user);
    
    /// <summary>
    /// Finds a user with provided id.
    /// </summary>
    /// <param name="userId">The provided id.</param>
    /// <returns>Returns the user if it exists or null otherwise.</returns>
    Task<User?> GetById(Guid userId);

    /// <summary>
    /// Finds a user with provided id, but in readonly mode. Changes won't be saved in database.
    /// </summary>
    /// <param name="userId">The provided id.</param>
    /// <returns>Returns the user if it exists or null otherwise.</returns>
    Task<User?> GetByIdReadOnly(Guid userId);
    
    /// <summary>
    /// Finds a user with provided email. 
    /// </summary>
    /// <param name="email">The provided email.</param>
    /// <returns>Returns the user in case of success. Returns null otherwise.</returns>
    Task<User?> GetByEmail(string email);
    
    /// <summary>
    /// Returns the roles of the user.
    /// </summary>
    /// <param name="userId">The user id.</param>
    /// <returns>Returns collection of roles or an empty collection in case of failure.</returns>
    Task<ICollection<Role>> GetUserRoles(Guid userId);
    
    /// <summary>
    /// Returns user permissions.
    /// </summary>
    /// <param name="userId">The user id.</param>
    /// <returns></returns>
    Task<ICollection<Permission>> GetUserPermissions(Guid userId);
    
    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="user">The user to update.</param>
    /// <returns></returns>
    Task Update(User user);
    
    /// <summary>
    /// Removes user from the database.
    /// NOTE: Associated UserActivationTokens, UserRoles and CompanyUser will also be deleted due to cascade behavior.
    /// </summary>
    /// <param name="user">User to delete</param>
    /// <returns></returns>
    Task Delete(User user);
}