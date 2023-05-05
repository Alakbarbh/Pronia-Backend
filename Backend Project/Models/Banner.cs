using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Backend_Project.Models
{
    public class Banner : BaseEntity
    {
        public string Image { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public bool Islarge { get; set; } = false;
    }
}
