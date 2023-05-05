using Backend_Project.Models;
using Backend_Project.Services.Interfaces;
using Backend_Project.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Backend_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISliderService _sliderService;
        public HomeController(ISliderService sliderService)
        {
            _sliderService = sliderService;
        }
        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _sliderService.GetAll();

            HomeVM model = new()
            {
                Sliders = sliders
            };
            return View(model);
        }

        

        
    }
}