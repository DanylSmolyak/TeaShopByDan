using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data;

public static class DataSeeder 
{
    public static async Task SeedEssentialsAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {

        await roleManager.CreateAsync(new IdentityRole(Role.Roles.Administrator.ToString()));
        await roleManager.CreateAsync(new IdentityRole(Role.Roles.Moderator.ToString()));
        await roleManager.CreateAsync(new IdentityRole(Role.Roles.UserServices.ToString()));
        
        var defaultUser = new User
        {
            UserName = Authorization.default_username,
            Email = Authorization.default_email,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };

        if (userManager.Users.All(u => u.Id != defaultUser.Id))
        {
            await userManager.CreateAsync(defaultUser, Authorization.default_password);
            await userManager.AddToRoleAsync(defaultUser, Authorization.default_role.ToString());
        }
    }
}