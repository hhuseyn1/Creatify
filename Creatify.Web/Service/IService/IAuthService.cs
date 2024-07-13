using Creatify.Web.Models;
using Services.Auth.API.Models.Dto;

namespace Creatify.Web.Service.IService;

public interface IAuthService
{
    Task<ResponseDto?> LoginAsync(LoginDto loginDto);
    Task<ResponseDto?> RegisterAsync(RegisterDto registerDto);
    Task<ResponseDto?> AssignRoleAsync(RegisterDto registerDto);
}
