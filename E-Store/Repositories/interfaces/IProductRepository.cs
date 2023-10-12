using E_Store.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_Store.Repositories.interfaces;

public interface IProductRepository
{
    public Task<IReadOnlyList<Product>> GetProductsAsync();
    public Task<Product> GetProductAsync(string id);
    public Task<List<SelectListItem>> GetBrandListAsync();
    public Task<List<SelectListItem>> GetCategoryListAsync();
}
