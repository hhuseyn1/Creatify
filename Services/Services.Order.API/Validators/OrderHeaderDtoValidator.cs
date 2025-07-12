using FluentValidation;
using Services.Order.API.Models.Dto;

namespace Services.Order.API.Validators;

public class OrderHeaderDtoValidator : AbstractValidator<OrderHeaderDto>
{
    public OrderHeaderDtoValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(x => x.OrderHeaderId)
            .NotEmpty().WithMessage("Order header ID is required.");

        RuleFor(x => x.CouponCode)
            .MaximumLength(50).WithMessage("Coupon code must not exceed 50 characters.")
            .When(x => !string.IsNullOrEmpty(x.CouponCode));

        RuleFor(x => x.Discount)
            .GreaterThanOrEqualTo(0).WithMessage("Discount cannot be negative.");

        RuleFor(x => x.OrderTotal)
            .GreaterThanOrEqualTo(0).WithMessage("Order total must be greater than or equal to 0.");

        RuleFor(x => x.Fullname)
            .NotEmpty().WithMessage("Fullname is required.")
            .MaximumLength(50).WithMessage("Fullname must not exceed 50 characters.");

        RuleFor(x => x.Phone)
             .NotEmpty().WithMessage("Phone number is required.")
             .Matches(@"^\d{7,15}$").WithMessage("Phone number must be a valid format.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email is not valid.");

        RuleFor(x => x.OrderTime)
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Order time cannot be in the future.");

        RuleFor(x => x.Status)
            .MaximumLength(30).WithMessage("Status must not exceed 30 characters.")
            .When(x => !string.IsNullOrEmpty(x.Status));

        RuleFor(x => x.PaymentIntentId)
            .NotEmpty().WithMessage("Payment intent ID is required.");

        RuleFor(x => x.StripeSessionId)
            .NotEmpty().WithMessage("Stripe session ID is required.");

        RuleFor(x => x.OrderDetails)
            .NotNull().WithMessage("Order details are required.")
            .Must(x => x.Any()).WithMessage("Order details cannot be empty.");
    }
}
