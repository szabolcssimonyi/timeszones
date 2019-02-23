using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using TimeZones.Extensibility.Dto;

namespace TimeZones.Extensibility.Interfaces
{
    public interface IAuthenticationService
    {
        Task<IdentityUser> Login(LoginDto request);
        string CreateToken(IdentityUser user);
    }
}
