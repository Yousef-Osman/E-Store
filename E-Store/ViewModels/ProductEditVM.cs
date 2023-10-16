using E_Store.Attributes;
using E_Store.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace E_Store.ViewModels;

//This model works for both create and Update
public class ProductEditVM: ProductVM
{
    [Display(Name = "Image")] //removed [Required] to allow it to work in update
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
