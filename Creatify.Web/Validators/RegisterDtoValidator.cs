using FluentValidation;
using Services.Auth.API.Models.Dto;

namespace Creatify.Web.Validators;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("PhoneNumber is required")
            .Matches(@"^\+994\d{9}$").WithMessage("Azerbaijan phone format required");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required");
    }
}
