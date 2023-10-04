using E_Store.Data;
using E_Store.Models.Entities;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace E_Store.Models;

public class ShoppingCart
{
    private readonly ApplicationDbContext _context;

    public string ShoppingCartId { get; set; }
    public List<CartItem> Items { get; set; }

    public ShoppingCart(ApplicationDbContext context)
    {
        _context = context;
    }

    public static ShoppingCart GetShoppingCart(IServiceProvider services)
    {
        ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
        var context = services.GetService<ApplicationDbContext>();

        var cartId = session.GetString("ShoppingCartId") ?? Guid.NewGuid().ToString();
        session.SetString("ShoppingCartId", cartId);

        return new ShoppingCart(context) { ShoppingCartId = cartId };
    }

    public async Task AddToCartAsync(Product product)
    {
        var item = await _context.CartItems.FirstOrDefaultAsync(a => a.ShoppingCartId == ShoppingCartId && a.ProductId == product.Id);

        if (item == null)
        {
            item = new CartItem()
            {
                ShoppingCartId = ShoppingCartId,
                ProductId = product.Id,
                Price = product.Price,
                Quantity = 1
            };

            _context.CartItems.Add(item);
        }
        else
        {
            item.Quantity++;
        }

        await _context.SaveChangesAsync();
    }

    public async Task RemoveFromCartAsync(string productId)
    {
        var item = await _context.CartItems.FirstOrDefaultAsync(a => a.ShoppingCartId == ShoppingCartId && a.ProductId == productId);

        if (item == null)
            return;

        if (item.Quantity > 1)
            item.Quantity--;
        else
            _context.Remove(item);

        await _context.SaveChangesAsync();
    }

    public async Task<List<CartItem>> GetCartItems()
    {
        Items ??= (await _context.CartItems.Where(a => a.ShoppingCartId == ShoppingCartId).Include(a => a.Product).ToListAsync());
        return Items;
    }

    public async Task<decimal> GetCartTotalAsync()
    {
        return await _context.CartItems.Select(a => a.Quantity * a.Price).SumAsync();
    }

    public async Task ClearShoppingCartAsync()
    {
        var items = await _context.CartItems.Where(a => a.ShoppingCartId == ShoppingCartId).ToListAsync();

        if (items.Any())
        {
            _context.RemoveRange(items);
            await _context.SaveChangesAsync();
        }
    }
}
