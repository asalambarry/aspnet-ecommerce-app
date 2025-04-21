using Microsoft.AspNetCore.Identity;
using ShopZone.Models;

namespace ShopZone.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                // Création des rôles
                string[] roleNames = { "Admin", "Client" };
                foreach (var roleName in roleNames)
                {
                    var roleExist = await roleManager.RoleExistsAsync(roleName);
                    if (!roleExist)
                    {
                        await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }

                // Création de l'admin
                var adminUser = new IdentityUser
                {
                    UserName = "admin@shopzone.com",
                    Email = "admin@shopzone.com",
                    EmailConfirmed = true
                };

                if (await userManager.FindByEmailAsync(adminUser.Email) == null)
                {
                    await userManager.CreateAsync(adminUser, "Admin@123");
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }

        public static async Task SeedCategoriesAsync(ApplicationDbContext context)
        {
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category {
                        Name = "Electronics",
                        Description = "Electronic devices and gadgets"
                    },
                    new Category {
                        Name = "Clothing",
                        Description = "Fashion and apparel"
                    },
                    new Category {
                        Name = "Books",
                        Description = "Books and publications"
                    }
                };

                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();
            }
        }
    }
}