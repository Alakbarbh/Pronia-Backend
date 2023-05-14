using Backend_Project.Data;
using Backend_Project.Models;
using Backend_Project.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend_Project.Services
{
    public class SocialService : ISocialService
    {
        private readonly AppDbContext _context;
        public SocialService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Social>> GetSocials()
        {
            return await _context.Socials.Where(m => !m.SoftDelete).ToListAsync();
        }

        public async Task<Social> GetById(int? id)
        {
            return await _context.Socials.Where(m => !m.SoftDelete).FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
