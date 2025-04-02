using FluentValidation;
using Services.ShoppingCart.API.Models.Dto;

namespace Services.ShoppingCart.API.Validators;
public class CartHeaderDtoValidator : AbstractValidator<CartHeaderDto>
{
    public CartHeaderDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Cart Header ID is required.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(x => x.CouponCode)
            .MaximumLength(50).WithMessage("Coupon code must not exceed 50 characters.")
            .When(x => !string.IsNullOrEmpty(x.CouponCode));

        RuleFor(x => x.Discount)
            .GreaterThanOrEqualTo(0).WithMessage("Discount cannot be negative.");

        RuleFor(x => x.CartTotal)
            .GreaterThanOrEqualTo(0).WithMessage("Cart total must be greater than or equal to 0.");

        RuleFor(x => x.Fullname)
            .NotEmpty().WithMessage("Fullname is required.")
            .MaximumLength(50).WithMessage("Fullname must not exceed 50 characters.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone is required.")
            .Matches(@"^\+994\d{9}$").WithMessage("Phone must be a valid Azerbaijani number (+994XXXXXXXXX).");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email is not valid.");
    }
}
