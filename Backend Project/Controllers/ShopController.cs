using Backend_Project.Data;
using Backend_Project.Models;
using Backend_Project.Services.Interfaces;
using Backend_Project.ViewModels;
using EntityFramework_Slider.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> Index(int page = 1,int take = 4)
        {
            List<Product> paginateProduct = await _productService.GetPaginateDatas(page, take);
            int pageCount = await GetPageCountAsync(take);
            Paginate<Product> paginateDatas = new(paginateProduct, page, pageCount);

            Dictionary<string, string> headerBackground = _context.HeaderBackgrounds.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);

            List<Category> categories = await _categoryService.GetCategories();
            List<Product> newProducts = await _productService.GetNewProducts();
            List<Product> products = await _productService.GetAll();
            List<Color> colors = await _colorService.GetAllColors();

            ShopVM model = new()
            {
                HeaderBackgrounds = headerBackground,
                Categories = categories,
                NewProduct = newProducts,
                Products = products,
                Colors = colors,
                PaginateProduct = paginateDatas
            };

            return View(model);
        }

        public async Task<IActionResult> GetProductByCategory(int? id)
        {
            List<Product> products = await _context.ProductCategories.Where(m => m.CategoryId == id).Select(m => m.Product).ToListAsync();

            return PartialView("_ProductsPartial", products);

        }

        public async Task<IActionResult> GetAllProduct(int? id)
        {
            List<Product> products = await _productService.GetAll();

            return PartialView("_ProductsPartial", products);

        }

        public async Task<IActionResult> GetAllProductByColor(int? id)
        {
            List<Product> products = await _productService.GetAll();

            return PartialView("_ProductsPartial", products);

        }


        public async Task<IActionResult> GetProductByColor(int? id)
        {
            List<Product> products = await _context.Products.Include(m => m.Color).Where(m=>m.Color.Id == id).ToListAsync();

            return PartialView("_ProductsPartial", products);

        }

        private async Task<int> GetPageCountAsync(int take)
        {
            var productCount = await _productService.GetCountAsync();
            return (int)Math.Ceiling((decimal)productCount / take);
        }

        public async Task<IActionResult> GetProductFilteredByPrice(string icon)
        {
            List<Product> products = await _context.Products.OrderByDescending(m=>m.Price).ToListAsync();

            return PartialView("_ProductsPartial", products);

        }
    }
}
