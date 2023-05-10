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
    public class AdvertisingController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IAdvertisingService _advertisingService;
        public AdvertisingController(AppDbContext context,
                                IWebHostEnvironment env,
                                IAdvertisingService advertisingService)
        {
            _context = context;
            _env = env;
            _advertisingService = advertisingService;
        }


        public async Task<IActionResult> Index()
        {
            List<Advertising> advertisings = await _context.Advertisings.ToListAsync();
            return View(advertisings);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            Advertising advertising = await _advertisingService.GetById(id);
            if (advertising is null) return NotFound();
            return View(advertising);
        }




        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdvertisingCreateVM advertising)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }



                if (!advertising.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File type must be image");
                    return View();
                }




                if (!advertising.Photo.CheckFileSize(200))
                {
                    ModelState.AddModelError("Photo", "Image size must be max 200kb");
                    return View();
                }



                string fileName = Guid.NewGuid().ToString() + "_" + advertising.Photo.FileName;


                string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", fileName);

                await FileHelper.SaveFileAsync(path, advertising.Photo);

                Advertising newAdvertising = new()
                {
                    Image = fileName,
                    Name = advertising.Name,
                    Description = advertising.Description
                };


                await _context.Advertisings.AddAsync(newAdvertising);



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

                Advertising dbAdvertising = await _advertisingService.GetById(id);

                if (dbAdvertising is null) return NotFound();

                string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", dbAdvertising.Image);

                FileHelper.DeleteFile(path);

                _context.Advertisings.Remove(dbAdvertising);

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
            Advertising dbAdvertising = await _advertisingService.GetById(id);
            if (dbAdvertising is null) return NotFound();

            AdvertisingUpdateVM model = new()
            {
                Image = dbAdvertising.Image,
                Name = dbAdvertising.Name,
                Description = dbAdvertising.Description,
            };

            return View(model);

        }
        



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, AdvertisingUpdateVM advertisingUpdate)
        {
            try
            {

                if (id == null) return BadRequest();

                Advertising dbAdvertising = await _advertisingService.GetById(id);

                if (dbAdvertising is null) return NotFound();

                AdvertisingUpdateVM model = new()
                {
                    Image = dbAdvertising.Image,
                    Name = dbAdvertising.Name,
                    Description = dbAdvertising.Description,
                };


                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (advertisingUpdate.Photo != null)
                {
                    if (!advertisingUpdate.Photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "Please choose correct image type");
                        return View(model);
                    }

                    if (!advertisingUpdate.Photo.CheckFileSize(200))
                    {
                        ModelState.AddModelError("Photo", "Image size must be max 200kb");
                        return View(model);
                    }


                    string dbPath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", dbAdvertising.Image);

                    FileHelper.DeleteFile(dbPath);


                    string fileName = Guid.NewGuid().ToString() + "_" + advertisingUpdate.Photo.FileName;

                    string newPath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", fileName);

                    await FileHelper.SaveFileAsync(newPath, advertisingUpdate.Photo);

                    dbAdvertising.Image = fileName;
                }
                else
                {
                    Advertising advertising = new()
                    {
                        Image = dbAdvertising.Image
                    };
                }


                dbAdvertising.Name = advertisingUpdate.Name;
                dbAdvertising.Description = advertisingUpdate.Description;

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
