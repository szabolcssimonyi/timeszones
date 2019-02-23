using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TimeZones.Extensibility.Dto;
using TimeZones.Extensibility.Interfaces;

namespace TimeZones.Api.Controllers
{
    [Authorize]
    [Route("api/[Controller]")]
    [Produces("application/json")]
    public class TimeZonesController : Controller
    {
        private readonly ITimeZonesRepository timeZonesRepository;

        public TimeZonesController(ITimeZonesRepository timeZonesRepository)
        {
            this.timeZonesRepository = timeZonesRepository;
        }

        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> Get()
        {
            return Ok(await timeZonesRepository.GetAsync<TimeZoneDto>());
        }

        [HttpGet]
        [Route("get/{userId}")]
        public async Task<IActionResult> Get(string userId)
        {
            return Ok(await timeZonesRepository.GetAsync<TimeZoneDto>(userId));
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Save(UserTimeZoneDto dto)
        {
            await timeZonesRepository.SaveUserTimeZoneAsync(dto);
            return Ok();
        }
    }
}