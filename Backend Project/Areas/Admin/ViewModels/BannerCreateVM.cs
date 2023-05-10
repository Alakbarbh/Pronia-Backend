using System.ComponentModel.DataAnnotations;

namespace Backend_Project.Areas.Admin.ViewModels
{
    public class BannerCreateVM
    {
        [Required(ErrorMessage = "Don't be empty")]
        public IFormFile Photo { get; set; }

        [Required(ErrorMessage = "Don't be empty")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Don't be empty")]
        public string Title { get; set; }
    }
}
