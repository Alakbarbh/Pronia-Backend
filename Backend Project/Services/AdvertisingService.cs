using Backend_Project.Data;
using Backend_Project.Models;
using Backend_Project.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend_Project.Services
{
    public class AdvertisingService : IAdvertisingService
    {
        private readonly AppDbContext _context;
        public AdvertisingService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Advertising>> GetAll()
        {
            return await _context.Advertisings.Where(m => !m.SoftDelete).ToListAsync();
        }

        public async Task<Advertising> GetById(int? id)
        {
            return await _context.Advertisings.Where(m => !m.SoftDelete).FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
