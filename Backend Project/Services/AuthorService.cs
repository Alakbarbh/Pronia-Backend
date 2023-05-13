using Backend_Project.Data;
using Backend_Project.Models;
using Backend_Project.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend_Project.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly AppDbContext _context;
        public AuthorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Author>> GetAllAuthor()
        {
            return await _context.Authors.Include(m => m.BLogs).Where(m => !m.SoftDelete).ToListAsync();
        }

        public async Task<Author> GetById(int? id)
        {
            return await _context.Authors.Where(m => !m.SoftDelete).FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
