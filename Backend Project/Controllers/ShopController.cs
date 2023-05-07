using Backend_Project.Data;
using Backend_Project.Models;
using Backend_Project.Services.Interfaces;
using Backend_Project.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Project.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IColorService _colorService;
        public ShopController(AppDbContext context,
                              ICategoryService categoryService,
                              IProductService productService,
                              IColorService colorService)
        {
            _context = context;
            _categoryService = categoryService;
            _productService = productService;
            _colorService = colorService;
        }
        public async Task<IActionResult> Index()
        {
            Dictionary<string, string> headerBackground = _context.HeaderBackgrounds.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);
            List<Category> categories = await _categoryService.GetCategories();
            List<Product> newProducts = await _productService.GetNewProducts();
            List<Color> colors = await _colorService.GetAllColors();

            ShopVM model = new()
            {
                HeaderBackgrounds = headerBackground,
                Categories = categories,
                NewProduct = newProducts,
                Colors = colors
            };

            return View(model);
        }
    }
}
