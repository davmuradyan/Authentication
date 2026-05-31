using Authentication.Domain.Entities.Auth;

namespace Authentication.Domain.Entities;

public class CompanyUser
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    // Nullable for global admin
    public Guid? CompanyId { get; private set; }
    public string Name { get; private set; } = null!;
    public string Surname { get; private set; } = null!;
    public string MiddleName { get; private set; } = null!;

    // Navigation property
    public virtual User User { get; set; } = null!;
    public virtual Company Company { get; set; } = null!;

    private CompanyUser() { }

    public static CompanyUser Create(Guid id, Guid userId, string name, string surname, string middleName, Guid? companyId)

    {
        return new CompanyUser
        {
            Id = id,
            UserId = userId,
            CompanyId = companyId,
            Name = name,
            Surname = surname,
            MiddleName = middleName
        };
    }
}