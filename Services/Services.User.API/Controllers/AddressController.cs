using Creatify.Shared.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.User.API.Services.IServices;
using System.Security.Claims;

namespace Services.User.API.Controllers;

[Authorize]
[Route("api/user/address")]
[ApiController]
public class AddressController : ControllerBase
{
    private readonly IAddressService _addressService;

    public AddressController(IAddressService addressService)
    {
        _addressService = addressService;
    }

    private Guid GetUserId() =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet("GetAllAddresses")]
    public async Task<IActionResult> GetUserAddresses()
    {
        var response = await _addressService.GetUserAddressesAsync(GetUserId());
        return Ok(response);
    }

    [HttpPost("CreateAddress")]
    public async Task<IActionResult> CreateAddress([FromBody] CreateAddressDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResponseDto { isSuccess = false, Message = "Model state is invalid." });

        var result = await _addressService.CreateAddressAsync(GetUserId(), dto);
        return result
            ? Ok(new ResponseDto { isSuccess = true, Message = "Address created successfully" })
            : BadRequest(new ResponseDto { isSuccess = false, Message = "Failed to create address" });
    }

    [HttpPut("UpdateAddressbyId/{addressId}")]
    public async Task<IActionResult> UpdateAddress(Guid addressId, [FromBody] CreateAddressDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResponseDto { isSuccess = false, Message = "Model state is invalid." });

        var result = await _addressService.UpdateAddressAsync(addressId, GetUserId(), dto);
        return result
            ? Ok(new ResponseDto { isSuccess = true, Message = "Address updated successfully" })
            : NotFound(new ResponseDto { isSuccess = false, Message = "Address not found or not owned by user" });
    }

    [HttpDelete("DeleteAddressbyId/{addressId}")]
    public async Task<IActionResult> DeleteAddress(Guid addressId)
    {
        var result = await _addressService.DeleteAddressAsync(addressId, GetUserId());
        return result
            ? Ok(new ResponseDto { isSuccess = true, Message = "Address deleted successfully" })
            : NotFound(new ResponseDto { isSuccess = false, Message = "Address not found or not owned by user" });
    }
}
