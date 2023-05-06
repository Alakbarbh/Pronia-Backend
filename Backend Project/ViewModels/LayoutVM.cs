using Backend_Project.Models;

namespace Backend_Project.ViewModels
{
    public class LayoutVM
    {
        public IEnumerable<Social> Socials { get; set; }
        public Dictionary<string, string> Settings { get; set; }
    }
}
