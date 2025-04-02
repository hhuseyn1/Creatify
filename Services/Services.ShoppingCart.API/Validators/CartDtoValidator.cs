using FluentValidation;
using Services.ShoppingCart.API.Models.Dto;

namespace Services.ShoppingCart.API.Validators;

public class CartDtoValidator : AbstractValidator<CartDto>
{
    public CartDtoValidator()
    {
        RuleFor(x => x.CartDetails)
            .NotNull().WithMessage("Cart details are required.");
    }
}
