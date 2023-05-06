using Microsoft.AspNetCore.Mvc;

namespace Backend_Project.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
