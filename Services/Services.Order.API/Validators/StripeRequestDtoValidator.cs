using FluentValidation;
using Services.Order.API.Models.Dto;

namespace Services.Order.API.Validators;
public class StripeRequestDtoValidator : AbstractValidator<StripeRequestDto>
{
    public StripeRequestDtoValidator()
    {
        RuleFor(x => x.ApprovedUrl)
            .NotEmpty().WithMessage("Approved url is required");

        RuleFor(x => x.CancelUrl)
            .NotEmpty().WithMessage("Cancel url is required");
    }
}
