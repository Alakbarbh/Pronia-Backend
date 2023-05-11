using Backend_Project.Data;
using Backend_Project.Models;
using Backend_Project.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend_Project.Services
{
    public class BrandService : IBrandService
    {
        private readonly AppDbContext _context;
        public BrandService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Brand>> GetBrands()
        {
            return await _context.Brands.Where(m => !m.SoftDelete).ToListAsync();
        }

        public async Task<Brand> GetById(int? id)
        {
            return await _context.Brands.Where(m => !m.SoftDelete).FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
