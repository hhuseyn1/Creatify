using Creatify.Web.Models;
using LoginDto = Services.Auth.API.Models.Dto.LoginDto;
using RegisterDto = Services.Auth.API.Models.Dto.RegisterDto;
using LoginResponseDto = Services.Auth.API.Models.Dto.LoginResponseDto;

namespace Services.Auth.API.Services.IAuth;

public interface IAuthService
{
	Task<string> Register(RegisterDto registerDto);
	Task<LoginResponseDto> Login(LoginDto loginDto);
	Task<bool> AssignRole(string email, string roleName);
}
