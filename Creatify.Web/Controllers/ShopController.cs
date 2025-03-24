using Creatify.Web.Models;
using Creatify.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Creatify.Web.Controllers;

public class ShopController : Controller
{
    private readonly IShopService _shopService;

    public ShopController(IShopService shopService)
    {
        _shopService = shopService;
    }

    [HttpGet]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> ShopIndex()
    {
        var response = await _shopService.GetAllShopsAsync();
        List<ShopDto> shopList = new List<ShopDto>();

        if (response.isSuccess && response.Result != null)
        {
            shopList = JsonConvert.DeserializeObject<List<ShopDto>>(response.Result.ToString());
        }
        else
        {
            TempData["error"] = response.Message;
        }

        return View(shopList);
    }

    [HttpGet]
    [Authorize(Roles = "ADMIN")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ShopDto model)
    {
        if (!ModelState.IsValid) return View(model);

        var response = await _shopService.CreateShopAsync(model);
        if (response.isSuccess && response.Result != null)
        {
            TempData["success"] = "Shop created successfully";
            return RedirectToAction(nameof(ShopIndex));
        }
        else
        {
            TempData["error"] = response.Message;
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var response = await _shopService.GetShopByIdAsync(id);
        if (response.isSuccess && response.Result != null)
        {
            var shop = JsonConvert.DeserializeObject<ShopDto>(response.Result.ToString());
            return View(shop);
        }
        TempData["error"] = response.Message ?? "Error fetching Shop details.";
        return RedirectToAction(nameof(ShopIndex));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ShopDto model)
    {
        if (!ModelState.IsValid) return View(model);

        var response = await _shopService.UpdateShopAsync(model);
        if (response.isSuccess && response.Result != null)
        {
            TempData["success"] = "Shop updated successfully";
            return RedirectToAction(nameof(ShopIndex));
        }
        else
        {
            TempData["Error"] = response.Message;
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _shopService.GetShopByIdAsync(id);
        if (response.isSuccess && response.Result != null)
        {
            var shop = JsonConvert.DeserializeObject<ShopDto>(response.Result.ToString());
            return View(shop);
        }
        TempData["error"] = response.Message ?? "Error fetching Shop details.";
        return RedirectToAction(nameof(ShopIndex));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(ShopDto model)
    {
        var response = await _shopService.DeleteShopByIdAsync(model.Id);
        if (response.isSuccess)
        {
            TempData["success"] = "Shop deleted successfully";
        }
        else
        {
            TempData["error"] = response.Message;
        }

        return RedirectToAction(nameof(ShopIndex));
    }
}

