using Backend_Project.Models;

namespace Backend_Project.Services.Interfaces
{
    public interface IAuthorService
    {
        Task<Author> GetAllAsync(int? id);
    }
}
