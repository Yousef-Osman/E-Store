using E_Store.Models;
using E_Store.Repositories.interfaces;
using E_Store.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Store.Controllers;

[Authorize]
public class OrdersController : Controller
{
    private readonly ShoppingCart _shoppingCart;
    private readonly IOrderRepository _orderRepo;

    public OrdersController(ShoppingCart shoppingCart,
                            IOrderRepository orderRepo)
    {
        _shoppingCart = shoppingCart;
        _orderRepo = orderRepo;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> AddOrder()
    {
        var model = await FillOrderViewModel(new OrderVM());

        if (model.TotalPrice == 0)
            return BadRequest();

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddOrder(OrderVM model)
    {
        model = await FillOrderViewModel(model);

        if (!ModelState.IsValid)
            return View(model);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _orderRepo.AddOrderAsync(model, userId);

        if (!result)
            return StatusCode(StatusCodes.Status500InternalServerError);

        await _shoppingCart.ClearShoppingCartAsync();
        return RedirectToAction(nameof(Checkout));
    }

    private async Task<OrderVM> FillOrderViewModel(OrderVM model)
    {
        model.CartItems = await _shoppingCart.GetCartItemsVM();
        model.TotalPrice = model.CartItems.Any() ? await _shoppingCart.GetCartTotalPriceAsync() : 0;

        return model;
    }

    public IActionResult Checkout()
    {
        return View();
    }
}
