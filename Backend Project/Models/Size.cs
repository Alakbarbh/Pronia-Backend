﻿namespace Backend_Project.Models
{
    public class Size : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<ProductSize> ProductSizes { get; set; }
    }
}
