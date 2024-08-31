namespace Creatify.Web.Models;

public class CouponDto
{
	public Guid Id { get; set; }
	public string CouponCode { get; set; }
	public double DiscountAmount { get; set; }
	public int MinAmount { get; set; }
}
