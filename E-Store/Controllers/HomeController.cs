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
    private readonly int _pageSize = 12;

    public HomeController(ILogger<HomeController> logger,
                          IProductRepository productRepo,
                          IMapper mapper)
    {
        _logger = logger;
        _productRepo = productRepo;
        _mapper = mapper;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> LoadProducts(int pageNumber)
    {
        try
        {
            var query = _productRepo.GetDataQuery();
            var data = await query
                .Skip((pageNumber - 1) * _pageSize)
                .Take(_pageSize)
                .ToListAsync();

            if (data.Count < 1)
                return StatusCode(StatusCodes.Status204NoContent);

            var model = _mapper.Map<List<Product>, List<ProductVM>>(data);

            return PartialView("_LoadProducts", model);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    public async Task<IActionResult> ProductDetails(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest();

        var product = await _productRepo.GetProductAsync(id);
        var model = _mapper.Map<Product, ProductVM>(product);
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
