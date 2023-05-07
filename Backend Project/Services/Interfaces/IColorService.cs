using Backend_Project.Models;

namespace Backend_Project.Services.Interfaces
{
    public interface IColorService
    {
        Task<List<Color>> GetAllColors();
    }
}
