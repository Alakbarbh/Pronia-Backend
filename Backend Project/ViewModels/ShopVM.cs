using Backend_Project.Models;
using Backend_Project.Helpers;

namespace Backend_Project.ViewModels
{
    public class ShopVM
    {
        public Dictionary<string, string> HeaderBackgrounds { get; set; }
        public List<Category> Categories { get; set; }
        public List<Product> NewProduct { get; set; }
        public List<Product> Products { get; set; }
        public List<Color> Colors { get; set; }
        public Paginate<Product> PaginateProduct { get; set; }
        public List<Tag> Tags { get; set; }
    }
}
