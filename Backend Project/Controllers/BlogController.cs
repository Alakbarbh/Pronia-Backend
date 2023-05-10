﻿using Backend_Project.Data;
using Backend_Project.Models;
using Backend_Project.Services;
using Backend_Project.Services.Interfaces;
using Backend_Project.ViewModels;
using EntityFramework_Slider.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Backend_Project.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IBlogService _blogService;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;
        public BlogController(AppDbContext context,
                              IBlogService blogService,
                              IProductService productService,
                              ICategoryService categoryService,
                              ITagService tagService)
        {
            _context = context;
            _blogService = blogService;
            _productService = productService;
            _categoryService = categoryService;
            _tagService = tagService;
        }


        public async Task<IActionResult> Index(int page = 1, int take = 2)
        {
            Dictionary<string, string> headerBackground = _context.HeaderBackgrounds.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);
            List<BLog> bLogs = await _blogService.GetBlogs();

            List<BLog> paginateProduct = await _blogService.GetPaginateDatas(page, take);
            int pageCount = await GetPageCountAsync(take);
            Paginate<BLog> paginateDatas = new(paginateProduct, page, pageCount);


            List<Category> categories = await _categoryService.GetCategories();
            List<Product> products = await _productService.GetNewProducts();
            List<Tag> tags = await _tagService.GetAllAsync();
            List<Product> newProducts = await _productService.GetNewProducts();

            BlogVM model = new()
            {
                HeaderBackgrounds = headerBackground,
                BLogs = bLogs,
                PaginateBlog = paginateDatas,
                Categories = categories,
                Product = products,
                Tags = tags,
                NewProduct = newProducts
            };

            return View(model);
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


        private async Task<int> GetPageCountAsync(int take)
        {
            var blogCount = await _productService.GetCountAsync();
            return (int)Math.Ceiling((decimal)blogCount / take);
        }


        
    }
}
