using Backend_Project.Data;
using Backend_Project.Models;
using Backend_Project.Services.Interfaces;
using Backend_Project.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Backend_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ISliderService _sliderService;
        private readonly IAdvertisingService _advertisingService;
        private readonly IProductService _productService;
        private readonly IClientService _clientService;
        private readonly IBrandService _brandService;
        private readonly IBlogService _blogService;
        public HomeController(ISliderService sliderService,
                              IAdvertisingService advertisingService,
                              AppDbContext context,
                              IProductService productService,
                              IClientService clientService,
                              IBrandService brandService,
                              IBlogService blogService)
        {
            _sliderService = sliderService;
            _advertisingService = advertisingService;
            _context = context;
            _productService = productService;
            _clientService = clientService;
            _brandService = brandService;
            _blogService = blogService;
        }
        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _sliderService.GetAll();
            List<Advertising> advertisings = await _advertisingService.GetAll();
            Dictionary<string, string> headerBackground = _context.HeaderBackgrounds.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);
            List<Product> featuredProduct = await _productService.GetFeaturedProducts();
            List<Product> bestsellerProduct = await _productService.GetBestsellerProducts();
            List<Product> latestProduct = await _productService.GetLatestProducts();
            List<Product> newProduct = await _productService.GetNewProducts();
            List<Banner> banners = await _context.Banners.ToListAsync();
            List<Client> clients = await _context.Clients.ToListAsync();
            List<Brand> brands = await _context.Brands.ToListAsync();
            List<BLog> blogs = await _blogService.GetBlogs();

            HomeVM model = new()
            {
                Sliders = sliders,
                Advertisings = advertisings,
                HeaderBackgrounds = headerBackground,
                FeaturedProduct = featuredProduct,
                BestSellerProduct = bestsellerProduct,
                LatestProduct = latestProduct,
                Banners = banners,
                NewProducts = newProduct,
                Clients = clients,
                Brands = brands,
                BLogs = blogs
            };
            return View(model);
        }
    }
}