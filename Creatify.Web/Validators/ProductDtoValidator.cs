using Creatify.Web.Models;
using Creatify.Web.Utility;
using FluentValidation;

namespace Creatify.Web.Validators;

public class ProductDtoValidator : AbstractValidator<ProductDto>
{
    public ProductDtoValidator()
    {
        RuleFor(x => x.Count)
            .InclusiveBetween(1, 100).WithMessage("Count must be in range 1-100");

        RuleFor(x => x.Image)
            .ValidImageFile();
    }
}
