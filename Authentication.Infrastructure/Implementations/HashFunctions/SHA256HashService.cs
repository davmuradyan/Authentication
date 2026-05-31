using System.Security.Cryptography;
using System.Text;
using Authentication.Application.Services.HashFunctions;

namespace Authentication.Infrastructure.Implementations.HashFunctions;

public class SHA256HashService : ITokenHashService
{
    public string Hash(string value) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(value)));

    public bool Verify(string value, string hash) =>
        Hash(value) == hash;
}