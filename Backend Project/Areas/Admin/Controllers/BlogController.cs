using Backend_Project.Areas.Admin.ViewModels;
using Backend_Project.Data;
using Backend_Project.Helpers;
using Backend_Project.Models;
using Backend_Project.Services;
using Backend_Project.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace Backend_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private IBlogService _blogService;
        public BlogController(AppDbContext context,
                                IWebHostEnvironment env,
                                IBlogService blogService)

        {
            _context = context;
            _env = env;
            _blogService = blogService;
        }


        public async Task<IActionResult> Index()
        {
            IEnumerable<BLog> bLogs = await _context.BLogs.Include(c => c.Images).Include(c => c.Author).Where(m => !m.SoftDelete).ToListAsync();
            return View(bLogs);
        }


        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            BLog bLog = await _context.BLogs.Include(c => c.Images).Include(c => c.Author).FirstOrDefaultAsync(m => m.Id == id);
            if (bLog is null) return NotFound();
            return View(bLog);
        }



        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.author = await GetAuthorAsync();
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BLogCreateVM blog)
        {
            try
            {
                ViewBag.author = await GetAuthorAsync();

                if (!ModelState.IsValid)
                {
                    return View();
                }

                foreach (var photo in blog.Photos)
                {
                    if (!photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "File type must be image");
                        return View();
                    }


                    if (!photo.CheckFileSize(200))
                    {
                        ModelState.AddModelError("Photo", "Image size must be max 200kb");
                        return View();
                    }
                }


                List<BlogImage> blogImages = new();

                foreach (var photo in blog.Photos)
                {
                    string fileName = Guid.NewGuid().ToString() + "_" + photo.FileName;

                    string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", fileName);

                    await FileHelper.SaveFileAsync(path, photo);

                    BlogImage blogImage = new()
                    {
                        Image = fileName
                    };

                    blogImages.Add(blogImage);
                }


                BLog newBlog = new()
                {
                    Title = blog.Title,
                    Description = blog.Description,
                    AuthorId =blog.AuthorId,
                    Images = blogImages
                };

                await _context.BlogImages.AddRangeAsync(blogImages);
                await _context.BLogs.AddAsync(newBlog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null) return BadRequest();

                BLog dbBlog = await _blogService.GetById(id);

                if (dbBlog == null) return NotFound();

                foreach (var photo in dbBlog.Images)
                {

                    string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", photo.Image);

                    FileHelper.DeleteFile(path);


                }

                _context.BLogs.Remove(dbBlog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                throw;
            }


        }






        private async Task<SelectList> GetAuthorAsync()
        {
            IEnumerable<Author> authors = await _blogService.GetAuthorsAsync();
            return new SelectList(authors, "Id", "Name");
        }




        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();

            ViewBag.authors = await GetAuthorAsync();


            BLog dbBlog = await _blogService.GetById((int)id);

            if (dbBlog == null) return NotFound();


            BlogUpdateVM model = new()
            {
                Title = dbBlog.Title,
                AuthorId = dbBlog.AuthorId,
                Images = dbBlog.Images.ToList(),
                Description = dbBlog.Description
            };


            return View(model);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, BlogUpdateVM updatedBlog)
        {

            if (id == null) return BadRequest();

            ViewBag.authors = await GetAuthorAsync();

            BLog dbBlog = await _blogService.GetById(id);

            if (dbBlog == null) return NotFound();

            BlogUpdateVM model = new()
            {
                Title = dbBlog.Title,
                AuthorId = dbBlog.AuthorId,
                Images = dbBlog.Images.ToList(),
                Description = dbBlog.Description
            };


            if (!ModelState.IsValid)
            {
                model.Images = dbBlog.Images.ToList();
                return View(model);
            }


            if (updatedBlog.Photo is not null)
            {
                foreach (var photo in updatedBlog.Photo)
                {
                    if (!photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "File type must be image");
                        return View(model);
                    }

                    if (!photo.CheckFileSize(200))
                    {
                        ModelState.AddModelError("Photo", "Image size must be max 200kb");
                        return View(model);
                    }
                }

                foreach (var item in dbBlog.Images)
                {
                    string dbPath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", item.Image);

                    FileHelper.DeleteFile(dbPath);
                }

                List<BlogImage> blogImages = new();

                foreach (var photo in updatedBlog.Photo)
                {

                    string fileName = Guid.NewGuid().ToString() + "_" + photo.FileName;

                    string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", fileName);

                    await FileHelper.SaveFileAsync(path, photo);

                    BlogImage blogImage = new()
                    {
                        Image = fileName
                    };

                    blogImages.Add(blogImage);
                }

                await _context.BlogImages.AddRangeAsync(blogImages);
                dbBlog.Images = blogImages;
            }
            else
            {
                BLog blog = new()
                {
                    Images = dbBlog.Images
                };
            }


            dbBlog.Title = updatedBlog.Title;
            dbBlog.Description = updatedBlog.Description;
            dbBlog.AuthorId = updatedBlog.AuthorId;


            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
