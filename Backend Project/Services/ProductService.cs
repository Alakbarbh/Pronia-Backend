﻿using Backend_Project.Data;
using Backend_Project.Models;
using Backend_Project.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend_Project.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        public ProductService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Product>> GetAll() => await _context.Products.Include(m => m.Images).Where(m => !m.SoftDelete).ToListAsync();
        public async Task<Product> GetById(int id) => await _context.Products.FindAsync(id);
        public async Task<int> GetCountAsync() => await _context.Products.CountAsync();


        //public async Task<Product> GetFullDataById(int id) => await _context.Products.Include(m => m.Images).Include(m => m.Category).FirstOrDefaultAsync(m => m.Id == id);
        //public async Task<List<Product>> GetPaginateDatas(int page, int take) => await _context.Products.Include(m => m.Category).Include(m => m.Images).Skip((page * take) - take).Take(take).ToListAsync();
    }
}
