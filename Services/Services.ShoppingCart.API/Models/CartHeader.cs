using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Services.ShoppingCart.API.Models;

public class CartHeader
{
    [Key]
    public Guid Id { get; set; }
    public string? UserId { get; set; }
    public string? CouponCode { get; set; }
    [NotMapped]
    public double Discount { get; set; }
    [NotMapped]
    public double CartTotal { get; set; }
}
