using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Services.Product.API.Models;
public class Product
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Range(0, 1000)]
    public double Price { get; set; }

    public string Description { get; set; }

    [Required]
    [ForeignKey(nameof(MainCategory))]
    public Guid MainCategoryId { get; set; }
    public Category MainCategory { get; set; }

    [Required]
    [ForeignKey(nameof(SubCategory))]
    public Guid SubCategoryId { get; set; }
    public Category SubCategory { get; set; }

    public string? ImageUrl { get; set; }
    public string? ImageLocalPath { get; set; }
}
