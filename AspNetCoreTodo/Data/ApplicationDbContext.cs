using System;
using System.Collections.Generic;
using System.Text;
using AspNetCoreTodo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreTodo.Data
{
    public class ApplicationDbContext: IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<TodoItem> Items { get; set; }
        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Category>().HasData(
                new Category{
                    Id = Guid.NewGuid(),
                    Name = "Standard"
                },
                new Category{
                    Id = Guid.NewGuid(),
                    Name = "Special"
                }
            );
            
            // builder.Entity<TodoItem>().HasData(
            //     new TodoItem {
            //         Id = Guid.NewGuid(),
            //         Title = "Curso ASP.NET Core",
            //         DueAt = DateTimeOffset.Now.AddDays(1)},
            //     new TodoItem {
            //         Id = Guid.NewGuid(),
            //         Title = "Curso React",
            //         DueAt = DateTimeOffset.Now.AddDays(1)}
            //         );
        }
    }
}
