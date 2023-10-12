using AutoMapper;
using E_Store.Models.Entities;
using E_Store.Repositories.interfaces;
using E_Store.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace E_Store.Controllers;
public class ProductsController : Controller
{
    private readonly IProductRepository _productRepo;
    private readonly IMapper _mapper;

    public ProductsController(IProductRepository productRepo,
                              IMapper mapper)
    {
        _productRepo = productRepo;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _productRepo.GetProductsAsync();
        var model = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductVM>>(products);

        return View(model);
    }

    public IActionResult Details(string id)
    {
        return View();
    }

    public async Task<IActionResult> Create()
    {
        var model = new ProductEditVM
        {
            Brands = await _productRepo.GetBrandListAsync(),
            Categories = await _productRepo.GetCategoryListAsync()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductEditVM model)
    {
        if (!ModelState.IsValid)
        {
            model.Brands = await _productRepo.GetBrandListAsync();
            model.Categories = await _productRepo.GetCategoryListAsync();
            return View(model);
        }

        var data = _mapper.Map<ProductEditVM, Product>(model);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(ProductVM model)
    {
        return View(model);
    }

    [HttpPost]
    public IActionResult Delete(string id)
    {
        return RedirectToAction(nameof(Index));
    }
}
