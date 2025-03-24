using Services.Shop.API.Models.Dto;

namespace Services.Shop.API.Service.IService;

public interface IShopService
{
    Task<ResponseDto> GetAllShopsAsync();
    Task<ResponseDto> GetShopByIdAsync(Guid id);
    Task<ResponseDto> GetShopByNameAsync(string name);
    Task<ResponseDto> CreateShopAsync(ShopDto shopDto);
    Task<ResponseDto> UpdateShopAsync(ShopDto shopDto);
    Task<ResponseDto> DeleteShopByIdAsync(Guid id);
}
