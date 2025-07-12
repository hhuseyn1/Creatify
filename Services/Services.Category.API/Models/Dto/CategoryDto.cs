namespace Services.Category.API.Models.Dto;
public class CategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid? ParentCategoryId { get; set; }
    public string? ParentCategoryName { get; set; } 
    public List<CategoryDto>? Subcategories { get; set; }
}