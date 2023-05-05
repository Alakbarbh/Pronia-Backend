namespace Backend_Project.Models
{
    public class BlogImage : BaseEntity
    {
        public string Image { get; set; }
        public int BlogId { get; set; }
        public BLog BLog { get; set; }
    }
}
