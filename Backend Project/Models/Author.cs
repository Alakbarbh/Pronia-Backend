namespace Backend_Project.Models
{
    public class Author : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<BLog> BLogs { get; set; }
    }
}
