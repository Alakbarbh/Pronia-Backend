using Backend_Project.Areas.Admin.ViewModels;
using Backend_Project.Data;
using Backend_Project.Helpers;
using Backend_Project.Models;
using Backend_Project.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Composition;

namespace Backend_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly ISliderService _sliderService;
        public SliderController(AppDbContext context, 
                                IWebHostEnvironment env,
                                ISliderService sliderService)
        {
            _context = context;
            _env = env;
            _sliderService = sliderService;
        }


        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _context.Sliders.ToListAsync();
            return View(sliders);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            Slider slider = await _sliderService.GetById(id);
            if (slider is null) return NotFound();
            return View(slider);
        }



        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderCreateVM slider)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                
                
                    if (!slider.Photos.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "File type must be image");
                        return View();
                    }




                    if (!slider.Photos.CheckFileSize(200))
                    {
                        ModelState.AddModelError("Photo", "Image size must be max 200kb");
                        return View();
                    }



                string fileName = Guid.NewGuid().ToString() + "_" + slider.Photos.FileName;


                    string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", fileName);

                    await FileHelper.SaveFileAsync(path, slider.Photos);

                    Slider newSlider = new()
                    {
                        Image = fileName,
                        Title = slider.Title,
                        Offer = slider.Offer,
                        Description = slider.Description
                    };


                    await _context.Sliders.AddAsync(newSlider);
                




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

                Slider dbSlider = await _sliderService.GetById(id);

                if (dbSlider is null) return NotFound();

                string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", dbSlider.Image);

                FileHelper.DeleteFile(path);

                _context.Sliders.Remove(dbSlider);

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
            Slider dbSlider = await _sliderService.GetById(id);
            if (dbSlider is null) return NotFound();

            SliderUpdateVM model = new()
            {
                Image = dbSlider.Image,
                Title = dbSlider.Title,
                Description = dbSlider.Description,
                Offer = dbSlider.Offer
            };

            return View(model);

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, SliderUpdateVM sliderUpdate)
        {
            try
            {

                if (id == null) return BadRequest();

                Slider dbSlider = await _sliderService.GetById(id);

                if (dbSlider is null) return NotFound();

                SliderUpdateVM model = new()
                {
                    Image = dbSlider.Image,
                    Title = dbSlider.Title,
                    Description = dbSlider.Description,
                    Offer = dbSlider.Offer
                };


                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (sliderUpdate.Photo != null)
                {
                    if (!sliderUpdate.Photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "Please choose correct image type");
                        return View(model);
                    }

                    if (!sliderUpdate.Photo.CheckFileSize(200))
                    {
                        ModelState.AddModelError("Photo", "Image size must be max 200kb");
                        return View(model);
                    }


                    string dbPath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", dbSlider.Image);

                    FileHelper.DeleteFile(dbPath);


                    string fileName = Guid.NewGuid().ToString() + "_" + sliderUpdate.Photo.FileName;

                    string newPath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", fileName);

                    await FileHelper.SaveFileAsync(newPath, sliderUpdate.Photo);

                    dbSlider.Image = fileName;
                }
                else
                {
                    Slider slider = new()
                    {
                        Image = dbSlider.Image
                    };
                }


                dbSlider.Title = sliderUpdate.Title;
                dbSlider.Description = sliderUpdate.Description;
                dbSlider.Offer = sliderUpdate.Offer;

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
