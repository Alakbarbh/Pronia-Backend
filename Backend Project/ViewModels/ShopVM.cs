using Backend_Project.Models;

namespace Backend_Project.ViewModels
{
    public class ShopVM
    {
        public Dictionary<string, string> HeaderBackgrounds { get; set; }
        public List<Category> Categories { get; set; }
        public List<Product> NewProduct { get; set; }
        public List<Color> Colors { get; set; }
    }
}
