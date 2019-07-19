using System;
using System.Collections.Generic;
using System.Text;
using AspNetCoreTodo.Models;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreTodo.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<TodoItem> Items { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TodoItem>().HasData(
                new TodoItem {
                    Id = Guid.NewGuid(),
                    Title = "Curso ASP.NET Core",
                    DueAt = DateTimeOffset.Now.AddDays(1)},
                new TodoItem {
                    Id = Guid.NewGuid(),
                    Title = "Curso React",
                    DueAt = DateTimeOffset.Now.AddDays(1)}
                    );

            base.OnModelCreating(builder);
            // ...
        }
    }
}
