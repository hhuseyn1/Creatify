using Creatify.Web.Models;
using Creatify.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Creatify.Web.Controllers;

public class ProductController : Controller
{
    private readonly IProductService service;

    public ProductController(IProductService service)
    {
        this.service = service;
    }
    public async Task<IActionResult> ProductIndex()
    {
        List<ProductDto> list = new();
        ResponseDto responseDto = await service.GetAllProductsAsync();
        if (responseDto.isSuccess && responseDto != null)
            list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(responseDto.Result));
        else
            TempData["error"] = responseDto?.Message;
        return View(list);
    }

    public IActionResult ProductCreate()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> ProductCreate(ProductDto ProductDto)
    {
        if(ModelState.IsValid)
        {
            ResponseDto? responseDto = await service.CreateProductAsync(ProductDto);
            if (responseDto.isSuccess && responseDto != null) { 
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
		ResponseDto? responseDto = await service.DeleteProductAsync(id);
		if (responseDto.isSuccess && responseDto != null)
        {
            TempData["success"] = "Product deleted successfully!";
            return RedirectToAction(nameof(ProductIndex));
        }
        else
            TempData["error"] = responseDto?.Message;
        return NotFound();
	}


}
