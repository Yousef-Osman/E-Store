﻿using E_Store.Models.Entities;
using E_Store.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_Store.Repositories.interfaces;

public interface IProductRepository
{
    public IQueryable<Product> GetDataQuery();
    public Task<IReadOnlyList<Product>> GetProductsAsync();
    public Task<IReadOnlyList<Product>> GetVendorProductsAsync();
    public Task<Product> GetProductAsync(string id);
    public Task<bool> CreateAsync(ProductAddVM model);
    public Task<bool> UpdateAsync(ProductEditVM model);
    public Task<bool> DeleteAsync(string id);
    public Task<List<SelectListItem>> GetBrandListAsync();
    public Task<List<SelectListItem>> GetCategoryListAsync();
}
