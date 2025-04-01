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
    public async Task<IActionResult> GetAllShops()
    {
        var response = await _shopService.GetAllShopsAsync();
        return response.isSuccess ? Ok(response) : BadRequest(response);
    }

    [HttpGet("GetShopById/{id}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> GetShopById(Guid id)
    {
        var response = await _shopService.GetShopByIdAsync(id);
        return response.isSuccess ? Ok(response) : NotFound(response);
    }

    [HttpGet("GetShopByName/{name}")]
    public async Task<IActionResult> GetShopByName(string name)
    {
        var response = await _shopService.GetShopByNameAsync(name);
        return response.isSuccess ? Ok(response) : NotFound(response);
    }

    [HttpPost("CreateShop")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> CreateShop([FromForm] ShopDto shopDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResponseDto { isSuccess = false, Message = "Model state is invalid." });

        var response = await _shopService.CreateShopAsync(shopDto);
        return response.isSuccess ? Ok(response) : BadRequest(response);
    }

    [HttpPut("UpdateShop")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> UpdateShop([FromForm] ShopDto shopDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResponseDto { isSuccess = false, Message = "Model state is invalid." });

        var response = await _shopService.UpdateShopAsync(shopDto);
        return response.isSuccess ? Ok(response) : NotFound(response);
    }

    [HttpDelete("DeleteShopById/{id}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> DeleteShopById(Guid id)
    {
        var response = await _shopService.DeleteShopByIdAsync(id);
        return response.isSuccess ? Ok(response) : NotFound(response);
    }
}
