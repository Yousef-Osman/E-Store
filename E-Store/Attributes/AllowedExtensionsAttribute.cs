using System.ComponentModel.DataAnnotations;

namespace E_Store.Attributes;

public class AllowedExtensionsAttribute : ValidationAttribute
{
    private readonly string _allowedExtensions;

    public AllowedExtensionsAttribute(string allowedExtensions)
    {
        _allowedExtensions = allowedExtensions;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var file = value as IFormFile;

        if (file != null)
        {
            var extension = Path.GetExtension(file.FileName);
            var extensions = _allowedExtensions.Split(',');
            var isAllowed = extensions.Contains(extension, StringComparer.OrdinalIgnoreCase);

            if (!isAllowed)
                return new ValidationResult($"Only {_allowedExtensions} Files are allowed");
        }

        return ValidationResult.Success;
    }
}
