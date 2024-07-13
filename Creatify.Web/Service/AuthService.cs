using Creatify.Web.Models;
using Creatify.Web.Service.IService;
using Creatify.Web.Utility;
using Services.Auth.API.Models.Dto;

namespace Creatify.Web.Service;

public class AuthService : IAuthService
{
    private readonly IBaseService _baseService;
    public AuthService(IBaseService baseService)
    {
        this._baseService = baseService;
    }

    public async Task<ResponseDto?> AssignRoleAsync(RegisterDto loginDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            APIType = StaticDetails.APIType.POST,
            Data = loginDto,
            Url = StaticDetails.AuthAPIBase + "/api/auth/AssignRole"
        });
    }

    public async Task<ResponseDto?> LoginAsync(LoginDto loginDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            APIType = StaticDetails.APIType.POST,
            Data = loginDto,
            Url = StaticDetails.AuthAPIBase + "/api/auth/login"
        });
    }

    public async Task<ResponseDto?> RegisterAsync(RegisterDto registerDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            APIType = StaticDetails.APIType.POST,
            Data = registerDto,
            Url = StaticDetails.AuthAPIBase + "/api/auth/register"
        });
    }
}
