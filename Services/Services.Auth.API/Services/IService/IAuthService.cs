using Services.Auth.API.Models.Dto;

namespace Services.Auth.API.Services.IAuth;

public interface IAuthService
{
    Task<string> Register(RegisterDto registerDto);
    Task<string> Login(LoginDto loginDto);
}
