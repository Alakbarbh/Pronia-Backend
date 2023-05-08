using Backend_Project.Data;
using Backend_Project.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_Project.Controllers
{
    public class AboutController : Controller
    {

        private readonly AppDbContext _context;
        public AboutController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            Dictionary<string, string> headerBackground = _context.HeaderBackgrounds.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);

            AboutVM model = new()
            {
                HeaderBackgrounds = headerBackground
            };

            return View(model);
        }

       
    }
}
