using Services.ShoppingCart.API.Models.Dto;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Services.ShoppingCart.API.Models;

public class CartDetails
{
    [Key]
    public Guid Id { get; set; }
    public string CartHeaderId { get; set; }
    [ForeignKey("CartHeaderId")]
    public CartHeader CartHeader { get; set; }
    public string ProductId { get; set; }
    [NotMapped]
    public ProductDto Product { get; set; }
    public int Count { get; set; }
}
