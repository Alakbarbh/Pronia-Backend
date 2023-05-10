using Backend_Project.Models;

namespace Backend_Project.Services.Interfaces
{
    public interface IClientService
    {
        Task<List<Client>> GetClients();
        Task<Client> GetById(int? id);
    }
}
