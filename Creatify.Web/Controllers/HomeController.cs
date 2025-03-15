using Creatify.Web.Models;
using Creatify.Web.Service.IService;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Creatify.Web.Controllers;

public class HomeController : Controller
{
    private readonly IProductService _productService;
    private readonly ICartService _cartService;

    public HomeController(IProductService productService, ICartService cartService)
    {
        _productService = productService;
        _cartService = cartService;
    }

    public async Task<IActionResult> Index()
    {
        List<ProductDto> list = new();
        ResponseDto responseDto = await _productService.GetAllProductsAsync();
        if (responseDto != null && responseDto.isSuccess)
            list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(responseDto.Result));
        else
            TempData["error"] = responseDto?.Message;
        return View(list);
    }

    [Authorize]
    [HttpGet("ProductDetails/{productId}")]
    public async Task<IActionResult> ProductDetails(Guid productId)
    {
        ProductDto productDto = new();
        ResponseDto responseDto = await _productService.GetProductByIdAsync(productId);
        if (responseDto != null && responseDto.isSuccess)
            productDto = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(responseDto.Result));
        else
            TempData["error"] = responseDto?.Message;
        return View(productDto);
    }

    [Authorize]
    [HttpPost("AddToCart")]
    public async Task<IActionResult> AddToCart(ProductDto productDto)
    {
        var userIdClaim = User.Claims.FirstOrDefault(u => u.Type == JwtClaimTypes.Subject)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
        {
            TempData["error"] = "User not found.";
            return RedirectToAction("Index");
        }

        CartDto cartDto = new CartDto
        {
            CartHeader = new CartHeaderDto
            {
                UserId = Guid.Parse(userIdClaim)
            }
        };

        CartDetailsDto cartDetailsDto = new CartDetailsDto
        {
            Count = productDto.Count,
            ProductId = productDto.Id
        };

        cartDto.CartDetails = new List<CartDetailsDto> { cartDetailsDto };

        ResponseDto responseDto = await _cartService.UpsertCartAsync(cartDto);
        if (responseDto != null && responseDto.isSuccess)
        {
            TempData["sucess"] = "Item has been added to Shopping Cart";
            return RedirectToAction(nameof(Index));
        }
        else
        {
            TempData["error"] = responseDto?.Message;
        }
        return View("ProductDetails", productDto);
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
