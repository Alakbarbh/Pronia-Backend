using Backend_Project.Data;
using Backend_Project.Models;
using Backend_Project.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend_Project.Services
{
    public class BannerService : IBannerService
    {
        private readonly AppDbContext _context;
        public BannerService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Banner> GetById(int? id)
        {
            return await _context.Banners.Where(m => !m.SoftDelete).FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
