namespace Services.Order.API.Models.Dto;

public class OrderHeaderDto
{
    public string? UserId { get; set; }
    public string? OrderHeaderId { get; set; }
    public string? CouponCode { get; set; }
    public double Discount { get; set; }
    public double OrderTotal { get; set; }
    public string? Fullname { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public DateTime OrderTime { get; set; }
    public string? Status { get; set; }

    public string? PaymentIntentId { get; set; }
    public string? StripeSessionId { get; set; }
    public IEnumerable<OrderDetailsDto> OrderDetails { get; set; }
}
