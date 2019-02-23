using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TimeZones.Extensibility.Dto;
using TimeZones.Extensibility.Interfaces;

namespace TimeZones.Api.Controllers
{
    [Route("api/[Controller]")]
    [Produces("application/json")]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginDto request)
        {
            IdentityUser user = await authenticationService.Login(request);
            return user == null
                   ? StatusCode(StatusCodes.Status401Unauthorized) as IActionResult
                   : Ok(authenticationService.CreateToken(user));

        }
    }
}