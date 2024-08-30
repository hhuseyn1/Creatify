using Services.Order.API.Models.Dto;
using System.ComponentModel.DataAnnotations.Schema;

namespace Services.Order.API.Models;

public class OrderDetails
{
    public string OrderDetailsId { get; set; }
    public string OrderHeaderId { get; set; }
    [ForeignKey("OrderHeaderId")]
    public OrderHeader? OrderHeader { get; set; }
    public string ProductId { get; set; }
    [NotMapped]
    public ProductDto? Product { get; set; }
    public int Count { get; set; }
    public string ProductName { get; set; }
    public double Price { get; set; }
}
