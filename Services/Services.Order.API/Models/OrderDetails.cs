using Services.Order.API.Models.Dto;
using System.ComponentModel.DataAnnotations.Schema;

namespace Services.Order.API.Models;

public class OrderDetails
{
    public Guid OrderDetailsId { get; set; }
    public Guid OrderHeaderId { get; set; }
    [ForeignKey("OrderHeaderId")]
    public OrderHeader? OrderHeader { get; set; }
    public Guid ProductId { get; set; }
    [NotMapped]
    public ProductDto? Product { get; set; }
    public int Count { get; set; }
    public string ProductName { get; set; }
    public double Price { get; set; }
}
