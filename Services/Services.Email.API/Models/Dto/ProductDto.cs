using System.ComponentModel.DataAnnotations;

namespace Services.Email.Models.Dto;

public class ProductDto
{
	public Guid Id { get; set; }
	public string Name { get; set; }
	public double Price { get; set; }
	public string Description { get; set; }
	public string CategoryName { get; set; }
	public string ImageUrl { get; set; }
	[Range(1,100)]
    public int Count { get; set; }
}
