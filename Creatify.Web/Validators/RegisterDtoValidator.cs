using Creatify.Web.Models;
using FluentValidation;

namespace Creatify.Web.Validators;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is not valid");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone is required.")
            .Matches(@"^\+994\d{9}$").WithMessage("Phone must be a valid Azerbaijani number (+994XXXXXXXXX).");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required");
    }
}
