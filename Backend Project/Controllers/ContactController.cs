using Microsoft.AspNetCore.Mvc;

namespace Backend_Project.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
