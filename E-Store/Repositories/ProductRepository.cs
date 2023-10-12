using E_Store.Data;
using E_Store.Models.Entities;
using E_Store.Repositories.interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IReadOnlyList<Product>> GetProductsAsync()
    {
        return await _context.Products
            .Include(a => a.Brand)
            .Include(a => a.Category)
            .ToListAsync();
    }

    public async Task<List<SelectListItem>> GetBrandListAsync()
    {
        return await _context.Brands
            .Select(a => new SelectListItem() { Value = a.Id.ToString(), Text = a.Name })
            .OrderBy(a => a.Text)
            .ToListAsync();
    }

    public async Task<List<SelectListItem>> GetCategoryListAsync()
    {
        return await _context.Categories
            .Select(a=>new SelectListItem() { Value = a.Id.ToString(), Text = a.Name})
            .OrderBy(a => a.Text)
            .ToListAsync();
    }
}
