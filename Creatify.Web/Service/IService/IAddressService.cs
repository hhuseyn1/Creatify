using Creatify.Shared.Models.Dto;

namespace Creatify.Web.Service.IService;

public interface IAddressService
{
    Task<ResponseDto?> GetUserAddressesAsync(Guid userId);
    Task<ResponseDto?> CreateAddressAsync(Guid userId, CreateAddressDto dto);
    Task<ResponseDto?> UpdateAddressAsync(Guid addressId, Guid userId, CreateAddressDto dto);
    Task<ResponseDto?> DeleteAddressAsync(Guid addressId, Guid userId);
}

