using Creatify.Web.Models;
using Creatify.Web.Utility;
using FluentValidation;

namespace Creatify.Web.Validators;

public class ShopDtoValidator : AbstractValidator<ShopDto>
{
    public ShopDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(20).WithMessage("Name must not exceed 20 characters.");

        RuleFor(x => x.OwnerEmail)
            .NotEmpty().WithMessage("OwnerEmail is required.")
            .EmailAddress().WithMessage("OwnerEmail should be in valid email format.");

        RuleFor(x => x.ContactEmail)
            .NotEmpty().WithMessage("ContactEmail is required.")
            .EmailAddress().WithMessage("ContactEmail should be in valid email format.");

        RuleFor(x => x.PhoneNumber)
         .NotEmpty().WithMessage("Phone number is required.")
         .Matches(@"^\d{7,15}$").WithMessage("Phone number must be a valid format.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

        RuleFor(x => x.Image)
            .ValidImageFile();
    }
}
