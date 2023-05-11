﻿using Backend_Project.Areas.Admin.ViewModels;
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
    public class BrandController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private IBrandService _brandService;
        public BrandController(AppDbContext context,
                                IWebHostEnvironment env,
                                IBrandService brandService)

        {
            _context = context;
            _env = env;
            _brandService = brandService;
        }
        public async Task<IActionResult> Index()
        {
            List<Brand> brands = await _context.Brands.ToListAsync();
            return View(brands);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            Brand brand = await _brandService.GetById(id);
            if (brand is null) return NotFound();
            return View(brand);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrandCreateVM brand)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }



                if (!brand.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File type must be image");
                    return View();
                }




                if (!brand.Photo.CheckFileSize(200))
                {
                    ModelState.AddModelError("Photo", "Image size must be max 200kb");
                    return View();
                }



                string fileName = Guid.NewGuid().ToString() + "_" + brand.Photo.FileName;


                string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", fileName);

                await FileHelper.SaveFileAsync(path, brand.Photo);

                Brand newBrand = new()
                {
                    Image = fileName,
                };


                await _context.Brands.AddAsync(newBrand);



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

                Brand dbBrand = await _brandService.GetById(id);

                if (dbBrand is null) return NotFound();

                string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", dbBrand.Image);

                FileHelper.DeleteFile(path);

                _context.Brands.Remove(dbBrand);

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
            Brand dbBrand = await _brandService.GetById(id);
            if (dbBrand is null) return NotFound();

            BrandUpdateVM model = new()
            {
                Image = dbBrand.Image,
            };

            return View(model);

        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, BrandUpdateVM brandUpdate)
        {
            try
            {

                if (id == null) return BadRequest();

                Brand dbBrand = await _brandService.GetById(id);

                if (dbBrand is null) return NotFound();

                BrandUpdateVM model = new()
                {
                    Image = dbBrand.Image,
                };


                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (brandUpdate.Photo != null)
                {
                    if (!brandUpdate.Photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "Please choose correct image type");
                        return View(model);
                    }

                    if (!brandUpdate.Photo.CheckFileSize(200))
                    {
                        ModelState.AddModelError("Photo", "Image size must be max 200kb");
                        return View(model);
                    }


                    string dbPath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", dbBrand.Image);

                    FileHelper.DeleteFile(dbPath);


                    string fileName = Guid.NewGuid().ToString() + "_" + brandUpdate.Photo.FileName;

                    string newPath = FileHelper.GetFilePath(_env.WebRootPath, "assets/images/website-images", fileName);

                    await FileHelper.SaveFileAsync(newPath, brandUpdate.Photo);

                    dbBrand.Image = fileName;
                }
                else
                {
                    Brand brand = new()
                    {
                        Image = dbBrand.Image
                    };
                }


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
