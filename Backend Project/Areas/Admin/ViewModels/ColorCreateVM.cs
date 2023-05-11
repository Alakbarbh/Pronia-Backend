using System.ComponentModel.DataAnnotations;

namespace Backend_Project.Areas.Admin.ViewModels
{
    public class ColorCreateVM
    {
        [Required(ErrorMessage = "Don't be empty")]
        public string Name { get; set; }
        
    }
}
