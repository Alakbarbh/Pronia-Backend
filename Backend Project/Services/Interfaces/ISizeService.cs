using Backend_Project.Models;

namespace Backend_Project.Services.Interfaces
{
    public interface ISizeService
    {
        Task<List<Size>> GetAllSize();
        Task<Size> GetById(int? id);
    }
}
