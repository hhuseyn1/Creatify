using Creatify.Web.Models;
using Creatify.Web.Service.IService;
using Creatify.Web.Utility;

namespace Creatify.Web.Service;

public class CategoryService : ICategoryService
{
    private readonly IBaseService _baseService;

    public CategoryService(IBaseService baseService)
    {
        _baseService = baseService;
    }

    public async Task<ResponseDto> GetAllCategoriesAsync()
    {
        return await _baseService.SendAsync(new()
        {
            APIType = StaticDetails.APIType.GET,
            Url = StaticDetails.CategoryAPIBase + "/api/category/GetAllCategories"
        });
    }

    public async Task<ResponseDto> GetCategoryByIdAsync(Guid id)
    {
        return await _baseService.SendAsync(new()
        {
            APIType = StaticDetails.APIType.GET,
            Url = StaticDetails.CategoryAPIBase + $"/api/category/GetCategoryById/{id}"
        });
    }

    public async Task<ResponseDto> CreateCategoryAsync(CategoryDto dto)
    {
        return await _baseService.SendAsync(new()
        {
            APIType = StaticDetails.APIType.POST,
            Url = StaticDetails.CategoryAPIBase + "/api/category/CreateCategory",
            Data = dto
        });
    }

    public async Task<ResponseDto> UpdateCategoryAsync(CategoryDto dto)
    {
        return await _baseService.SendAsync(new()
        {
            APIType = StaticDetails.APIType.PUT,
            Url = StaticDetails.CategoryAPIBase + "/api/category/UpdateCategory",
            Data = dto
        });
    }

    public async Task<ResponseDto> DeleteCategoryAsync(Guid id)
    {
        return await _baseService.SendAsync(new()
        {
            APIType = StaticDetails.APIType.DELETE,
            Url = StaticDetails.CategoryAPIBase + "/api/category/DeleteCategoryById/" + id
        });
    }

    public async Task<ResponseDto> GetRootCategoriesAsync()
    {
        return await _baseService.SendAsync(new()
        {
            APIType = StaticDetails.APIType.GET,
            Url = StaticDetails.CategoryAPIBase + "/api/category/GetRootCategories"
        });
    }
    public async Task<ResponseDto> GetSubCategoriesAsync(Guid parentId)
    {
        return await _baseService.SendAsync(new()
        {
            APIType = StaticDetails.APIType.GET,
            Url = StaticDetails.CategoryAPIBase + "/api/category/GetSubCategories/" + parentId
        });
    }
}
