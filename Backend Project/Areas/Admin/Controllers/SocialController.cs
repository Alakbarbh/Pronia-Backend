using Backend_Project.Data;
using Backend_Project.Models;
using Backend_Project.Services;
using Backend_Project.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SocialController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly ISocialService _socialService;
        public SocialController(AppDbContext context,
                                IWebHostEnvironment env,
                                ISocialService socialService)
        {
            _context = context;
            _env = env;
            _socialService = socialService;
        }
        public async Task<IActionResult> Index()
        {
            List<Social> socials = await _socialService.GetSocials();
            return View(socials);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            Social social = await _socialService.GetById(id);
            if (social is null) return NotFound();
            return View(social);
        }
    }
}
