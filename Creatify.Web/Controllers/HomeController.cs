using Creatify.Web.Models;
using Creatify.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Creatify.Web.Controllers;

public class HomeController : Controller
{
    private readonly IProductService _productService;

    public HomeController(IProductService productService)
    {
        this._productService = productService;
    }

    public async Task<IActionResult> Index()
    {
        List<ProductDto> list = new();
        ResponseDto responseDto = await _productService.GetAllProductsAsync();
        if (responseDto.isSuccess && responseDto != null)
            list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(responseDto.Result));
        else
            TempData["error"] = responseDto?.Message;
        return View(list);
    }

    [Authorize]
    public async Task<IActionResult> ProductDetails(Guid productId)
    {
        ProductDto productDto = new();
        ResponseDto responseDto = await _productService.GetProductByIdAsync(productId);
        if (responseDto.isSuccess && responseDto != null)
            productDto = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(responseDto.Result));
        else
            TempData["error"] = responseDto?.Message;
        return View(productDto);
    }

    public IActionResult Privacy()
    {
        return View();

    }

    public IActionResult Error()
    {
        return View();
    }
}
