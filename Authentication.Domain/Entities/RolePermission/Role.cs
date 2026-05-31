using Authentication.Domain.Entities.Auth;

namespace Authentication.Domain.Entities.RolePermission;

public class Role
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    // Navigation properties
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

    private Role() { }

    public static Role Create(string name, string description)
    {
        return new Role
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description
        };
    }
}

