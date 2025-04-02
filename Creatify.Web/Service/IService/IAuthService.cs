using Creatify.Web.Models;

namespace Creatify.Web.Service.IService;

public interface IAuthService
{
	Task<ResponseDto?> LoginAsync(LoginDto loginDto);
	Task<ResponseDto?> RegisterAsync(RegisterDto registerDto);
	Task<ResponseDto?> AssignRoleAsync(RegisterDto registerDto);
}
