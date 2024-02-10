using Microsoft.AspNetCore.Mvc;
using Services.Auth.API.Models.Dto;
using Services.Auth.API.Services.IAuth;

namespace Services.Auth.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {

        private readonly IAuthService _authService;
        protected ResponseDto _responseDto;

        public AuthAPIController(ResponseDto responseDto, IAuthService authService)
        {
            _responseDto = responseDto;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login()
        {
            return Ok();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var message = await _authService.Register(registerDto);
            if (!string.IsNullOrEmpty(message))
            {
                _responseDto.isSuccess = true;
                _responseDto.Message = message;
                return BadRequest(_responseDto);
            }
            return Ok(_responseDto);
        }
    }
}
