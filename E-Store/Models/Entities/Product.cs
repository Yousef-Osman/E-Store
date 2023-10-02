using System.ComponentModel.DataAnnotations;
using System.Drawing.Drawing2D;

namespace E_Store.Models.Entities;

public class Product
{
    public Product()
    {
        Id = Guid.NewGuid().ToString();
    }

    [Key]
    [StringLength(50)]
    public string Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; }

    [Required, StringLength(400)]
    public string Description { get; set; }

    public double Price { get; set; }

    public int Stock { get; set; }

    public string ImageUrl { get; set; }

    public int BrandId { get; set; }

    public int CategoryId { get; set; }

    public Brand Brand { get; set; }
    public Category Category { get; set; }
}
