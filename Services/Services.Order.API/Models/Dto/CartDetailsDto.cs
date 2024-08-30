using Services.Order.API.Models.Dto;

namespace Services.Order.API.Models;

public class CartDetailsDto
{
    public string CartDetailsId { get; set; }
    public string CartHeaderId { get; set; }
    public CartHeaderDto? CartHeader { get; set; }
    public string ProductId { get; set; }
    public ProductDto? Product { get; set; }
    public int Count { get; set; }
}
