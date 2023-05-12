using Backend_Project.Data;
using Backend_Project.Models;
using Backend_Project.Services.Interfaces;
using Backend_Project.ViewModels;
using Backend_Project.Helpers;
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
        private readonly ITagService _tagService;
        private readonly IAdvertisingService _advertisingService;
        public ShopController(AppDbContext context,
                              ICategoryService categoryService,
                              IProductService productService,
                              IColorService colorService,
                              ITagService tagService,
                              IAdvertisingService advertisingService)
        {
            _context = context;
            _categoryService = categoryService;
            _productService = productService;
            _colorService = colorService;
            _tagService = tagService;
            _advertisingService = advertisingService;
        }
        public async Task<IActionResult> Index(int page = 1,int take = 5,int? cateId = null)
        {
            List<Product> paginateProduct = await _productService.GetPaginateDatas(page, take,cateId);
            int pageCount = await GetPageCountAsync(take);
            Paginate<Product> paginateDatas = new(paginateProduct, page, pageCount);

            Dictionary<string, string> headerBackground = _context.HeaderBackgrounds.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);

            List<Category> categories = await _categoryService.GetCategories();
            List<Product> newProducts = await _productService.GetNewProducts();
            List<Product> products = await _productService.GetAll();
            List<Color> colors = await _colorService.GetAllColors();
            List<Tag> tags = await _tagService.GetAllAsync();

            ShopVM model = new()
            {
                HeaderBackgrounds = headerBackground,
                Categories = categories,
                NewProduct = newProducts,
                Products = products,
                Colors = colors,
                PaginateProduct = paginateDatas,
                Tags = tags
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


        public async Task<IActionResult> MainSearch(string searchText)
        {
            var products = await _context.Products
                                .Include(m => m.Images)
                                .Include(m => m.ProductCategories)?
                                .OrderByDescending(m => m.Id)
                                .Where(m => !m.SoftDelete && m.Name.ToLower().Trim().Contains(searchText.ToLower().Trim()))
                                .Take(6)
                                .ToListAsync();

            return View(products);
        }



        public async Task<IActionResult> Search(string searchText)
        {
            List<Product> products = await _context.Products.Include(m => m.Images)
                                            .Include(m => m.ProductCategories)
                                            .Include(m => m.ProductSizes)
                                            .Include(m => m.ProductTags)
                                            .Include(m => m.Comments)
                                            .Where(m => m.Name.ToLower().Contains(searchText.ToLower()))
                                            .Take(5)
                                            .ToListAsync();
            return PartialView("_SearchPartial", products);
        }


        public async Task<IActionResult> ProductDetail(int? id)
        {
            Product product = await _productService.GettFullDataById((int)id);
            Dictionary<string, string> headerBackground = _context.HeaderBackgrounds.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);
            List<Product> products = await _productService.GetAll();
            List<Advertising> advertisings = await _advertisingService.GetAll();

            //var related = await _context.Products.Include(m => m.ProductCategories).ThenInclude(m => m.Category).ToListAsync();

            List<Category> categories = await _categoryService.GetCategories();
            List<Product> relatedProducts = new();
            
            foreach (var category in categories)
            {
                Product relatedProduct = await _context.ProductCategories.Where(m => m.Category.Id == category.Id).Select(m => m.Product).FirstAsync();
                relatedProducts.Add(relatedProduct);
            }

            

            ProductDetailVM model = new()
            {
                ProductDetail = product,
                HeaderBackgrounds = headerBackground,
                Products = products,
                Advertisings = advertisings,
            };
            return View(model);
        }



        public async Task<IActionResult> GetProductsByTag(int? id)
        {
            List<Product> products = await _context.ProductTags.Where(m => m.Tag.Id == id).Select(m => m.Product).ToListAsync();

            return PartialView("_ProductsPartial", products);
        }


    }
}
