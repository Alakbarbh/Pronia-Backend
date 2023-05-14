using Backend_Project.Data;
using Backend_Project.Models;
using Backend_Project.Services;
using Backend_Project.Services.Interfaces;
using Backend_Project.ViewModels;
using Backend_Project.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Reflection.Metadata;

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


        public async Task<IActionResult> Index(int page = 1, int take = 4)
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
                                            //.Include(m => m.Comments)
                                            .Where(m => m.Name.ToLower().Contains(searchText.ToLower()))
                                            .Take(5)
                                            .ToListAsync();
            return PartialView("_SearchPartial", products);
        }


        private async Task<int> GetPageCountAsync(int take)
        {
            var blogCount = await _blogService.GetCountAsync();
            return (int)Math.Ceiling((decimal)blogCount / take);
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

        public async Task<IActionResult> GetProductsByTag(int? id)
        {
            List<Product> products = await _context.ProductTags.Where(m => m.Tag.Id == id).Select(m => m.Product).ToListAsync();

            return PartialView("_ProductsPartial", products);
        }



        public async Task<IActionResult> BlogDetail(int? id)
        {
            BLog blogDt = await _blogService.GetById((int)id);
            Dictionary<string, string> headerBackgrounds = _context.HeaderBackgrounds.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);
            List<Category> categories = await _categoryService.GetCategories();
            List<BLog> blogs = await _blogService.GetBlogs();
            List<Tag> tags = await _tagService.GetAllAsync();
            List<Product> newProduct = await _productService.GetNewProducts();


            List<BlogComment> blogComments = await _context.BlogComments.Include(m => m.AppUser).Where(m => m.BlogId == id).ToListAsync();
            CommentVM commentVM = new CommentVM();


            BlogDetailVM model = new()
            {
                BlogDt = blogDt,
                HeaderBackgrounds = headerBackgrounds,
                BlogComments = blogComments,
                CommentVM = commentVM,
                Categories = categories,
                Blogs = blogs,
                Tags = tags,
                NewProducts = newProduct
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> PostComment(BlogDetailVM blogDetailVM, string userId, int blogId)
        {
            if (blogDetailVM.CommentVM.Message == null)
            {
                ModelState.AddModelError("Message", "Don't empty");
                return RedirectToAction(nameof(BlogDetail), new { id = blogId });
            }

            BlogComment blogComment = new()
            {
                FullName = blogDetailVM.CommentVM?.FullName,
                Email = blogDetailVM.CommentVM?.Email,
                Subject = blogDetailVM.CommentVM?.Subject,
                Message = blogDetailVM.CommentVM?.Message,
                AppUserId = userId,
                BlogId = blogId
            };

            await _context.BlogComments.AddAsync(blogComment);
            await _context.SaveChangesAsync();
        

            return RedirectToAction(nameof(BlogDetail), new { id = blogId });

        }

    }
}
