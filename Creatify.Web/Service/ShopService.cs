using Creatify.Shared.Models.Dto;
using Creatify.Web.Models;
using Creatify.Web.Service.IService;
using Creatify.Web.Utility;

namespace Creatify.Web.Service;

public class ShopService : IShopService
{
    private readonly IBaseService _baseService;

    public ShopService(IBaseService baseService)
    {
        _baseService = baseService;
    }
    public async Task<ResponseDto> GetAllShopsAsync()
    {
        return await _baseService.SendAsync(new()
        {
            APIType = StaticDetails.APIType.GET,
            Url = StaticDetails.ShopAPIBase + "/api/shop/GetAllShops"
        });
    }

    public async Task<ResponseDto> GetShopByIdAsync(Guid id)
    {
        return await _baseService.SendAsync(new()
        {
            APIType = StaticDetails.APIType.GET,
            Url = StaticDetails.ShopAPIBase + "/api/shop/GetShopById/" + id
        });
    }

    public async Task<ResponseDto> GetShopByNameAsync(string name)
    {
        return await _baseService.SendAsync(new()
        {
            APIType = StaticDetails.APIType.GET,
            Url = StaticDetails.ShopAPIBase + "/api/shop/GetShopByName/" + name
        });
    }

    public async Task<ResponseDto> CreateShopAsync(ShopDto shopDto)
    {
        return await _baseService.SendAsync(new()
        {
            APIType = StaticDetails.APIType.POST,
            Data = shopDto,
            Url = StaticDetails.ShopAPIBase + "/api/shop/CreateShop",
            ContentType = StaticDetails.ContentType.MultipartFormData
        });
    }

    public async Task<ResponseDto> UpdateShopAsync(ShopDto shopDto)
    {
        return await _baseService.SendAsync(new()
        {
            APIType = StaticDetails.APIType.PUT,
            Data = shopDto,
            Url = StaticDetails.ShopAPIBase + "/api/shop/UpdateShop",
            ContentType = StaticDetails.ContentType.MultipartFormData
        });
    }

    public async Task<ResponseDto> DeleteShopByIdAsync(Guid id)
    {
        return await _baseService.SendAsync(new()
        {
            APIType = StaticDetails.APIType.DELETE,
            Url = StaticDetails.ShopAPIBase + "/api/shop/DeleteShopById/" + id
        });
    }
}
