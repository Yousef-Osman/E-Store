using E_Store.Attributes;
using E_Store.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace E_Store.ViewModels;

public class ProductEditVM: ProductVM
{
    [Required, Display(Name = "Image")]
    [AllowedExtensions(FileSettings.AllowedExtensions)]
    [MaxFileSize(FileSettings.MaxFileSizeInBytes)]
    public IFormFile ImageFile { get; set; }

    [Required, Display(Name = "Categories")]
    public List<int> SelectedCategories { get; set; }

    [Required, Display(Name = "Brand")]
    public int SelectedBrand { get; set; }

    public IEnumerable<SelectListItem> Categories { get; set; }

    public IEnumerable<SelectListItem> Brands { get; set; }
}
