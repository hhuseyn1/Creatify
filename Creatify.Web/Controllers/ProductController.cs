using Creatify.Web.Models;
using Creatify.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Creatify.Web.Controllers;

public class ProductController : Controller
{
	private readonly IProductService _service;

	public ProductController(IProductService service)
	{
		this._service = service;
	}

	public async Task<IActionResult> ProductIndex()
	{
		List<ProductDto> list = new();
		ResponseDto responseDto = await _service.GetAllProductsAsync();
		if (responseDto.isSuccess && responseDto != null)
			list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(responseDto.Result));
		else
			TempData["error"] = responseDto?.Message;
		return View(list);
	}

	public async Task<IActionResult> ProductCreate()
	{
		return View();
	}


	[HttpPost]
	public async Task<IActionResult> ProductCreate(ProductDto ProductDto)
	{
		if (ModelState.IsValid)
		{
			ResponseDto? responseDto = await _service.CreateProductAsync(ProductDto);
			if (responseDto.isSuccess && responseDto != null)
			{
				TempData["success"] = "Product created successfully!";
				return RedirectToAction(nameof(ProductIndex));
			}
			else
				TempData["error"] = responseDto?.Message;
		}
		return View(ProductDto);
	}

    public async Task<IActionResult> ProductDelete(Guid id)
    {
        ResponseDto? responseDto = await _service.GetProductByIdAsync(id);
        if (responseDto.isSuccess && responseDto != null)
        {
			ProductDto? model = JsonConvert.DeserializeObject<ProductDto>(responseDto.Result.ToString());
            return View(model);
        }
        else
            TempData["error"] = responseDto?.Message;
        return NotFound();
    }

    [HttpPost]
	public async Task<IActionResult> ProductDelete(ProductDto product)
	{
		ResponseDto? responseDto = await _service.DeleteProductAsync(product.Id);
		if (responseDto.isSuccess && responseDto != null)
		{
			TempData["success"] = "Product deleted successfully!";
			return RedirectToAction(nameof(ProductIndex));
		}
		else
			TempData["error"] = responseDto?.Message;
		return NotFound();
	}

    public async Task<IActionResult> ProductEdit(Guid id)
    {
        ResponseDto? responseDto = await _service.GetProductByIdAsync(id);
        if (responseDto.isSuccess && responseDto != null)
        {
            ProductDto? model = JsonConvert.DeserializeObject<ProductDto>(responseDto.Result.ToString());
            return View(model);
        }
        else
            TempData["error"] = responseDto?.Message;
        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> ProductEdit(ProductDto product)
    {
        ResponseDto? responseDto = await _service.UpdateProductAsync(product);
        if (responseDto.isSuccess && responseDto != null)
        {
            TempData["success"] = "Product updated successfully!";
            return RedirectToAction(nameof(ProductIndex));
        }
        else
            TempData["error"] = responseDto?.Message;
        return NotFound();
    }


}
