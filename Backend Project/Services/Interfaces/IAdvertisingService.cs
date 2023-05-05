using Backend_Project.Models;

namespace Backend_Project.Services.Interfaces
{
    public interface IAdvertisingService
    {
        Task<List<Advertising>> GetAll();
    }
}
