using Backend_Project.Areas.Admin.ViewModels;
using Backend_Project.Data;
using Backend_Project.Models;
using Backend_Project.Services;
using Backend_Project.Services.Interfaces;
using Backend_Project.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Backend_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IColorService _colorService;
        private readonly ISizeService _sizeService;
        private readonly ITagService _tagService;
        public ProductController(AppDbContext context,
                                IWebHostEnvironment env,
                                IProductService productService,
                                ICategoryService categoryService,
                                IColorService colorService,
                                ISizeService sizeService,
                                ITagService tagService)
        {
            _context = context;
            _env = env;
            _productService = productService;
            _categoryService = categoryService;
            _colorService = colorService;
            _sizeService = sizeService;
            _tagService = tagService;
        }
        public async Task<IActionResult> Index(int page = 1, int take = 5,int? cateId = null)
        {
            List<Product> products = await _productService.GetPaginateDatas(page, take,cateId);
            List<ProductListVM> mappedDatas = GetMappedDatas(products);
            int pageCount = await GetPageCountAsync(take);
            Paginate<ProductListVM> paginatedDatas = new(mappedDatas, page, pageCount);
            ViewBag.take = take;
            return View(paginatedDatas);
        }

        private async Task<SelectList> GetCategoryAsync()
        {
            IEnumerable<Category> categories = await _categoryService.GetCategories();
            return new SelectList(categories, "Id", "Name");
        }

        private async Task<SelectList> GetColorAsync()
        {
            IEnumerable<Color> colors = await _colorService.GetAllColors();
            return new SelectList(colors, "Id", "Name");
        }

        private async Task<SelectList> GetSizeAsync()
        {
            IEnumerable<Size> sizes = await _sizeService.GetAllSize();
            return new SelectList(sizes, "Id", "Name");
        }

        private async Task<SelectList> GetTagAsync()
        {
            IEnumerable<Tag> tags = await _tagService.GetAllAsync();
            return new SelectList(tags, "Id", "Name");
        }

        private async Task<int> GetPageCountAsync(int take)
        {
            var productCount = await _productService.GetCountAsync();
            return (int)Math.Ceiling((decimal)productCount / take);
        }


        private List<ProductListVM> GetMappedDatas(List<Product> products)
        {
            List<ProductListVM> mappedDatas = new();

            foreach (var product in products)
            {
                ProductListVM productVM = new()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    MainImage = product.MainImage,
                    SKU = product.SKU
                };

                mappedDatas.Add(productVM);

            }

            return mappedDatas;
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();
            Product product = await _productService.GettFullDataById((int)id);
            if (product is null) return NotFound();

            ProductDetailVM model = new()
            {
                Name = product.Name,
                SaleCount = product.SaleCount,
                StockCount = product.StockCount,
                Description = product.Description,
                Price = product.Price,
                SKU = product.SKU,
                ProductCategories = product.ProductCategories,
                ProductImages = product.Images,
                ProductSizes = product.ProductSizes,
                ProductTags = product.ProductTags,
                HoverImage = product.HoverImage,
                MainImage = product.MainImage,
                ColorName = product.Color.Name,
                Rate = product.Rate
            };

            return View(model);
        }
    }
}
