using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace E_Store.ViewModels;

public class ProductEditVM: ProductVM
{
    [Required, Display(Name = "Image")]
    public IFormFile ImageFile { get; set; }

    [Required, Display(Name = "Category")]
    public int CategoryId { get; set; }

    [Required, Display(Name = "Brand")]
    public int BrandId { get; set; }

    public List<SelectListItem> Categories { get; set; }

    public List<SelectListItem> Brands { get; set; }
}
