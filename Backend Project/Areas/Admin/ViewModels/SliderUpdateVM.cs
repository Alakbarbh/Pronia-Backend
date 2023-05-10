using Backend_Project.Models;
using System.ComponentModel.DataAnnotations;

namespace Backend_Project.Areas.Admin.ViewModels
{
    public class SliderUpdateVM
    {
        public IFormFile Photo { get; set; }
        public string Image { get; set; }
        public string Offer { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
