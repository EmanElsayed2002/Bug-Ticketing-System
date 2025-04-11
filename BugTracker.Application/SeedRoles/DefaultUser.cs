using BugTracker.Application.Const;
using BugTracker.Data.models.Identity;

using Microsoft.AspNetCore.Identity;

namespace School.Application.SeedRoles;

public class DefaultUser
{
    public async static Task SeedAdminUserAsync(UserManager<User> userManager)
    {
        var Admin = new User()
        {
            FullName = "Eman Elsayed",
            Email = "Admin@gmail.com",
            UserName = "Admin@gmail.com",
            EmailConfirmed = true
        };
        var user = await userManager.FindByEmailAsync(Admin.Email);
        if (user is null)
        {
            await userManager.CreateAsync(Admin, "P@ssword123");
            await userManager.AddToRoleAsync(Admin, AppRoles.Manager);
        }
    }
}
