using Creatify.Web.Models;
using Creatify.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Creatify.Web.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    public ProductController(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> ProductIndex()
    {
        var response = await _productService.GetAllProductsAsync();
        if (response.isSuccess && response.Result != null)
        {
            var products = JsonConvert.DeserializeObject<List<ProductDto>>(response.Result.ToString());
            return View(products);
        }
        TempData["error"] = response.Message;
        return View(new List<ProductDto>());
    }

    [Authorize]
    public async Task<IActionResult> ProductCreate()
    {
        await LoadCategoriesToViewBag();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ProductCreate(ProductDto productDto)
    {
        if (ModelState.IsValid)
        {
            var response = await _productService.CreateProductAsync(productDto);
            if (response.isSuccess && response.Result != null)
            {
                TempData["success"] = "Product created successfully!";
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = response.Message;
            }
        }

        await LoadCategoriesToViewBag(productDto.MainCategoryId, productDto.SubCategoryId);
        return View(productDto);
    }


    public async Task<IActionResult> ProductEdit(Guid id)
    {
        var response = await _productService.GetProductByIdAsync(id);
        if (response.isSuccess && response.Result != null)
        {
            var product = JsonConvert.DeserializeObject<ProductDto>(response.Result.ToString());
            await LoadCategoriesToViewBag();
            return View(product);
        }
        TempData["error"] = response.Message;
        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> ProductEdit(ProductDto productDto)
    {
        if (ModelState.IsValid)
        {
            var response = await _productService.UpdateProductAsync(productDto);
            if (response.isSuccess && response.Result != null)
            {
                TempData["success"] = "Product updated successfully!";
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = response.Message;
            }
        }

        await LoadCategoriesToViewBag();
        return View(productDto);
    }

    public async Task<IActionResult> ProductDelete(Guid id)
    {
        var response = await _productService.GetProductByIdAsync(id);
        if (response.isSuccess && response.Result != null)
        {
            var product = JsonConvert.DeserializeObject<ProductDto>(response.Result.ToString());
            return View(product);
        }
        else
        {
            TempData["error"] = response.Message;
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> ProductDelete(ProductDto productDto)
    {
        var response = await _productService.DeleteProductAsync(productDto.Id);
        if (response.isSuccess && response.Result != null)
        {
            TempData["success"] = "Product deleted successfully!";
            return RedirectToAction(nameof(ProductIndex));
        }
        else
        {
            TempData["error"] = response.Message;
            return NotFound();
        }
    }

    private async Task LoadCategoriesToViewBag(Guid? selectedMainCategoryId = null, Guid? selectedSubCategoryId = null)
    {
        var mainResponse = await _categoryService.GetRootCategoriesAsync();
        var mainList = new List<SelectListItem>();
        var subList = new List<SelectListItem>();
        if (mainResponse.isSuccess && mainResponse.Result != null)
        {
            var mainCategories = JsonConvert.DeserializeObject<List<CategoryDto>>(mainResponse.Result.ToString());
            mainList = mainCategories.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString(),
                Selected = c.Id == selectedMainCategoryId
            }).ToList();

            ViewBag.MainCategories = mainList;

            if (selectedMainCategoryId.HasValue)
            {
                var subResponse = await _categoryService.GetSubCategoriesAsync(selectedMainCategoryId.Value);
                if (subResponse.isSuccess && subResponse.Result != null)
                {
                    var subCategories = JsonConvert.DeserializeObject<List<CategoryDto>>(subResponse.Result.ToString());
                    subList = subCategories.Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString(),
                        Selected = c.Id == selectedSubCategoryId
                    }).ToList();
                }
            }
            ViewBag.SubCategories = subList;
        }
    }
}
