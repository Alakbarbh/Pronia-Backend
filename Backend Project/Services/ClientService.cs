using Backend_Project.Data;
using Backend_Project.Models;
using Backend_Project.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend_Project.Services
{
    public class ClientService : IClientService
    {
        private readonly AppDbContext _context;
        public ClientService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Client>> GetClients()
        {
            return await _context.Clients.Where(m => !m.SoftDelete).ToListAsync();
        }

        public async Task<Client> GetById(int? id)
        {
            return await _context.Clients.Where(m => !m.SoftDelete).FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
