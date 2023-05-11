using Backend_Project.Models;
using System.ComponentModel.DataAnnotations;

namespace Backend_Project.Areas.Admin.ViewModels
{
    public class BLogCreateVM
    {
        [Required(ErrorMessage = "Don't be empty")]
        public List<IFormFile> Photos { get; set; }
        [Required(ErrorMessage = "Don't be empty")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Don't be empty")]
        public string Description { get; set; }
        public int AuthorId { get; set; }
        
        //public ICollection<Comment> Comments { get; set; }
    }
}
