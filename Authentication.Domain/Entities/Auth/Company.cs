namespace Authentication.Domain.Entities.Auth;

public class Company
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public bool IsActive { get; private set; }
    
    // Navigation properties
    public ICollection<CompanyUser> CompanyUsers { get; set; } = new List<CompanyUser>();
    
    private Company() { }

    public static Company Create(Guid companyId, string name)
    {
        return new Company
        {
            Id = companyId,
            Name = name,
            IsActive = true
        };
    }
}