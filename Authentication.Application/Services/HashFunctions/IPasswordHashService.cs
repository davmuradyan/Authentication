namespace Authentication.Application.Services.HashFunctions;

public interface IPasswordHashService
{
    string Hash(string value);
    bool Verify(string value, string hash);
}