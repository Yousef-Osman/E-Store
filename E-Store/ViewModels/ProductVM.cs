using E_Store.Attributes;
using E_Store.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace E_Store.ViewModels;

public class ProductBaseVM
{
    public string Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; }

    [Required, StringLength(400)]
    public string Description { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    public int Stock { get; set; }
}

public class ProductVM: ProductBaseVM
{
    [StringLength(1000)]
    [Display(Name = "Image")]
    public string ImageUrl { get; set; }

    [Display(Name="Brand")]
    public string BrandName{ get; set; }

    [Display(Name = "Categories")]
    public List<string> CategoryNames { get; set; }
}

public class ProductAddVM: ProductBaseVM, IProductLists
{
    [Required, Display(Name = "Image")]
    [AllowedExtensions(FileSettings.AllowedExtensions)]
    [MaxFileSize(FileSettings.MaxFileSizeInBytes)]
    public virtual IFormFile ImageFile { get; set; }

    [Required, Display(Name = "Categories")]
    public List<int> SelectedCategories { get; set; }

    [Required, Display(Name = "Brand")]
    public int SelectedBrand { get; set; }

    public IEnumerable<SelectListItem> Categories { get; set; }

    public IEnumerable<SelectListItem> Brands { get; set; }
}

public class ProductEditVM : ProductAddVM
{
    [Display(Name = "Image")]
    [AllowedExtensions(FileSettings.AllowedExtensions)]
    [MaxFileSize(FileSettings.MaxFileSizeInBytes)]
    public new IFormFile ImageFile { get; set; }

    [StringLength(1000)]
    [Display(Name = "Image")]
    public string ImageUrl { get; set; }
}

//Added to be Used with Generic method (totally unnecessary)
public interface IProductLists
{
    public IEnumerable<SelectListItem> Categories { get; set; }
    public IEnumerable<SelectListItem> Brands { get; set; }
}