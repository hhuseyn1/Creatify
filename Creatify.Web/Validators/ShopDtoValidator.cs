using Creatify.Web.Models;
using Creatify.Web.Utility;
using FluentValidation;

namespace Creatify.Web.Validators;

public class ShopDtoValidator : AbstractValidator<ShopDto>
{
    public ShopDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.OwnerEmail)
            .NotEmpty()       
            .EmailAddress();

        RuleFor(x => x.ContactEmail)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+994\d{9}$").WithMessage("Azerbaijan phone format required")
            .NotEmpty();

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(1000);

        RuleFor(x => x.Image)
            .ValidImageFile();
    }
}
