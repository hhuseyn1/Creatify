using FluentValidation;
using Services.Order.API.Models.Dto;

namespace Services.Order.API.Validators;
public class RewardsDtoValidator : AbstractValidator<RewardsDto>
{
    public RewardsDtoValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(x => x.RewardsActivity)
            .GreaterThanOrEqualTo(0).WithMessage("Rewards activity must be zero or greater.");

        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("Order ID is required.");
    }
}
