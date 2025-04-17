using DVLD.Core.Constants;
using DVLD.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace DVLD.Core.Helpers
{
    public static class Initializer
    {
        public static async Task SeedDataAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();

            // Ensure Role exists
            string adminRole = Roles.AdminRole;
            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new AppRole
                {
                    Name = adminRole
                });
            }

            // Create admin user if not exists
            string adminEmail = "testemail@gmail.com";
            string adminPassword = "Testato1234+-";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new AppUser
                {
                    UserName = "IbrahimHany",
                    Email = adminEmail,
                    EmailConfirmed = true,
                    PhoneNumber="01011283465",
                    IsActive=true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, adminRole);
                }
                else
                {
                    // Optional: Log errors
                    foreach (var error in result.Errors)
                        Console.WriteLine($"Error creating admin user: {error.Description}");
                }
            }
        }

    }
}
