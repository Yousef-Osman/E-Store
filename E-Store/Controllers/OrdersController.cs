using E_Store.Models;
using E_Store.Models.Enums;
using E_Store.Repositories.interfaces;
using E_Store.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe.Checkout;
using System.Security.Claims;

namespace E_Store.Controllers;

[Authorize]
public class OrdersController : Controller
{
    private readonly ShoppingCart _shoppingCart;
    private readonly IOrderRepository _orderRepo;
    private readonly int _pageSize = 12;

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

    public async Task<IActionResult> LoadOrders(int pageNumber)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var query = _orderRepo.GetDataQuery(userId);
            var data = await query
                .Skip((pageNumber - 1) * _pageSize)
                .Take(_pageSize)
                .ToListAsync();

            if (data.Count < 1)
                return StatusCode(StatusCodes.Status204NoContent);

            return PartialView("_LoadOrders", data);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    public async Task<IActionResult> Checkout()
    {
        var model = await FillOrderViewModel(new OrderVM());

        if (model.TotalPrice == 0)
            return BadRequest();

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(OrderVM model)
    {
        model = await FillOrderViewModel(model);

        if (!ModelState.IsValid || !(model.TotalPrice > 0))
            return View(model);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var orderId = await _orderRepo.AddOrderAsync(model, userId);

        if (orderId == null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        await _shoppingCart.ClearShoppingCartAsync();
        return RedirectToAction(nameof(Payment), new { id = orderId });
    }

    public async Task<IActionResult> Payment(string id)
    {
        var order = await _orderRepo.GetOrderWithDetailsAsync(id);

        if (!order.OrderDetails.Any() || order.Status != OrderStatus.Pending.ToString())
            return BadRequest();

        var lineItems = new List<SessionLineItemOptions>();
        foreach (var item in order.OrderDetails)
        {
            var line = new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "egp",
                    UnitAmount = (long)item.Price * 100,
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = item.ProductName,
                    },
                },
                Quantity = item.Quantity,
            };

            lineItems.Add(line);
        }

        var domain = $"{Request.Scheme}://{Request.Host}";
        var options = new SessionCreateOptions
        {
            LineItems = lineItems,
            Mode = "payment",
            SuccessUrl = domain + $"/Orders/Success?id={id}",
            CancelUrl = domain + "/Orders/Index",
        };

        var service = new SessionService();
        Session session = service.Create(options);

        order.SessionId = session.Id;
        order.PaymentIntentId = session.PaymentIntentId;
        var success = await _orderRepo.UpdateStripDataAsync(order);

        if (!success)
            return StatusCode(StatusCodes.Status500InternalServerError);

        Response.Headers.Add("Location", session.Url);
        return new StatusCodeResult(303);
    }

    public async Task<IActionResult> Success(string id)
    {
        var order = await _orderRepo.GetOrderAsync(id);

        if (order == null)
            return BadRequest();

        var service = new SessionService();
        Session session = await service.GetAsync(order.SessionId);

        if (session.PaymentStatus.ToLower() == OrderStatus.Paid.ToString().ToLower())
        {
            await _orderRepo.UpdateOrderStatusAsync(id, OrderStatus.Paid);
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteOrder(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest();

        var result = await _orderRepo.DeleteOrderAsync(id);

        if (!result)
            return NotFound();

        return RedirectToAction(nameof(Index));
    }

    private async Task<OrderVM> FillOrderViewModel(OrderVM model)
    {
        model.CartItems = await _shoppingCart.GetCartItemsVM();
        model.TotalPrice = model.CartItems.Any() ? await _shoppingCart.GetCartTotalPriceAsync() : 0;

        return model;
    }
}
