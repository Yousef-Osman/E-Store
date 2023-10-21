using E_Store.Data;
using E_Store.Models.Entities;
using E_Store.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_Store.Repositories;

public class BrandRepository: IBrandRepository
{
    private readonly ApplicationDbContext _context;

    public BrandRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Brand>> GetBrandsImages()
    {
        return await _context.Brands
            .Select(a => new Brand { Name = a.Name, ImageUrl = a.ImageUrl })
            .AsNoTracking()
            .ToListAsync();
    }
}
