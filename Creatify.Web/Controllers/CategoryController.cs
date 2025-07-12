using Creatify.Web.Models;
using Creatify.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Creatify.Web.Controllers;

public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<IActionResult> CategoryIndex()
    {
        List<CategoryDto> list = new();
        var response = await _categoryService.GetAllCategoriesAsync();
        if (response.isSuccess && response.Result != null)
            list = JsonConvert.DeserializeObject<List<CategoryDto>>(response.Result.ToString());

        return View(list);
    }

    public async Task<IActionResult> CategoryCreate()
    {
        var response = await _categoryService.GetRootCategoriesAsync();
        if (response.isSuccess && response.Result != null)
        {
            var categoryList = JsonConvert.DeserializeObject<List<CategoryDto>>(response.Result.ToString());
            ViewBag.Categories = categoryList
                .Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() })
                .ToList();
        }
        else
        {
            ViewBag.Categories = new List<SelectListItem>();
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CategoryCreate(CategoryDto dto)
    {
        if (ModelState.IsValid)
        {
            var response = await _categoryService.CreateCategoryAsync(dto);
            if (response.isSuccess)
            {
                TempData["success"] = "Category created successfully";
                return RedirectToAction(nameof(CategoryIndex));
            }
            TempData["error"] = response.Message;
        }
        return View(dto);
    }

    public async Task<IActionResult> CategoryEdit(Guid id)
    {
        var response = await _categoryService.GetCategoryByIdAsync(id);
        if (!response.isSuccess || response.Result == null)
        {
            TempData["error"] = response.Message;
            return NotFound();
        }

        var model = JsonConvert.DeserializeObject<CategoryDto>(response.Result.ToString());

        var rootCategoriesResponse = await _categoryService.GetRootCategoriesAsync();
        if (rootCategoriesResponse.isSuccess && rootCategoriesResponse.Result != null)
        {
            var categoryList = JsonConvert.DeserializeObject<List<CategoryDto>>(rootCategoriesResponse.Result.ToString());
            ViewBag.Categories = categoryList
                .Where(c => c.Id != model.Id)
                .Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() })
                .ToList();
        }
        else
        {
            ViewBag.Categories = new List<SelectListItem>();
        }

        return View(model);
    }


    [HttpPost]
    public async Task<IActionResult> CategoryEdit(CategoryDto dto)
    {
        if (ModelState.IsValid)
        {
            var response = await _categoryService.UpdateCategoryAsync(dto);
            if (response.isSuccess)
            {
                TempData["success"] = "Category updated successfully";
                return RedirectToAction(nameof(CategoryIndex));
            }
            TempData["error"] = response.Message;
        }
        return View(dto);
    }

    public async Task<IActionResult> CategoryDelete(Guid id)
    {
        ResponseDto? responseDto = await _categoryService.GetCategoryByIdAsync(id);
        if (responseDto.isSuccess && responseDto != null)
        {
            CategoryDto? model = JsonConvert.DeserializeObject<CategoryDto>(responseDto.Result.ToString());
            return View(model);
        }
        else
            TempData["error"] = responseDto?.Message;
        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> CategoryDelete(CategoryDto category)
    {
        ResponseDto? responseDto = await _categoryService.DeleteCategoryAsync(category.Id);
        if (responseDto.isSuccess && responseDto != null)
        {
            TempData["success"] = "Category deleted successfully!";
            return RedirectToAction(nameof(CategoryIndex));
        }
        else
            TempData["error"] = responseDto?.Message;
        return NotFound();
    }

    [HttpGet("GetSubCategories/{parentId}")]
    public async Task<IActionResult> GetSubCategories([FromRoute] Guid parentId)
    {
        var response = await _categoryService.GetSubCategoriesAsync(parentId);
        return response.isSuccess ? Json(response) : BadRequest(response);
    }
}
