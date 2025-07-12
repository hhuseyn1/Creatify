using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Category.API.Models.Dto;
using Services.Category.API.Service.IService;

namespace Services.Category.API.Controllers;

[Authorize(Roles = "ADMIN")]
[Route("api/category")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet("GetAllCategories")]
    public async Task<IActionResult> GetAllCategories()
    {
        var response = await _categoryService.GetAllCategoriesAsync();
        return response.isSuccess ? Ok(response) : BadRequest(response);
    }

    [HttpGet("GetRootCategories")]
    public async Task<IActionResult> GetRootCategories()
    {
        var response = await _categoryService.GetRootCategoriesAsync();
        return response.isSuccess ? Ok(response) : BadRequest(response);
    }

    [HttpGet("GetSubCategories/{parentId}")]
    public async Task<IActionResult> GetSubCategories(Guid parentId)
    {
        var response = await _categoryService.GetSubCategoriesAsync(parentId);
        return response.isSuccess ? Ok(response) : BadRequest(response);
    }

    [HttpGet("GetCategoryById/{id}")]
    public async Task<IActionResult> GetCategoryById(Guid id)
    {
        var response = await _categoryService.GetCategoryByIdAsync(id);
        return response.isSuccess ? Ok(response) : NotFound(response);
    }

    [HttpPost("CreateCategory")]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResponseDto { isSuccess = false, Message = "Model state is invalid." });

        var response = await _categoryService.CreateCategoryAsync(dto);
        return response.isSuccess ? Ok(response) : BadRequest(response);
    }

    [HttpPut("UpdateCategory")]
    public async Task<IActionResult> UpdateCategory([FromBody] CategoryDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResponseDto { isSuccess = false, Message = "Model state is invalid." });

        var response = await _categoryService.UpdateCategoryAsync(dto);
        return response.isSuccess ? Ok(response) : BadRequest(response);
    }

    [HttpDelete("DeleteCategoryById/{id}")]
    public async Task<IActionResult> DeleteCategoryById(Guid id)
    {
        var response = await _categoryService.DeleteCategoryByIdAsync(id);
        return response.isSuccess ? Ok(response) : NotFound(response);
    }
}
