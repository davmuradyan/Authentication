namespace Authentication.Contracts.Auth.Dtos.Creation;

public record ActivationDto
{
    public string Token { get; set; }
    public string Password { get; set; }
}

