namespace Authentication.Contracts.Auth.Dtos.Login;

public record LoginDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}