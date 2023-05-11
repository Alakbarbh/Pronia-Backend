using Backend_Project.Models;
using System.ComponentModel.DataAnnotations;

namespace Backend_Project.Areas.Admin.ViewModels
{
    public class BlogUpdateVM
    {
        
        public int Id { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int AuthorId { get; set; }
        public List<BlogImage> Images { get; set; }
        public List<IFormFile> Photo { get; set; }
        
    }
}
