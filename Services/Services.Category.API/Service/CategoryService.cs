using Microsoft.EntityFrameworkCore;
using Services.Category.API.Data;
using Services.Category.API.Models.Dto;
using Services.Category.API.Service.IService;

namespace Services.Category.API.Service;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;

    public CategoryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseDto> GetAllCategoriesAsync()
    {
        var allCategories = await _context.Categories.ToListAsync();

        var dtos = allCategories.Select(cat => new CategoryDto
        {
            Id = cat.Id,
            Name = cat.Name,
            ParentCategoryId = cat.ParentCategoryId,
            ParentCategoryName = allCategories
                .FirstOrDefault(p => p.Id == cat.ParentCategoryId)?.Name
        }).ToList();

        return new ResponseDto { isSuccess = true, Result = dtos };
    }

    public async Task<ResponseDto> GetRootCategoriesAsync()
    {
        var categories = await _context.Categories
            .Where(c => c.ParentCategoryId == null)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            }).ToListAsync();

        return new ResponseDto { isSuccess = true, Result = categories };
    }

    public async Task<ResponseDto> GetSubCategoriesAsync(Guid parentId)
    {
        var allCategories = await _context.Categories.ToListAsync();

        var subcategories = allCategories
            .Where(c => c.ParentCategoryId == parentId)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                ParentCategoryId = c.ParentCategoryId,
                ParentCategoryName = allCategories.FirstOrDefault(p => p.Id == c.ParentCategoryId)?.Name
            }).ToList();

        return new ResponseDto { isSuccess = true, Result = subcategories };
    }

    public async Task<ResponseDto> GetCategoryByIdAsync(Guid id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
            return new ResponseDto { isSuccess = false, Message = "Category not found" };

        string? parentName = null;
        if (category.ParentCategoryId.HasValue)
        {
            parentName = await _context.Categories
                .Where(p => p.Id == category.ParentCategoryId)
                .Select(p => p.Name)
                .FirstOrDefaultAsync();
        }

        return new ResponseDto
        {
            isSuccess = true,
            Result = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                ParentCategoryId = category.ParentCategoryId,
                ParentCategoryName = parentName
            }
        };
    }

    public async Task<ResponseDto> CreateCategoryAsync(CategoryDto dto)
    {
        var exists = await _context.Categories
            .AnyAsync(c => c.Name.ToLower() == dto.Name.ToLower() && c.ParentCategoryId == dto.ParentCategoryId);

        if (exists)
        {
            return new ResponseDto
            {
                isSuccess = false,
                Message = "Category already exists within this name"
            };
        }

        var category = new Models.Category
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            ParentCategoryId = dto.ParentCategoryId
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return new ResponseDto { isSuccess = true, Result = category };
    }

    public async Task<ResponseDto> UpdateCategoryAsync(CategoryDto dto)
    {
        var category = await _context.Categories.FindAsync(dto.Id);
        if (category == null)
            return new ResponseDto { isSuccess = false, Message = "Category not found" };

        var exists = await _context.Categories
            .AnyAsync(c => c.Id != dto.Id && c.Name.ToLower() == dto.Name.ToLower() && c.ParentCategoryId == dto.ParentCategoryId);

        if (exists)
        {
            return new ResponseDto
            {
                isSuccess = false,
                Message = "Category already exists within same stage"
            };
        }

        category.Name = dto.Name;
        category.ParentCategoryId = dto.ParentCategoryId;
        await _context.SaveChangesAsync();

        return new ResponseDto { isSuccess = true, Result = category };
    }

    public async Task<ResponseDto> DeleteCategoryByIdAsync(Guid id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
            return new ResponseDto { isSuccess = false, Message = "Category not found" };

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return new ResponseDto { isSuccess = true, Result = category };
    }
}
