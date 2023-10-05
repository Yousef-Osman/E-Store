using E_Store.Models;
using E_Store.Repositories.interfaces;
using E_Store.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace E_Store.Controllers;
public class ShoppingCartController : Controller
{
    private readonly IProductRepository _productRepo;
    private readonly ShoppingCart _shoppingCart;

    public ShoppingCartController(IProductRepository productRepo, ShoppingCart shoppingCart)
    {
        _productRepo = productRepo;
        _shoppingCart = shoppingCart;
    }

    public async Task<IActionResult> Index()
    {
        var items = await _shoppingCart.GetCartItemsVM();
        var tottalPrice = items.Any() ? await _shoppingCart.GetCartTotalPriceAsync() : 0;

        var model = new ShoppingCartVM()
        {
            Items = items,
            TotalPrice = tottalPrice,
        };

        return View(model);
    }

    public async Task<IActionResult> AddItemToCart(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest();

        var product = await _productRepo.GetProductAsync(id);

        if (product == null)
            return BadRequest();

        await _shoppingCart.AddToCartAsync(product);

        string returnUrl = HttpContext.Request.Query["returnUrl"];
        if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> RemoveItemFromCart(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest();

        await _shoppingCart.RemoveFromCartAsync(id);

        string returnUrl = HttpContext.Request.Query["returnUrl"];
        if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> DeleteCartItem(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest();

        await _shoppingCart.DeleteCartItemAsync(id);

        string returnUrl = HttpContext.Request.Query["returnUrl"];
        if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> GetCartItemsCount()
    {
        var ItemsCount = await _shoppingCart.GetCartItemsCountAsync();

        return Ok(new { Count = ItemsCount });
    }
}
