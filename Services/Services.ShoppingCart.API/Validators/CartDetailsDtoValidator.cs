using FluentValidation;
using Services.ShoppingCart.API.Models.Dto;

namespace Services.ShoppingCart.API.Validators;

public class CartDetailsDtoValidator : AbstractValidator<CartDetailsDto>
{
    public CartDetailsDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Cart details ID is required.");

        RuleFor(x => x.CartHeaderId)
            .NotEmpty().WithMessage("Cart header ID is required.");

        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product ID is required.");

        RuleFor(x => x.Count)
            .GreaterThan(0).WithMessage("Product count must be greater than 0.");
    }
}
