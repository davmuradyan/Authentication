using Authentication.Domain.Entities.Auth;

namespace Authentication.Application.Services.Repositories.Auth;

public interface IRefreshTokenRepository
{
    /// <summary>
    /// Creates new refresh token in the DB.
    /// </summary>
    /// <param name="refreshToken">The refresh token to create.</param>
    /// <returns>Returns the created refresh token.</returns>
    Task<RefreshToken> Create(RefreshToken refreshToken);
    
    /// <summary>
    /// Retrieves refresh token by the token.
    /// </summary>
    /// <param name="hashedToken">The hashed token.</param>
    /// <returns>Returns the refresh token if found.</returns>
    Task<RefreshToken?> GetByToken(string hashedToken);
    
    /// <summary>
    /// Retrieves a refresh token by its unique identifier.
    /// </summary>
    /// <param name="refreshTokenId">The id of the refresh token to retrieve.</param>
    /// <returns>Returns the refresh token if found.</returns>
    Task<RefreshToken> GetById(Guid refreshTokenId);

    /// <summary>
    /// Retrieves all refresh tokens from the database.
    /// </summary>
    /// <returns>Returns a collection of all refresh tokens.</returns>
    Task<IEnumerable<RefreshToken>> GetAll();

    /// <summary>
    /// Updates an existing refresh token in the database.
    /// </summary>
    /// <param name="refreshToken">The refresh token entity with updated values.</param>
    /// <returns></returns>
    Task Update(RefreshToken refreshToken);

    /// <summary>
    /// Deletes a refresh token from the database.
    /// </summary>
    /// <param name="refreshToken">The refresh token entity to delete.</param>
    /// <returns></returns>
    Task Delete(RefreshToken refreshToken);
    
    /// <summary>
    /// Revokes a token by the hashed token.
    /// </summary>
    /// <param name="token">Hashed refresh token.</param>
    /// <returns></returns>
    Task Revoke(RefreshToken token);
    
    /// <summary>
    /// Revokes all refresh tokens of a user. (Logs out from all devices.)
    /// </summary>
    /// <param name="userId">The user Id.</param>
    Task RevokeAll(Guid userId);
}