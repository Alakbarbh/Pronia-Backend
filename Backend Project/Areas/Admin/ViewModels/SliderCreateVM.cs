using System.ComponentModel.DataAnnotations;

namespace Backend_Project.Areas.Admin.ViewModels
{
    public class SliderCreateVM
    {
        [Required(ErrorMessage = "Don't be empty")]
        public IFormFile Photos { get; set; }
        [Required(ErrorMessage = "Don't be empty")]
        public string Offer { get; set; }
        [Required(ErrorMessage = "Don't be empty")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Don't be empty")]
        public string Description { get; set; }
    }
}
