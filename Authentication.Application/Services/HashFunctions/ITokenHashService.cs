namespace Authentication.Application.Services.HashFunctions;

public interface ITokenHashService
{
    string Hash(string value);
    bool Verify(string value, string hash);
}