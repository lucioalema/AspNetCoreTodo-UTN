using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreTodo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreTodo.Data
{
    public static class ApplicationDbInitializer
    {
        public static void Initialize(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            if (!context.Roles.Any())
                SeedRole(roleManager);
            if (!context.Users.Any())
                SeedUsers(userManager);
            if (!context.Categories.Any())
                SeedCategories(context);
            if (!context.Items.Any())
                SeedTodoItems(context, userManager);
        }

        private static void SeedRole(RoleManager<IdentityRole> roleManager)
        {
            if (roleManager.Roles
                .Where(x => x.Name == Constants.AdministratorRole)
                .SingleOrDefaultAsync().Result != null)
                return;

            var testAdmin = new IdentityRole
            {
                Name = Constants.AdministratorRole,
                NormalizedName = Constants.AdministratorRole.ToUpper()
            };

            IdentityResult result = roleManager.CreateAsync(testAdmin).Result;
        }

        private static void SeedUsers(UserManager<IdentityUser> userManager)
        {
            if (userManager.Users
                .Where(x => x.UserName == "admin@todo.local")
                .SingleOrDefaultAsync().Result != null)
                return;

            var testAdmin = new IdentityUser
            {
                UserName = "admin@todo.local",
                Email = "admin@todo.local"
            };

            IdentityResult result = userManager.CreateAsync(testAdmin, "NotSecure123!!").Result;
            
            if (result.Succeeded)
            {
                userManager.AddToRoleAsync(testAdmin, Constants.AdministratorRole).Wait();
            }
        }

        private static void SeedCategories(ApplicationDbContext context)
        {
            context.AddRange(new List<Category>
            {
                new Category{
                    Id = Guid.NewGuid(),
                    Name = "Standard"
                },
                new Category{
                    Id = Guid.NewGuid(),
                    Name = "Special"
                }
            });
            context.SaveChanges();
        }

        private static void SeedTodoItems(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            var user = userManager.Users
                .Where(x => x.UserName == "admin@todo.local")
                .SingleOrDefaultAsync().Result;
            context.AddRange(new List<TodoItem>
            {
                new TodoItem {
                    Id = Guid.NewGuid(),
                    Title = "Curso ASP.NET Core",
                    DueAt = DateTimeOffset.Now.AddDays(1),
                    Category = context.Categories.FirstOrDefault(x => x.Name == "Standard"),
                    UserId = user.Id
                },
                new TodoItem {
                    Id = Guid.NewGuid(),
                    Title = "Curso React",
                    DueAt = DateTimeOffset.Now.AddDays(1),
                    Category = context.Categories.FirstOrDefault(x => x.Name == "Special"),
                    UserId = user.Id
                }
            });
            context.SaveChanges();
        }
    }
}