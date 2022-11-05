using api.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace api.Data
{
    public class AuthDbSeeder
    {
        private readonly UserManager<RegisteredUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AuthDbSeeder(UserManager<RegisteredUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            await AddDefaultRoles();
            await AddAdminUser();
        }

        private async Task AddDefaultRoles()
        {
            foreach (var role in Roles.All)
            {
                var roleExists = await roleManager.RoleExistsAsync(role);
                if (!roleExists)
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        private async Task AddAdminUser()
        {
            var newAdminUser = new RegisteredUser()
            {
                Email = "admin@admin.com",
                FirstName = "Admin",
                LastName = "Admin",
                PhoneNumber = "",
                IsApproved = true,
                UserName = "admin@admin.com",
                Address = "",
                City = "",
                ZipCode = "",
                HasFinishedRegistration = true
            };

            var existingAdminUser = await userManager.FindByEmailAsync(newAdminUser.Email);
            if (existingAdminUser == null)
            {
                var createAdminUser = await userManager.CreateAsync(newAdminUser, "Taip123.");
                if (createAdminUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdminUser, Roles.Admin);
                }
            }
        }
    }
}
