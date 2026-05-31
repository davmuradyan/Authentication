using Authentication.Domain.Entities.Auth;
using Authentication.Domain.Entities.RolePermission;

namespace Authentication.Application.Services.Repositories;

public interface IRoleRepository
{
    /// <summary>
    /// Creates a new role in the database.
    /// </summary>
    /// <param name="role">The role entity to create.</param>
    /// <returns>Returns the created role.</returns>
    Task<Role> Create(Role role);

    /// <summary>
    /// Retrieves a role by its unique identifier.
    /// </summary>
    /// <param name="RoleId">The id of the role to retrieve.</param>
    /// <returns>Returns the role if found.</returns>
    Task<Role> GetById(Guid RoleId);

    /// <summary>
    /// Retrieves all roles from the database.
    /// </summary>
    /// <returns>Returns a collection of all roles.</returns>
    Task<IEnumerable<Role>> GetAll();

    /// <summary>
    /// Updates an existing role in the database.
    /// </summary>
    /// <param name="role">The role entity with updated values.</param>
    /// <returns></returns>
    Task Update(Role role);

    /// <summary>
    /// Deletes a role from the database.
    /// </summary>
    /// <param name="role">The role entity to delete.</param>
    /// <returns></returns>
    Task Delete(Role role);
}