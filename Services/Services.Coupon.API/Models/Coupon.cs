using System.ComponentModel.DataAnnotations;

namespace Services.Coupon.API.Models;

public class Coupon
{
	[Key]
	public Guid Id { get; set; }
	[Required]
	public string CouponCode { get; set; }
	[Required]
	public double DiscountAmount { get; set; }
	public int MinAmount { get; set; }
}
