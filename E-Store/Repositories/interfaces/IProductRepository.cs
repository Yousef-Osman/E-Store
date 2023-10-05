using E_Store.Models.Entities;

namespace E_Store.Repositories.interfaces;

public interface IProductRepository
{
    public Task<List<Product>> GetProductsAsync();
    public Task<Product> GetProductAsync(string id);

}
