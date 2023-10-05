using E_Store.Data;
using System.ComponentModel.DataAnnotations;

namespace E_Store.Models.Entities;

public class Order : BaseEntity
{
    public Order()
    {
        Id = Guid.NewGuid().ToString();
    }

    [Key, StringLength(50)]
    public string Id { get; set; }

    [Required]
    public string UserId { get; set; }

    [Required, StringLength(50)]
    public string FirstName { get; set; }

    [Required, StringLength(50)]
    public string LastName { get; set; }

    [EmailAddress, StringLength(255)]
    public string Email { get; set; }

    [Required, StringLength(30)]
    public string PhoneNumber { get; set; }

    [Required, StringLength(255)]
    public string AddressLine1 { get; set; }

    [StringLength(255)]
    public string AddressLine2 { get; set; }

    [Required, StringLength(50)]
    public string City { get; set; }

    [StringLength(50)]
    public string State { get; set; }

    [Required, StringLength(50)]
    public string Country { get; set; }

    [StringLength(20)]
    public string ZipCode { get; set; }

    [Required]
    public decimal TotalPrice { get; set; }

    public ApplicationUser User { get; set; }
    public List<OrderDetail> OrderDetails { get; set; }
}
