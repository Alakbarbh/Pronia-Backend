namespace Backend_Project.Models
{
    public class ProductCategory : BaseEntity
    {
        public int ProductId { get; set; }
        public Product Pro { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
