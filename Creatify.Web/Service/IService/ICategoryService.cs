using Creatify.Web.Models;

namespace Creatify.Web.Service.IService;

public interface ICategoryService
{
    Task<ResponseDto> GetAllCategoriesAsync();
    Task<ResponseDto> GetRootCategoriesAsync();
    Task<ResponseDto> GetCategoryByIdAsync(Guid id);
    Task<ResponseDto> CreateCategoryAsync(CategoryDto dto);
    Task<ResponseDto> UpdateCategoryAsync(CategoryDto dto);
    Task<ResponseDto> DeleteCategoryAsync(Guid id);
    Task<ResponseDto> GetSubCategoriesAsync(Guid parentId);
}
