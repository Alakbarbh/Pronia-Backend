using Backend_Project.Models;
using System.Reflection.Metadata;

namespace Backend_Project.ViewModels
{
    public class BlogDetailVM
    {
        public BLog BlogDt { get; set; }
        public List<BLog> Blogs { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Product> NewProducts { get; set; }
        public Dictionary<string, string> HeaderBackgrounds { get; set; }
        public List<Category> Categories { get; set; }
        public CommentVM CommentVM { get; set; }
        public List<BlogComment> BlogComments { get; set; }
    }
}
