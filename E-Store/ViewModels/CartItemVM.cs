using E_Store.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace E_Store.ViewModels;

public class CartItemVM
{
    public string Id { get; set; }

    public string ProductId { get; set; }

    [Display(Name = "Name")]
    public string ProductName { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    [Display(Name ="Image")]
    public string imageUrl { get; set; }
}
