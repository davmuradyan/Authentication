namespace Authentication.Contracts.Auth.Dtos.Login;

public class LogoutDto
{
    public string RefreshToken { get; set; } = string.Empty;
}