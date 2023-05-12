using Backend_Project.Models;

namespace Backend_Project.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetCategories();
        Task<Category> GetById(int? id);
    }
}
