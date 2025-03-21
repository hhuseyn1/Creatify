﻿namespace Services.ShoppingCart.API.Models.Dto;

public class CartDetailsDto
{
    public Guid Id { get; set; }
    public Guid CartHeaderId { get; set; }
    public CartHeaderDto? CartHeader { get; set; }
    public Guid ProductId { get; set; }
    public ProductDto? Product { get; set; }
    public int Count { get; set; }
}
