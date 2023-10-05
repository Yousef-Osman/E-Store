using E_Store.Models.Entities;
using E_Store.ViewModels;

namespace E_Store.Repositories.interfaces;

public interface IOrderRepository
{
    public Task<List<Order>> GetOrdersAsync();
    public Task<Order> GetOrderAsync(string id);
    public Task<bool> AddOrderAsync(OrderVM orderVM, string userId);
}
