﻿namespace Backend_Project.Areas.Admin.ViewModels
{
    public class ClientUpdateVM
    {
        public IFormFile Photo { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
