namespace Services.Shop.API.Models.Dto;

public class ShopDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string OwnerEmail { get; set; }
    public string ContactEmail { get; set; }
    public string PhoneNumber { get; set; }
    public string Location { get; set; }
    public string Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? ImageLocalPath { get; set; }
    public IFormFile Image { get; set; }
}
