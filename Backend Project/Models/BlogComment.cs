using System.Reflection.Metadata;

namespace Backend_Project.Models
{
    public class BlogComment : BaseEntity
    {
        public int BlogId { get; set; }
        public BLog Blog { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? Subject { get; set; }
        public string Message { get; set; }
    }
}
