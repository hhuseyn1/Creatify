﻿using System.ComponentModel.DataAnnotations;

namespace Services.Order.API.Models;

public class OrderHeader
{
    [Key]
    public string Id { get; set; }
    public string? UserId { get; set; }
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
    public IEnumerable<OrderDetails> OrderDetails { get; set; }
}