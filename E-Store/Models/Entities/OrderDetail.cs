using System.ComponentModel.DataAnnotations;

namespace E_Store.Models.Entities;

public class OrderDetail: BaseEntity
{
    public OrderDetail()
    {
        Id = Guid.NewGuid().ToString();
    }

    [Key, StringLength(50)]
    public string Id { get; set; }

    [Required]
    public string OrderId { get; set; }

    [Required]
    public string ProductId { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    public int Quantity { get; set; }

    public Product Product { get; set; }
    public Order Order { get; set; }
}
