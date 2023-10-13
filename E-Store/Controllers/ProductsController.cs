﻿using AutoMapper;
using E_Store.Models.Entities;
using E_Store.Repositories.interfaces;
using E_Store.ViewModels;
using Microsoft.AspNetCore.Mvc;

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
        var products = await _productRepo.GetVendorProductsAsync();
        var model = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductVM>>(products);

        return View(model);
    }

    public async Task<IActionResult> Details(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest();

        var product = await _productRepo.GetProductAsync(id);
        var model = _mapper.Map<Product, ProductVM>(product);
        return View(model);
    }

    public async Task<IActionResult> Create()
    {
        var model = await FillModel(new ProductEditVM());
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductEditVM model)
    {
        if (!ModelState.IsValid)
        {
            model = await FillModel(model);
            return View(model);
        }

        var success = await _productRepo.CreateAsync(model);

        if (!success)
            return StatusCode(StatusCodes.Status500InternalServerError);

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

    private async Task<ProductEditVM> FillModel(ProductEditVM model)
    {
        model.Brands = await _productRepo.GetBrandListAsync();
        model.Categories = await _productRepo.GetCategoryListAsync();
        return model; //return is unnecessary because it's a reference type
    }
}
