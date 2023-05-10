using Backend_Project.Models;

namespace Backend_Project.Services.Interfaces
{
    public interface IBannerService
    {
        Task<Banner> GetById(int? id);
    }
}
