namespace Services.Category.API.Models;
public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }
    public ICollection<Category>? Subcategories { get; set; }
}
