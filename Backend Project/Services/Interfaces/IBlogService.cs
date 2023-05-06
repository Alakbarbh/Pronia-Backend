using Backend_Project.Models;

namespace Backend_Project.Services.Interfaces
{
    public interface IBlogService
    {
        Task<List<BLog>> GetBlogs();
    }
}
