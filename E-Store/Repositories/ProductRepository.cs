using E_Store.Data;
using E_Store.Models.Entities;
using E_Store.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

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
        return await _context.Products
            .Include(a => a.Brand)
            .Include(a => a.Category)
            .FirstOrDefaultAsync(a=>a.Id == id);
    }

    public async Task<IReadOnlyList<Product>> GetProductsAsync()
    {
        return await _context.Products
            .Include(a=>a.Brand)
            .Include(a=>a.Category)
            .ToListAsync();
    }
}
