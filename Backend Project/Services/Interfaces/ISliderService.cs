using Backend_Project.Data;
using Backend_Project.Models;

namespace Backend_Project.Services.Interfaces
{
    public interface ISliderService
    {
        Task<List<Slider>> GetAll();
    }
}
