using System.ComponentModel.DataAnnotations;

namespace Creatify.Web.Models;

public class CartHeaderDto
{
    public Guid CartHeaderId { get; set; }
    public Guid UserId { get; set; }
    public string? CouponCode { get; set; }
    public double Discount { get; set; }
    public double CartTotal { get; set; }
    [Required]
    public string? Fullname { get; set; }
    [Required]
    public string? Phone { get; set; }
    [Required]
    public string? Email { get; set; }
}
