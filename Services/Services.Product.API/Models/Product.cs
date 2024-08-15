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
	public string CategoryName { get; set; }
	public string ImageUrl { get; set; }

}
