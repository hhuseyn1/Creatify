using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Shop.API.Models.Dto;
using Services.Shop.API.Service.IService;

namespace Services.Shop.API.Controllers;

[Route("api/shop")]
[ApiController]
public class ShopController : ControllerBase
{
    private readonly IShopService _shopService;

    public ShopController(IShopService shopService)
    {
        _shopService = shopService;
    }

    [HttpGet("GetAllShops")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ResponseDto> GetAllShops()
    {
        var response = new ResponseDto();
        try
        {
            response = await _shopService.GetAllShopsAsync();
        }
        catch (Exception ex)
        {
            response.isSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    [Authorize(Roles = "ADMIN")]
    [HttpGet("GetShopById/{id}")]
    public async Task<ResponseDto> GetShopById(Guid id)
    {
        var response = new ResponseDto();
        try
        {
            response = await _shopService.GetShopByIdAsync(id);
        }
        catch (Exception ex)
        {
            response.isSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    [HttpGet("GetShopByName/{name}")]
    public async Task<ResponseDto> GetShopByName(string name)
    {
        var response = new ResponseDto();
        try
        {
            response = await _shopService.GetShopByNameAsync(name);
        }
        catch (Exception ex)
        {
            response.isSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    [Authorize(Roles = "ADMIN")]
    [HttpPost("CreateShop")]
    public async Task<ResponseDto> CreateShop([FromBody] ShopDto shopDto)
    {
        var response = new ResponseDto();
        try
        {
            // Validation
            if (!ModelState.IsValid)
            {
                response.isSuccess = false;
                response.Message = "Model state is invalid.";
                return response;
            }

            response = await _shopService.CreateShopAsync(shopDto);
        }
        catch (Exception ex)
        {
            response.isSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    [Authorize(Roles = "SELLER")]
    [HttpPut("UpdateShop")]
    public async Task<ResponseDto> UpdateShop([FromBody] ShopDto shopDto)
    {
        var response = new ResponseDto();
        try
        {
            if (!ModelState.IsValid)
            {
                response.isSuccess = false;
                response.Message = "Model state is invalid.";
                return response;
            }

            response = await _shopService.UpdateShopAsync(shopDto);
        }
        catch (Exception ex)
        {
            response.isSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    [Authorize(Roles = "ADMIN")]
    [HttpDelete("DeleteShopById/{id}")]
    public async Task<ResponseDto> DeleteShopById(Guid id)
    {
        var response = new ResponseDto();
        try
        {
            response = await _shopService.DeleteShopByIdAsync(id);
        }
        catch (Exception ex)
        {
            response.isSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }
}
