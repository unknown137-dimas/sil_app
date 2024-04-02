using Microsoft.AspNetCore.Identity;

namespace Backend.Seed;

public static class DbSeed
{
    public static async Task Seed(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        if(!roleManager.Roles.Any())
        {
            var adminRole = new IdentityRole
            {
                Name = "Administrator",
            };
            await roleManager.CreateAsync(adminRole);
        }

        if(!userManager.Users.Any())
        {
            var defAdmin = new User
            {
                UserName = "admin",
                Email = "admin@fake-email.com",
                EmailConfirmed = true,
            };
            await userManager.CreateAsync(defAdmin, "Adm!nP4$$");
            await userManager.AddToRolesAsync(defAdmin, ["Administrator"]);
        }
    }
}
