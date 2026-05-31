namespace Authentication.Domain.Entities.RolePermission;

public class RolePermission
{
    public Guid PermissionId { get; set; }
    public Guid RoleId { get; set; }

    // Navigation properties
    public Permission Permission { get; set; } = null!;
    public Role Role { get; set; } = null!;
    
    private RolePermission() {}

    public static RolePermission Create(Guid permissionId, Guid roleId)
    {
        return new RolePermission()
        {
            PermissionId = permissionId,
            RoleId = roleId
        };
    }
}