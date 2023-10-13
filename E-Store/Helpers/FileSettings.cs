namespace E_Store.Helpers;

public static class FileSettings
{
    public const string ImagesPath = "images/products/";
    public const string AllowedExtensions = ".jpg,.jpeg,.png";
    public const int MaxSizeInMB = 1;
    public const int MaxSizeInBytes = MaxSizeInMB * 1024 * 1024;
}
