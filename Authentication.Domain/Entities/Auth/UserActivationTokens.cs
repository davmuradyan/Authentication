namespace Authentication.Domain.Entities.Auth;

public class UserActivationTokens
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Token { get; private set; } = null!;
    public DateTime ExpiresAt { get; private set; }
    public bool IsUsed { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Navigation property
    public User User { get; set; } = null!;

    private UserActivationTokens() { }

    public static UserActivationTokens Create(Guid userId, string token, DateTime expiresAt)
    {
        return new UserActivationTokens
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Token = token,
            ExpiresAt = expiresAt,
            IsUsed = false,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void MarkAsUsed()
    {
        IsUsed = true;
    }
}