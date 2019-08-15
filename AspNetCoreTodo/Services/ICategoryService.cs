using System;
using System.Threading.Tasks;
using AspNetCoreTodo.Models;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreTodo.Services
{
    public interface ICategoryService
    {
        Task<Category[]> GetAsync();
        Task<Category> GetByIdAsync(Guid Id);
    }
}