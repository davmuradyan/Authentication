using System;
using System.Collections.Generic;
using Authentication.Domain.Entities.Company;

namespace Authentication.Domain.Entities.Auth;

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public bool IsEmailConfirmed { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // Navigation properties
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<CompanyUser> CompanyUsers { get; set; } = new List<CompanyUser>();
    
    private User() { }

    public static User Create(string email, string passwordHash, bool isEmailConfirmed = false, bool isActive = true)
    {
        var now = DateTime.UtcNow;
        return new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = passwordHash,
            IsEmailConfirmed = isEmailConfirmed,
            IsActive = isActive,
            CreatedAt = now,
            UpdatedAt = now
        };
    }

    public void SetPassword(string passwordHash)
    {
        PasswordHash = passwordHash;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ConfirmEmail()
    {
        IsEmailConfirmed = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
}

