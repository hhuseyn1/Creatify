﻿namespace Services.Coupon.API.Models.Dto;

public class CouponDto
{
	public Guid Id { get; set; }
	public string CouponCode { get; set; }
	public double DiscountAmount { get; set; }
	public int MinAmount { get; set; }
}
