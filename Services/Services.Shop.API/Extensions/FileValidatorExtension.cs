using FluentValidation;

namespace Services.Shop.API.Extensions;

public static class FileValidatorExtension
{
    public static IRuleBuilderOptions<T, IFormFile?> ValidImageFile<T>(this IRuleBuilder<T, IFormFile?> ruleBuilder)
    {
        string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
        int maxFileSizeMb = 1;

        return ruleBuilder
            .Must(file => file == null || file.Length <= maxFileSizeMb * 1024 * 1024)
            .WithMessage($"Max allowed file size is {maxFileSizeMb} MB.")
            .Must(file => file == null || allowedExtensions.Contains(Path.GetExtension(file.FileName ?? string.Empty).ToLower()))
            .WithMessage($"Only {string.Join(", ", allowedExtensions)} files are allowed.");
    }
}
