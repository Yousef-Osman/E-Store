namespace E_Store.Models.Entities;

public class ProductCategory
{
    public string ProductId { get; set; }
    public int CategoryId { get; set; }
    public Product Product { get; set; }
    public Category Category { get; set; }
}
