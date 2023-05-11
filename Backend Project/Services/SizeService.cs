using Backend_Project.Data;
using Backend_Project.Models;
using Backend_Project.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend_Project.Services
{
    public class SizeService : ISizeService
    {
        private readonly AppDbContext _context;
        public SizeService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Size>> GetAllSize()
        {
            return await _context.Sizes.Include(m => m.ProductSizes).Where(m => !m.SoftDelete).ToListAsync();
        }

        public async Task<Size> GetById(int? id)
        {
            return await _context.Sizes.Where(m => !m.SoftDelete).FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
