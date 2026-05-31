using System;

namespace Authentication.Domain.Entities.Auth;

public class RefreshToken
{
    public Guid Id { get; private set; }
    public Guid UserId { get;private set; }
    public string Token { get; private set; } = null!;
    public DateTime ExpiresAt { get; private set; }
    public bool IsRevoked { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string? CreatedByIp { get; private set; }

    // Navigation property
    public User User { get; set; } = null!;

    private RefreshToken() { }

    public static RefreshToken Create(Guid userId, string token, DateTime expiresAt, string? createdByIp = null)
    {
        return new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Token = token,
            ExpiresAt = expiresAt,
            IsRevoked = false,
            CreatedAt = DateTime.UtcNow,
            CreatedByIp = createdByIp
        };
    }

    public void Revoke()
    {
        IsRevoked = true;
    }
}

