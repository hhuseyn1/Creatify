using Creatify.Web.Models;
using FluentValidation;

namespace Creatify.Web.Validators;

public class CartHeaderDtoValidator : AbstractValidator<CartHeaderDto>
{
    public CartHeaderDtoValidator()
    {
        RuleFor(x => x.Fullname)
            .NotEmpty().WithMessage("Fullname is required.")
            .MaximumLength(50).WithMessage("Fullname must not exceed 50 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("ContactEmail is required.")
            .EmailAddress().WithMessage("Email must be a valid email address.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\d{7,15}$").WithMessage("Phone number must be a valid format.");
    }
}
