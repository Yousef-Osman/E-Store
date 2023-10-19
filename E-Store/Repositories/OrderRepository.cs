using E_Store.Data;
using E_Store.Models.Entities;
using E_Store.Models.Enums;
using E_Store.Repositories.interfaces;
using E_Store.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace E_Store.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _context;

    public OrderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IQueryable<Order> GetDataQuery(string UserId)
    {
        return _context.Orders
            .Where(a => a.UserId == UserId)
            .Include(a => a.OrderDetails)
            .OrderByDescending(a => a.Created)
            .AsNoTracking();
    }

    public async Task<Order> GetOrderAsync(string id)
    {
        return await _context.Orders
            .Where(a => a.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    public async Task<Order> GetOrderWithDetailsAsync(string id)
    {
        return await _context.Orders
            .Where(a => a.Id == id)
            .Include(a => a.OrderDetails)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    public async Task<List<OrderDetail>> GetOrderDetailsAsync(string id)
    {
        return await _context.OrdersDetails
            .Where(a => a.OrderId == id)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<string> AddOrderAsync(OrderVM orderVM, string userId)
    {
        var order = new Order
        {
            UserId = userId,
            FirstName = orderVM.FirstName,
            LastName = orderVM.LastName,
            PhoneNumber = orderVM.PhoneNumber,
            Email = orderVM.Email,
            AddressLine1 = orderVM.AddressLine1,
            AddressLine2 = orderVM.AddressLine2,
            City = orderVM.City,
            Country = orderVM.Country,
            State = orderVM.State,
            ZipCode = orderVM.ZipCode,
            TotalPrice = orderVM.TotalPrice,
            Status = OrderStatus.Pending.ToString(),
            OrderDetails = new List<OrderDetail>()
        };

        foreach (var item in orderVM.CartItems)
        {
            order.OrderDetails.Add(new OrderDetail()
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Price = item.Price,
                Quantity = item.Quantity,
            });
        }

        _context.Add(order);
        await _context.SaveChangesAsync();
        return order.Id;
    }

    public async Task<bool> UpdateStripDataAsync(Order order)
    {
        var result = await _context.Orders.Where(a => a.Id == order.Id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(b => b.SessionId, order.SessionId)
                .SetProperty(b => b.PaymentIntentId, order.PaymentIntentId)
                .SetProperty(b => b.LastModified, DateTime.Now));

        var result2 = await _context.SaveChangesAsync();

        return result > 0;
    }

    public async Task<bool> UpdateOrderStatusAsync(string id, OrderStatus status)
    {
        var result = await _context.Orders.Where(a => a.Id == id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(b => b.PaymentDate, DateTime.Now)
                .SetProperty(b => b.Status, status.ToString())
                .SetProperty(b => b.LastModified, DateTime.Now));

        return result > 0;
    }

    public async Task<bool> DeleteOrderAsync(string id)
    {
        var result = await _context.Orders.Where(a => a.Id == id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(b => b.IsDeleted, true)
                .SetProperty(b => b.Deleted, DateTime.Now));

        return result > 0;
    }
}
