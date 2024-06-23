using khumalocraft.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace khumalocraft.Data
{
    public static class SeedData
    {
        public static async Task Initialize(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await SeedAdminUser(userManager);
            await AssignAdminRole(userManager);
        }

        private static async Task SeedAdminUser(UserManager<IdentityUser> userManager)
        {
            var adminUser = await userManager.FindByEmailAsync("admin@example.com");
            if (adminUser == null)
            {
                var user = new IdentityUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                };
                await userManager.CreateAsync(user, "Admin123!"); // Replace "Admin123!" with a secure password
            }
        }

        private static async Task AssignAdminRole(UserManager<IdentityUser> userManager)
        {
            var adminUser = await userManager.FindByEmailAsync("admin@gmail.com");
            if (adminUser != null)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}