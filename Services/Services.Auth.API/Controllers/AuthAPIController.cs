using Microsoft.AspNetCore.Mvc;

namespace Services.Auth.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {

        [HttpPost("login")]
        public async Task<IActionResult> Login()
        {
            return Ok();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register()
        {
            return Ok();
        }
    }
}
