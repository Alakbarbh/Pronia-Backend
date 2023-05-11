using Backend_Project.Data;
using Backend_Project.Models;
using Backend_Project.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend_Project.Services
{
    public class TagService : ITagService
    {
        private readonly AppDbContext _context;
        public TagService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Tag>> GetAllAsync()
        {
            return await _context.Tags.Include(m=>m.ProductTags).Where(m=>!m.SoftDelete).ToListAsync();
        }

        public async Task<Tag> GetById(int? id)
        {
            return await _context.Tags.Where(m => !m.SoftDelete).FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
