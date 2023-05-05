using Backend_Project.Models;

namespace Backend_Project.ViewModels
{
    public class HomeVM
    {
        public List<Slider> Sliders { get; set; }
        public List<Advertising> Advertisings { get; set; }
        public Dictionary<string,string> HeaderBackgrounds { get; set; }
        public List<Product> Products { get; set; }
    }
}
