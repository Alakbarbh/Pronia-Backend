using Backend_Project.Areas.Admin.ViewModels;
using Backend_Project.Data;
using Backend_Project.Helpers;
using Backend_Project.Models;
using Backend_Project.Services;
using Backend_Project.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ColorController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IColorService _colorService;
        public ColorController(AppDbContext context,
                                IWebHostEnvironment env,
                                IColorService colorService)
        {
            _context = context;
            _env = env;
            _colorService = colorService;
        }

        public async Task<IActionResult> Index()
        {
            List<Color> colors = await _colorService.GetAllColors();
            return View(colors);
        }


        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            Color color = await _colorService.GetById(id);
            if (color is null) return NotFound();
            return View(color);
        }



        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ColorCreateVM color)
        {
            try
            {

                Color newColor = new()
                {
                    Name = color.Name,
                };


                await _context.Colors.AddAsync(newColor);



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

                Color dbColor = await _colorService.GetById(id);

                if (dbColor is null) return NotFound();

                _context.Colors.Remove(dbColor);

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
            Color dbColor = await _colorService.GetById(id);
            if (dbColor is null) return NotFound();

            ColorUpdateVM model = new()
            {
                Name = dbColor.Name,
            };

            return View(model);

        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, ColorUpdateVM colorUpdate)
        {
            try
            {

                if (id == null) return BadRequest();

                Color dbColor = await _colorService.GetById(id);

                if (dbColor is null) return NotFound();

                ColorUpdateVM model = new()
                {
                    Name = dbColor.Name,
                };


                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                


                dbColor.Name = colorUpdate.Name;

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
