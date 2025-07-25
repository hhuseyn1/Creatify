using Creatify.Shared.Models.Dto;
using Services.User.API.Models.Dto;

namespace Services.User.API.Services.IServices;

public interface IAddressService
{
    Task<List<AddressDto>> GetUserAddressesAsync(Guid userId);
    Task<bool> CreateAddressAsync(Guid userId, CreateAddressDto dto);
    Task<bool> UpdateAddressAsync(Guid addressId, Guid userId, CreateAddressDto dto);
    Task<bool> DeleteAddressAsync(Guid addressId, Guid userId);
}
