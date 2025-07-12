using Services.Category.API.Models.Dto;

namespace Services.Category.API.Service.IService;
public interface ICategoryService
{
    Task<ResponseDto> GetAllCategoriesAsync();
    Task<ResponseDto> GetRootCategoriesAsync();
    Task<ResponseDto> GetSubCategoriesAsync(Guid parentId);
    Task<ResponseDto> GetCategoryByIdAsync(Guid id);
    Task<ResponseDto> CreateCategoryAsync(CategoryDto dto);
    Task<ResponseDto> UpdateCategoryAsync(CategoryDto dto);
    Task<ResponseDto> DeleteCategoryByIdAsync(Guid id);
}