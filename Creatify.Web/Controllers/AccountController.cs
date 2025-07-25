using Creatify.Shared.Models.Dto;
using Creatify.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.User.API.Models.Dto;
using System.Security.Claims;

namespace Creatify.Web.Controllers;

[Authorize]
public class AccountController : Controller
{
    private readonly IAddressService _addressService;

    public AccountController(IAddressService addressService)
    {
        _addressService = addressService;
    }

    private Guid GetUserId()
    {
        return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    public async Task<IActionResult> Index()
    {
        var response = await _addressService.GetUserAddressesAsync(GetUserId());
        var addresses = new List<AddressDto>();

        if (response != null && response.isSuccess && response.Result != null)
        {
            addresses = JsonConvert.DeserializeObject<List<AddressDto>>(response.Result.ToString()!);
        }

        return View(addresses);
    }

    public IActionResult CreateAddress()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateAddress(CreateAddressDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var response = await _addressService.CreateAddressAsync(GetUserId(), dto);

        if (response != null && response.isSuccess)
            return RedirectToAction(nameof(Index));

        TempData["Error"] = response?.Message ?? "Failed to create address";
        return View(dto);
    }

    public async Task<IActionResult> UpdateAddress(Guid id)
    {
        var response = await _addressService.GetUserAddressesAsync(GetUserId());
        var addresses = JsonConvert.DeserializeObject<List<AddressDto>>(response.Result.ToString()!);
        var address = addresses.FirstOrDefault(x => x.Id == id);
        if (address == null) return NotFound();

        var dto = new CreateAddressDto
        {
            Line1 = address.Line1,
            City = address.City,
            PostalCode = address.PostalCode
        };

        ViewBag.AddressId = id;
        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateAddress(Guid id, CreateAddressDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.AddressId = id;
            return View(dto);
        }

        var response = await _addressService.UpdateAddressAsync(id, GetUserId(), dto);
        if (response != null && response.isSuccess)
            return RedirectToAction(nameof(Index));

        TempData["Error"] = response?.Message ?? "Update failed";
        ViewBag.AddressId = id;
        return View(dto);
    }

    public async Task<IActionResult> DeleteAddress(Guid id)
    {
        var response = await _addressService.DeleteAddressAsync(id, GetUserId());
        if (response != null && response.isSuccess)
        {
            TempData["Success"] = "Address deleted successfully";
        }
        else
        {
            TempData["Error"] = response?.Message ?? "Failed to delete";
        }
        return RedirectToAction(nameof(Index));
    }
}
