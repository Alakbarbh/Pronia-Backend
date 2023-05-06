using Microsoft.AspNetCore.Mvc;

namespace Backend_Project.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
