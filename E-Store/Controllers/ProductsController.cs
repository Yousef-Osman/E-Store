using AutoMapper;
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
        var products = await _productRepo.GetProductsAsync();
        var model = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductVM>>(products);

        return View(model);
    }
}
