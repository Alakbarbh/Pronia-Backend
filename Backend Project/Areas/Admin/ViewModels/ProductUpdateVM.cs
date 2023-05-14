using Backend_Project.Models;
using System.ComponentModel.DataAnnotations;

namespace Backend_Project.Areas.Admin.ViewModels
{
    public class ProductUpdateVM
    {
        public int Id { get; set; }
        public List<IFormFile> Photos { get; set; }
        public List<ProductImage> Images { get; set; }
        [Required(ErrorMessage = "Don`t be empty")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Don`t be empty")]
        public string Description { get; set; }
        public string MainImage { get; set; }
        public IFormFile MainPhoto { get; set; }
        public string HoverImage { get; set; }
        public IFormFile HoverPhoto { get; set; }
        [Required(ErrorMessage = "Don`t be empty")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Don`t be empty")]
        public int SaleCount { get; set; }
        [Required(ErrorMessage = "Don`t be empty")]
        public int StockCount { get; set; }
        [Required(ErrorMessage = "Don`t be empty")]
        public string SKU { get; set; }
        [Required(ErrorMessage = "Don`t be empty")]
        public int ColorId { get; set; }
        [Required(ErrorMessage = "Don`t be empty")]
        public List<int> SizeIds { get; set; }

        [Required(ErrorMessage = "Don`t be empty")]
        public List<int> TagIds { get; set; }

        [Required(ErrorMessage = "Don`t be empty")]
        public List<int> CategoryIds { get; set; }
    }
}
