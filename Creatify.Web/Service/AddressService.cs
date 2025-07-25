using Creatify.Shared.Models.Dto;
using Creatify.Web.Service.IService;
using Creatify.Web.Utility;

namespace Creatify.Web.Service;

public class AddressService : IAddressService
{
    private readonly IBaseService _baseService;

    public AddressService(IBaseService baseService)
    {
        _baseService = baseService;
    }

    public async Task<ResponseDto?> GetUserAddressesAsync(Guid userId)
    {
        return await _baseService.SendAsync(new()
        {
            APIType = StaticDetails.APIType.GET,
            Url = StaticDetails.UserAPIBase + "/api/user/address/GetAllAddresses"
        });
    }

    public async Task<ResponseDto?> CreateAddressAsync(Guid userId, CreateAddressDto dto)
    {
        return await _baseService.SendAsync(new()
        {
            APIType = StaticDetails.APIType.POST,
            Url = StaticDetails.UserAPIBase + "/api/user/address/CreateAddress",
            Data = dto
        });
    }

    public async Task<ResponseDto?> UpdateAddressAsync(Guid addressId, Guid userId, CreateAddressDto dto)
    {
        return await _baseService.SendAsync(new()
        {
            APIType = StaticDetails.APIType.PUT,
            Url = StaticDetails.UserAPIBase + $"/api/user/address/UpdateAddressbyId/{addressId}",
            Data = dto
        });
    }

    public async Task<ResponseDto?> DeleteAddressAsync(Guid addressId, Guid userId)
    {
        return await _baseService.SendAsync(new()
        {
            APIType = StaticDetails.APIType.DELETE,
            Url = StaticDetails.UserAPIBase + $"/api/user/address/DeleteAddressbyId/{addressId}"
        });
    }
}
