using KosDrive.Models;
using Microsoft.AspNetCore.Identity;

namespace KosDrive.Data
{
    public class ApplicationDbInitializer
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            string[] roleNames = { "Admin", "Rider", "Driver", "CompanyAdmin" };

            foreach(var roleName in roleNames)
            {
                if(!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                }
            }

            var adminEmail = "admin@kosdrive.com";
            var adminUser = await userManager.FindByNameAsync(adminEmail);

            if(adminUser != null)
            {
                var user = new User
                {
                    UserName = adminUser.UserName,
                    Email = adminUser.Email,
                    EmailConfirmed = true,
                    FirstName = "System",
                    LastName = "Admin",
                    UserType = UserType.Admin
                };

                var result = await userManager.CreateAsync(user, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}
