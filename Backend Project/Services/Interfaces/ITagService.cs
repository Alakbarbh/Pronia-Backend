using Backend_Project.Models;

namespace Backend_Project.Services.Interfaces
{
    public interface ITagService
    {
        Task<List<Tag>> GetAllAsync();
        Task<Tag> GetById(int? id);
    }
}
