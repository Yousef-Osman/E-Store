using E_Store.Data;
using E_Store.Models.Entities;
using E_Store.Repositories.interfaces;

namespace E_Store.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Product> GetProductAsync(string id)
    {
        return await _context.Products.FindAsync(id);
    }

    public Task<List<Product>> GetProductsAsync()
    {
        throw new NotImplementedException();
    }
}
