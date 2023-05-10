using Backend_Project.Models;
using EntityFramework_Slider.Helpers;

namespace Backend_Project.ViewModels
{
    public class BlogVM
    {
        public Dictionary<string, string> HeaderBackgrounds { get; set; }
        public List<BLog> BLogs { get; set; }
        public Paginate<BLog> PaginateBlog { get; set; }
        public List<Category> Categories { get; set; }
        public List<Product> Product { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Product> NewProduct { get; set; }
    }
}
