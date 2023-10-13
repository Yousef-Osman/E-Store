using E_Store.Data;
using E_Store.Helpers;
using E_Store.Models.Entities;
using E_Store.Repositories.interfaces;
using E_Store.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace E_Store.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _environment;
    private readonly IHttpContextAccessor _httpContext;

    public ProductRepository(ApplicationDbContext context,
                             IWebHostEnvironment environment,
                             IHttpContextAccessor httpContext)
    {
        _context = context;
        _environment = environment;
        _httpContext = httpContext;
    }

    public async Task<IReadOnlyList<Product>> GetProductsAsync()
    {
        return await _context.Products
            .Include(a => a.Brand)
            .Include(a => a.Categories)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Product>> GetVendorProductsAsync()
    {
        var vendorId = GetCurrentUserId();

        return await _context.Products
            .Where(a => a.VendorId == vendorId)
            .Include(a => a.Brand)
            .Include(a => a.Categories)
            .OrderByDescending(a => a.Created)
            .ToListAsync();
    }

    public async Task<Product> GetProductAsync(string id)
    {
        return await _context.Products
            .Include(a => a.Brand)
            .Include(a => a.Categories)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<bool> CreateAsync(ProductEditVM model)
    {
        var fileName = await SaveFile(model.ImageFile);

        var product = new Product {
            Name = model.Name,
            Description = model.Description,
            Stock = model.Stock,
            Price = model.Price,
            ImageUrl = $"{FileSettings.ImagesPath}{fileName}",
            VendorId = GetCurrentUserId(),
            BrandId = model.SelectedBrand,
            Categories = model.SelectedCategories.Select(a=> new ProductCategory { CategoryId = a }).ToList(),
        };

        _context.Add(product);
        return await _context.SaveChangesAsync() > 0;
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
            .Select(a => new SelectListItem() { Value = a.Id.ToString(), Text = a.Name })
            .OrderBy(a => a.Text)
            .ToListAsync();
    }

    private async Task<string> SaveFile(IFormFile file)
    {
        var folerPath = $"{_environment.WebRootPath}/{FileSettings.ImagesPath}";
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(folerPath, fileName);

        using var stream = File.Create(filePath);
        await file.CopyToAsync(stream);

        return fileName;
    }

    private string GetCurrentUserId()
    {
        return _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
