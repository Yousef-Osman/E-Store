using E_Store.Data;
using E_Store.Models.Entities;
using E_Store.Repositories.interfaces;
using E_Store.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace E_Store.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;

    public CategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetCategoriesImages()
    {
        return await _context.Categories
            .Select(a => new Category { Name = a.Name, ImageUrl = a.ImageUrl })
            .AsNoTracking()
            .ToListAsync();
    }
}
