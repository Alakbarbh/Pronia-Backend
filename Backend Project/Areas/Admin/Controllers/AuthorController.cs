using Backend_Project.Areas.Admin.ViewModels;
using Backend_Project.Data;
using Backend_Project.Models;
using Backend_Project.Services;
using Backend_Project.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthorController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IAuthorService _authorService;
        public AuthorController(AppDbContext context,
                                IWebHostEnvironment env,
                                IAuthorService authorService)
        {
            _context = context;
            _env = env;
            _authorService = authorService;
        }
        public async Task<IActionResult> Index()
        {
            List<Author> authors = await _authorService.GetAllAuthor();
            return View(authors);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            Author author = await _authorService.GetById(id);
            if (author is null) return NotFound();
            return View(author);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AuthorCreateVM author)
        {
            try
            {

                Author newAuthor = new()
                {
                    Name = author.Name,
                };


                await _context.Authors.AddAsync(newAuthor);



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

                Author dbAuthor = await _authorService.GetById(id);

                if (dbAuthor is null) return NotFound();

                _context.Authors.Remove(dbAuthor);

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
            Author dbAuthor = await _authorService.GetById(id);
            if (dbAuthor is null) return NotFound();

            AuthorUpdateVM model = new()
            {
                Name = dbAuthor.Name,
            };

            return View(model);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, AuthorUpdateVM authorUpdate)
        {
            try
            {

                if (id == null) return BadRequest();

                Author dbAuthor = await _authorService.GetById(id);

                if (dbAuthor is null) return NotFound();

                AuthorUpdateVM model = new()
                {
                    Name = dbAuthor.Name
                };


                if (!ModelState.IsValid)
                {
                    return View(model);
                }



                dbAuthor.Name = authorUpdate.Name;

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
