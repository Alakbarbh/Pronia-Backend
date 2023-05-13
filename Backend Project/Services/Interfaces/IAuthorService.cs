using Backend_Project.Models;

namespace Backend_Project.Services.Interfaces
{
    public interface IAuthorService
    {
        Task<List<Author>> GetAllAuthor();
        Task<Author> GetById(int? id);
    }
}
