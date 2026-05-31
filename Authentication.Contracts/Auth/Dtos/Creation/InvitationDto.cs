namespace Authentication.Contracts.Auth.Dtos.Creation;

public record InvitationDto
{
    public string CompanyId { get; set; }
    public string Name  { get; set; }
    public string Surname { get; set; }
    public string MiddleName { get; set; }
    public string Email { get; set; }
    public string RoleId { get; set; }
}