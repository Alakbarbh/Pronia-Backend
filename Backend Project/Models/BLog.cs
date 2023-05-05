﻿namespace Backend_Project.Models
{
    public class BLog : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public ICollection<BlogImage> Images { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}