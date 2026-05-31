namespace Authentication.Contracts.Crud.Company.Results;

public class CreateCompanyResult
{
    public bool Success { get; set; }
    public Guid? CompanyId { get; set; }
    public string? CompanyName { get; set; }
    public string? Message { get; set; }
}