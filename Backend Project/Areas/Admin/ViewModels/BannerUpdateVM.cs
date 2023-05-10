namespace Backend_Project.Areas.Admin.ViewModels
{
    public class BannerUpdateVM
    {
        public IFormFile Photo { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
    }
}
