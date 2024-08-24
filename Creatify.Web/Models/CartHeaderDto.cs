namespace Creatify.Web.Models;

public class CartHeaderDto
{
    public string Id { get; set; }
    public string? UserId { get; set; }
    public string? CouponCode { get; set; }
    public double Discount { get; set; }
    public double CartTotal { get; set; }
}
