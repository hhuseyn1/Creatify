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
		this._productService = productService;
		this._cartService = cartService;
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

	[Authorize]
	[HttpPost]
	[ActionName("ProductDetails")]
	public async Task<IActionResult> ProductDetails(ProductDto productDto)
	{
		CartDto cartDto = new CartDto()
		{
			CartHeader = new CartHeaderDto
			{
				UserId = Guid.Parse(User.Claims.FirstOrDefault(u => u.Type == JwtClaimTypes.Subject)?.Value)
			}
		};

		CartDetailsDto cartDetailsDto = new CartDetailsDto()
		{
			Count = productDto.Count,
			ProductId = productDto.ProductId
		};

		List<CartDetailsDto> cartDetailsDtos = new() { cartDetailsDto };
		cartDto.CartDetails = cartDetailsDtos;

		ResponseDto responseDto = await _cartService.UpsertCartAsync(cartDto);
		if (responseDto.isSuccess && responseDto != null)
		{
			TempData["sucess"] = "Item has been added to Shopping Cart";
			return RedirectToAction(nameof(Index));
		}
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
