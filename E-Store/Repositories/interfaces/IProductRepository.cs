using E_Store.Models.Entities;
using E_Store.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Runtime.InteropServices;

namespace E_Store.Repositories.interfaces;

public interface IProductRepository
{
    public IQueryable<Product> GetDataQuery([Optional] string userId);
    public Task<Product> GetProductAsync(string id);
    public Task<bool> CreateAsync(ProductAddVM model, string userId);
    public Task<bool> UpdateAsync(ProductEditVM model);
    public Task<bool> DeleteAsync(string id);
    public Task<List<SelectListItem>> GetBrandListAsync();
    public Task<List<SelectListItem>> GetCategoryListAsync();
}
