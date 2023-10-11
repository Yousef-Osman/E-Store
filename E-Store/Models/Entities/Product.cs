using E_Store.Data;
using System.ComponentModel.DataAnnotations;

namespace E_Store.Models.Entities;

public class Product: BaseEntity
{
    public Product()
    {
        Id = Guid.NewGuid().ToString();
    }

    [Key, StringLength(50)]
    public string Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; }

    [Required, StringLength(400)]
    public string Description { get; set; }

    public decimal Price { get; set; }

    public int Stock { get; set; }

    [StringLength(1000)]
    public string ImageUrl { get; set; }

    public int BrandId { get; set; }

    public int CategoryId { get; set; }

    public string VendorId { get; set; }

    public Brand Brand { get; set; }
    public Category Category { get; set; }
    public ApplicationUser Vendor { get; set; }
}
