using Backend_Project.Data;
using Backend_Project.Models;
using Backend_Project.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend_Project.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;
        public CategoryService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Category>> GetCategories()
        {
            return await _context.Categories.Include(m => m.ProductCategories).Where(m => !m.SoftDelete).ToListAsync();
        }

        public async Task<Category> GetById(int? id)
        {
            return await _context.Categories.Where(m => !m.SoftDelete).FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
