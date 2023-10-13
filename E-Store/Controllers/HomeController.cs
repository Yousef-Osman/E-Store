using AutoMapper;
using E_Store.Data;
using E_Store.Models;
using E_Store.Models.Entities;
using E_Store.Repositories.interfaces;
using E_Store.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace E_Store.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductRepository _productRepo;
    private readonly IMapper _mapper;

    public HomeController(ILogger<HomeController> logger,
                          IProductRepository productRepo,
                          IMapper mapper)
    {
        _logger = logger;
        _productRepo = productRepo;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _productRepo.GetProductsAsync();
        var model = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductVM>>(products);

        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
