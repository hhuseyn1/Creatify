using Services.Auth.API.Models.Dto;

namespace Services.Auth.API.Services.IAuth;

public interface IAuthService
{
    Task<string> Register(RegisterDto registerDto);
    Task<Creatify.Web.Models.LoginResponseDto> Login(LoginDto loginDto);
    Task<bool> AssignRole(string email, string roleName);
}
