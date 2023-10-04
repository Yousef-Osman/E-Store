using Microsoft.AspNetCore.Mvc;

namespace E_Store.Controllers;
public class ShoppingCartController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
