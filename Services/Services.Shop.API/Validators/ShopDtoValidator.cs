using FluentValidation;
using Services.Shop.API.Extensions;
using Services.Shop.API.Models.Dto;

namespace Services.Shop.API.Validators;

public class ShopDtoValidator : AbstractValidator<ShopDto>
{
    public ShopDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Shop ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Shop name is required.")
            .MaximumLength(100).WithMessage("Shop name must not exceed 100 characters.");

        RuleFor(x => x.OwnerEmail)
            .NotEmpty().WithMessage("Owner email is required.")
            .EmailAddress().WithMessage("Owner email must be a valid email address.");

        RuleFor(x => x.ContactEmail)
            .NotEmpty().WithMessage("Contact email is required.")
            .EmailAddress().WithMessage("Contact email must be a valid email address.")
            .When(x => !string.IsNullOrEmpty(x.ContactEmail));

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number must be a valid format.")
            .Matches(@"^\d{7,15}$").WithMessage("Phone number must be a valid format.")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

        RuleFor(x => x.Location)
            .MaximumLength(500).WithMessage("Location must not exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Location));

        RuleFor(x => x.Image)
             .ValidImageFile();
    }
}
