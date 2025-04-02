using Creatify.Web.Models;
using FluentValidation;

namespace Creatify.Web.Validators;

public class CartHeaderDtoValidator : AbstractValidator<CartHeaderDto>
{
    public CartHeaderDtoValidator()
    {
        RuleFor(x => x.Fullname)
            .NotEmpty();

        RuleFor(x => x.Email)
            .EmailAddress()
            .NotEmpty();

        RuleFor(x => x.Phone)
            .Matches(@"^\+994\d{9}$").WithMessage("Azerbaijan phone format required")
            .NotEmpty();
    }
}
