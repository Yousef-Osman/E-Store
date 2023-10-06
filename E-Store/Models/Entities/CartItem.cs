using System.ComponentModel.DataAnnotations;

namespace E_Store.Models.Entities;

public class CartItem
{
    public CartItem()
    {
        Id = Guid.NewGuid().ToString();
        Created = DateTime.Now;
        LastModified = DateTime.Now;
    }

    [Key, StringLength(50)]
    public string Id { get; set; }

    [Required, StringLength(50)]
    public string ShoppingCartId { get; set; }

    [Required]
    public string ProductId { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public DateTime Created { get; set; }

    public DateTime? LastModified { get; set; }

    public Product Product { get; set; }
}
