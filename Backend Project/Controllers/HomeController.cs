using Backend_Project.Data;
using Backend_Project.Models;
using Backend_Project.Services.Interfaces;
using Backend_Project.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Backend_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ISliderService _sliderService;
        private readonly IAdvertisingService _advertisingService;
        private readonly IProductService _productService;
        public HomeController(ISliderService sliderService,
                              IAdvertisingService advertisingService,
                              AppDbContext context,
                              IProductService productService)
        {
            _sliderService = sliderService;
            _advertisingService = advertisingService;
            _context = context;
            _productService = productService;
        }
        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _sliderService.GetAll();
            List<Advertising> advertisings = await _advertisingService.GetAll();
            Dictionary<string, string> headerBackground = _context.HeaderBackgrounds.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);
            List<Product> products = await _productService.GetAll();

            HomeVM model = new()
            {
                Sliders = sliders,
                Advertisings = advertisings,
                HeaderBackgrounds = headerBackground,
                Products = products
            };
            return View(model);
        }
    }
}