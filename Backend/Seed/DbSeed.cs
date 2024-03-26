using Microsoft.AspNetCore.Identity;

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
            var managerRole = new IdentityRole
            {
                Name = "Manager",
            };
            var employeeRole = new IdentityRole
            {
                Name = "Employee",
            };
            await roleManager.CreateAsync(adminRole);
            await roleManager.CreateAsync(managerRole);
            await roleManager.CreateAsync(employeeRole);
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
