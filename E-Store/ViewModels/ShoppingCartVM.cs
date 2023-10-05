
namespace E_Store.ViewModels;

public class ShoppingCartVM
{
    public decimal TotalPrice { get; set; }
    public List<CartItemVM> Items { get; set; }
}
