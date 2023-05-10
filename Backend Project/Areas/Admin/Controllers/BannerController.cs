using Backend_Project.Areas.Admin.ViewModels;
using Backend_Project.Data;
using Backend_Project.Helpers;
using Backend_Project.Models;
using Backend_Project.Services;
using Backend_Project.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BannerController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private IBannerService _bannerService;
        public BannerController(AppDbContext context,
                                IWebHostEnvironment env,
                                IBannerService bannerService)

        {
            _context = context;
            _env = env;
            _bannerService = bannerService;
        }

        public async Task<IActionResult> Index()
        {
            List<Banner> banners = await _context.Banners.ToListAsync();
            return View(banners);
        }


        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            Banner banner = await _bannerService.GetById(id);
            if (banner is null) return NotFound();
            return View(banner);
        }



        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BannerCreateVM banner)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }



                if (!banner.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File type must be image");
                    return View();
                }




                if (!banner.Photo.CheckFileSize(200))
                {
                    ModelState.AddModelError("Photo", "Image size must be max 200kb");
                    return View();
                }



                string fileName = Guid.NewGuid().ToString() + "_" + banner.Photo.FileName;


                string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", fileName);

                await FileHelper.SaveFileAsync(path, banner.Photo);

                Banner newBanner = new()
                {
                    Image = fileName,
                    Name = banner.Name,
                    Title = banner.Title
                };


                await _context.Banners.AddAsync(newBanner);



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

                Banner dbBanner = await _bannerService.GetById(id);

                if (dbBanner is null) return NotFound();

                string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", dbBanner.Image);

                FileHelper.DeleteFile(path);

                _context.Banners.Remove(dbBanner);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                return View();
            }
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();
            Banner dbBanner = await _bannerService.GetById(id);
            if (dbBanner is null) return NotFound();

            BannerUpdateVM model = new()
            {
                Image = dbBanner.Image,
                Name = dbBanner.Name,
                Title = dbBanner.Title,
            };

            return View(model);

        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, BannerUpdateVM bannerUpdate)
        {
            try
            {

                if (id == null) return BadRequest();

                Banner dbBanner = await _bannerService.GetById(id);

                if (dbBanner is null) return NotFound();

                BannerUpdateVM model = new()
                {
                    Image = dbBanner.Image,
                    Name = dbBanner.Name,
                    Title = dbBanner.Title,
                };


                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (bannerUpdate.Photo != null)
                {
                    if (!bannerUpdate.Photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "Please choose correct image type");
                        return View(model);
                    }

                    if (!bannerUpdate.Photo.CheckFileSize(200))
                    {
                        ModelState.AddModelError("Photo", "Image size must be max 200kb");
                        return View(model);
                    }


                    string dbPath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", dbBanner.Image);

                    FileHelper.DeleteFile(dbPath);


                    string fileName = Guid.NewGuid().ToString() + "_" + bannerUpdate.Photo.FileName;

                    string newPath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", fileName);

                    await FileHelper.SaveFileAsync(newPath, bannerUpdate.Photo);

                    dbBanner.Image = fileName;
                }
                else
                {
                    Banner banner = new()
                    {
                        Image = dbBanner.Image
                    };
                }


                dbBanner.Name = bannerUpdate.Name;
                dbBanner.Title = bannerUpdate.Title;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                @ViewBag.error = ex.Message;
                return View();
            }
        }
    }
}
