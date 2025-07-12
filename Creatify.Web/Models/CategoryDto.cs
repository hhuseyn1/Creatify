namespace Creatify.Web.Models;

public class CategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid? ParentCategoryId { get; set; }
    public string? ParentCategoryName { get; set; }
}
