using System.ComponentModel.DataAnnotations;

namespace E_Store.ViewModels;

public class ProductVM
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

    [StringLength(1000)]
    [Display(Name = "Image")]
    public string ImageUrl { get; set; }

    [Display(Name="Brand")]
    public string BrandName{ get; set; }

    [Display(Name = "Category")]
    public string CategoryName { get; set; }
}
