using Backend_Project.Data;
using Backend_Project.Models;
using Backend_Project.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend_Project.Services
{
    public class ColorService : IColorService
    {
        private readonly AppDbContext _context;
        public ColorService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Color>> GetAllColors()
        {
            return await _context.Colors.Include(m => m.Products).Where(m => !m.SoftDelete).ToListAsync();
        }

        public async Task<Color> GetById(int? id)
        {
            return await _context.Colors.Where(m => !m.SoftDelete).FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
