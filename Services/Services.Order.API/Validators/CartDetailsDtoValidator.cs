using FluentValidation;
using Services.Order.API.Models;

namespace Services.Order.API.Validators;
public class CartDetailsDtoValidator : AbstractValidator<CartDetailsDto>
{
    public CartDetailsDtoValidator()
    {
        RuleFor(x => x.CartDetailsId)
            .NotEmpty().WithMessage("Cart details ID is required.");

        RuleFor(x => x.CartHeaderId)
            .NotEmpty().WithMessage("Cart header ID is required.");

        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product ID is required.");

        RuleFor(x => x.Count)
            .GreaterThan(0).WithMessage("Product count must be greater than 0.");
    }
}
