using Backend_Project.Data;
using Backend_Project.Models;
using Backend_Project.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend_Project.Services
{
    public class BlogService : IBlogService
    {
        private readonly AppDbContext _context;
        public BlogService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<BLog>> GetBlogs() => await _context.BLogs.
                                                 Where(m => !m.SoftDelete).
                                                 Include(m => m.Images).
                                                 Include(m => m.Author).
                                                 Include(m => m.BlogComments).
                                                 ToListAsync();

        public async Task<int> GetCountAsync() => await _context.Products.CountAsync();

        public async Task<List<BLog>> GetPaginateDatas(int page, int take) => await _context.BLogs.
            Where(m => !m.SoftDelete).
            Include(m => m.Images).
            Include(m => m.Author).
            Include(m => m.BlogComments).
            Skip((page * take) - take).
            Take(take).ToListAsync();


        public async Task<BLog> GetById(int? id)
        {
            return await _context.BLogs.Where(m => !m.SoftDelete).
                                        Include(m=>m.Images).
                                        Include(m=>m.Author).
                                        Include(m=>m.BlogComments).
                                        FirstOrDefaultAsync(m => m.Id == id);
        }


        public async Task<IEnumerable<Author>> GetAuthorsAsync() => await _context.Authors.ToListAsync();
    }
}
