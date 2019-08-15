using System;
using System.Threading.Tasks;
using AspNetCoreTodo.Data;
using AspNetCoreTodo.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreTodo.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Category[]> GetAsync()
        {
            return await _context.Categories.ToArrayAsync();
        }

        public async Task<Category> GetByIdAsync(Guid Id)
        {
            return await _context.Categories.FirstOrDefaultAsync(x => x.Id == Id);
        }
    }
}