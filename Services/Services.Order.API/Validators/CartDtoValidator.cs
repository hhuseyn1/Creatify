using FluentValidation;
using Services.Order.API.Models;

namespace Services.Order.API.Validators;

public class CartDtoValidator : AbstractValidator<CartDto>
{
    public CartDtoValidator()
    {
        RuleFor(x => x.CartDetails)
            .NotNull().WithMessage("Cart details are required.");
    }
}
