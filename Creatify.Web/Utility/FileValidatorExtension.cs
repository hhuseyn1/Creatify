using FluentValidation;

namespace Creatify.Web.Utility;

public static class FileValidatorExtension
{
    public static IRuleBuilderOptions<T, IFormFile> ValidImageFile<T>(this IRuleBuilder<T, IFormFile> ruleBuilder)
    {
        string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
        int maxFileSizeMb = 1;

        return ruleBuilder
            .NotNull().WithMessage("File is required.")
            .Must(file => file.Length <= maxFileSizeMb * 1024 * 1024)
            .WithMessage($"Max allowed file size is {maxFileSizeMb} MB.")
            .Must(file => allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            .WithMessage($"Only {string.Join(", ", allowedExtensions)} files are allowed.");
    }
}
