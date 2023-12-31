﻿using E_Store.Models.Entities;
using E_Store.Models.Enums;
using E_Store.ViewModels;

namespace E_Store.Repositories.interfaces;

public interface IOrderRepository
{
    public IQueryable<Order> GetDataQuery(string UserId);
    public Task<Order> GetOrderAsync(string id);
    public Task<Order> GetOrderWithDetailsAsync(string id);
    public Task<List<OrderDetail>> GetOrderDetailsAsync(string id);
    public Task<string> AddOrderAsync(OrderVM orderVM, string userId);
    public Task<bool> UpdateStripDataAsync(Order order);
    public Task<bool> UpdateOrderStatusAsync(string id, OrderStatus status);
    public Task<bool> DeleteOrderAsync(string id);
}
