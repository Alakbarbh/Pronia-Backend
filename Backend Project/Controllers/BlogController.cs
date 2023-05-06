using Microsoft.AspNetCore.Mvc;

namespace Backend_Project.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
