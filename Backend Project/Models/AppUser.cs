using Microsoft.AspNetCore.Identity;

namespace Backend_Project.Models
{
    public class AppUser : IdentityUser
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public bool IsRememberMe { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
