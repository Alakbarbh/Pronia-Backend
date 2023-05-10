using Backend_Project.Data;
using Backend_Project.Models;
using Backend_Project.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend_Project.Services
{
    public class SliderServive : ISliderService
    {
        private readonly AppDbContext _context;
        public SliderServive(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Slider>> GetAll()
        {
            return await _context.Sliders.Where(m=>!m.SoftDelete).ToListAsync();
        }


        public async Task<Slider> GetById(int? id)
        {
            return await _context.Sliders.Where(m => !m.SoftDelete).FirstOrDefaultAsync(m=>m.Id == id);
        }


    }
}
