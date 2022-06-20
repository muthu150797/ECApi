using EcAPI.Entities;
using EcAPI.Entity;

namespace EcAPI.Interfaces
{
    public interface ITokenService
    {
        string BuildToken(string key, string issuer, dynamic user);
        bool IsTokenValid(string key, string issuer, string audience, string token);
        string CreateToken(AppUser user);
    }

}
