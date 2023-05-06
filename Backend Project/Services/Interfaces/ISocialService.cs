using Backend_Project.Models;

namespace Backend_Project.Services.Interfaces
{
    public interface ISocialService
    {
        Task<List<Social>> GetSocials();
    }
}
