using Backend_Project.Models;

namespace Backend_Project.ViewModels
{
    public class BlogDetailVM
    {
        public BLog BlogDt { get; set; }
        public Dictionary<string, string> HeaderBackgrounds { get; set; }
    }
}
