using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace E_Store.Controllers;
public class ProductsController : Controller
{
    private readonly IMapper _mapper;

    public ProductsController(IMapper mapper)
    {
        _mapper = mapper;
    }
    public IActionResult Index()
    {
        return View();
    }
}
