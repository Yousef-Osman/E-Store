using E_Store.Data;
using E_Store.Models.Entities;
using E_Store.Repositories.interfaces;
using E_Store.ViewModels;

namespace E_Store.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _context;

    public OrderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AddOrderAsync(OrderVM orderVM, string userId)
    {
        var orderDetails = new List<OrderDetail>();

        foreach (var item in orderVM.CartItems)
        {
            var orderDetail = new OrderDetail()
            {
                ProductId = item.ProductId,
                Price = item.Price,
                Quantity = item.Quantity,
            };

            orderDetails.Add(orderDetail);
        }


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
            OrderDetails = orderDetails
        };

        _context.Add(order);
        return await _context.SaveChangesAsync() > 0;
    }

    public Task<Order> GetOrderAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Order>> GetOrdersAsync()
    {
        throw new NotImplementedException();
    }
}
