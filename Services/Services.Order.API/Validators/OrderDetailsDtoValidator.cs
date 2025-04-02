using FluentValidation;
using Services.Order.API.Models.Dto;

namespace Services.Order.API.Validators;
public class OrderDetailsDtoValidator : AbstractValidator<OrderDetailsDto>
{
    public OrderDetailsDtoValidator()
    {
        RuleFor(x => x.OrderDetailsId)
            .NotEmpty().WithMessage("Order details ID is required.");

        RuleFor(x => x.OrderHeaderId)
            .NotEmpty().WithMessage("Order header ID is required.");

        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product ID is required.");

        RuleFor(x => x.Count)
            .GreaterThan(0).WithMessage("Product count must be greater than 0.");

        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(100).WithMessage("Product name must not exceed 100 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");
    }
}
