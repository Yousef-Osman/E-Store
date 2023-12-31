﻿using E_Store.Data;
using E_Store.Helpers;
using E_Store.Models.Entities;
using E_Store.Repositories.interfaces;
using E_Store.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
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

    public IQueryable<Product> GetDataQuery([Optional] string userId)
    {
        return _context.Products
            .Where(a=> string.IsNullOrEmpty(userId) ? true : a.VendorId == userId)
            .Include(a => a.Brand)
            .Include(a => a.Categories)
            .ThenInclude(b => b.Category)
            .AsNoTracking();
    }

    public async Task<Product> GetProductAsync(string id)
    {
        return await _context.Products
            .Include(a => a.Brand)
            .Include(a => a.Categories)
            .ThenInclude(b => b.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<bool> CreateAsync(ProductAddVM model, string userId)
    {
        var fileName = await SaveFile(model.ImageFile);

        var product = new Product
        {
            Name = model.Name,
            Description = model.Description,
            Stock = model.Stock,
            Price = model.Price,
            ImageUrl = $"{FileSettings.ImagesPath}{fileName}",
            VendorId = userId,
            BrandId = model.SelectedBrand,
            Categories = model.SelectedCategories.Select(a => new ProductCategory { CategoryId = a }).ToList(),
        };

        _context.Add(product);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateAsync(ProductEditVM model)
    {
        var product = await _context.Products
            .Include(a => a.Categories)
            .FirstOrDefaultAsync(a => a.Id == model.Id);

        if (product == null)
            return false;

        if (model.ImageFile != null)
        {
            var fileName = product.ImageUrl.Replace(FileSettings.ImagesPath, "");
            await SaveFile(model.ImageFile, fileName);
        }

        product.Name = model.Name;
        product.Description = model.Description;
        product.Stock = model.Stock;
        product.Price = model.Price;
        product.BrandId = model.SelectedBrand;
        product.Categories = model.SelectedCategories.Select(a => new ProductCategory { CategoryId = a }).ToList();
        product.LastModified = DateTime.Now;

        _context.Update(product);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(a => a.Id == id);

        if (product == null)
            return false;

        var isUsed = await _context.OrdersDetails.AnyAsync(a => a.ProductId == id) ||
                     await _context.CartItems.AnyAsync(a => a.ProductId == id);

        if (isUsed)
        {
            product.IsDeleted = true;
            product.Deleted = DateTime.Now;
            _context.Update(product); //soft delete
        }
        else
        {
            File.Delete($"{_environment.WebRootPath}/{product.ImageUrl}");
            _context.Remove(product); //hard delete
        }

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<List<SelectListItem>> GetBrandListAsync()
    {
        return await _context.Brands
            .Select(a => new SelectListItem() { Value = a.Id.ToString(), Text = a.Name })
            .OrderBy(a => a.Text)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<SelectListItem>> GetCategoryListAsync()
    {
        return await _context.Categories
            .Select(a => new SelectListItem() { Value = a.Id.ToString(), Text = a.Name })
            .OrderBy(a => a.Text)
            .AsNoTracking()
            .ToListAsync();
    }

    private async Task<string> SaveFile(IFormFile file, string fileName = null)
    {
        var folerPath = $"{_environment.WebRootPath}/{FileSettings.ImagesPath}";
        fileName ??= $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(folerPath, fileName);

        using var stream = File.Create(filePath);
        await file.CopyToAsync(stream);

        return fileName;
    }
}
