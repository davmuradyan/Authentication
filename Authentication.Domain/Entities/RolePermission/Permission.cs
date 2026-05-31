namespace Authentication.Domain.Entities.RolePermission;

public class Permission
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;

    // Navigation property
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    
    private Permission(){}

    public static Permission Create(string name, string description)
    {
        return new Permission
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description
        };
    }

    public void Edit(string name, string description)
    {
        Name = name;
        Description = description;
    }
}

