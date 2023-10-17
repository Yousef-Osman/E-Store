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

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> GetData([FromForm] int start, [FromForm] int length = 10)
    {
        try
        {
            var searchValue = Request.Form["search[value]"].ToString().ToLower();
            var columnOrder = Request.Form["order[0][column]"];
            var sortColumn = Request.Form[string.Concat("columns[", columnOrder, "][name]")];
            var sortColumnDirection = Request.Form["order[0][dir]"];

            var products = await _productRepo.GetProductsAsync();
            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductVM>>(products);


            //var query = _surveyRepo.GetDataQuery();
            //var data = await query.Skip(start).Take(length).ToListAsync();
            var totalCount = 24;

            return Json(new
            {
                data = data.Take(10),
                recordsFiltered = totalCount,
                recordsTotal = totalCount
            });
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
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
        var model = await FillModel(new ProductAddVM());
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductAddVM model)
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

    public async Task<IActionResult> Edit(string id)
    {
        var product = await _productRepo.GetProductAsync(id);

        if (product == null)
            return NotFound();

        var model = _mapper.Map<Product, ProductEditVM>(product);
        model = await FillModel(model);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProductEditVM model)
    {
        if (!ModelState.IsValid)
        {
            model = await FillModel(model);
            return View(model);
        }

        var success = await _productRepo.UpdateAsync(model);

        if (!success)
            return StatusCode(StatusCodes.Status500InternalServerError);

        return RedirectToAction(nameof(Details), new { id = model.Id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest();

        var success = await _productRepo.DeleteAsync(id);

        if (!success)
            return StatusCode(StatusCodes.Status500InternalServerError);

        return RedirectToAction(nameof(Index));
    }

    private async Task<G> FillModel<G>(G model) where G : IProductLists
    {
        model.Brands = await _productRepo.GetBrandListAsync();
        model.Categories = await _productRepo.GetCategoryListAsync();
        return model;
    }
}
