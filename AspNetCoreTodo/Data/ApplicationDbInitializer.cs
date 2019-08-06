using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreTodo.Data
{
    public static class ApplicationDbInitializer
    {
        public static void SeedRole(RoleManager<IdentityRole> roleManager)
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

        public static void SeedUsers(UserManager<IdentityUser> userManager)
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
    }
}