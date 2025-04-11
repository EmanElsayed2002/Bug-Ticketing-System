


using BugTracker.Application.Const;
using BugTracker.Data.models.Identity;

using Microsoft.AspNetCore.Identity;

namespace School.Application.SeedRoles;

public class DefaultRoles
{
    public async static Task SeedRolesAsync(RoleManager<Role> roleManager)
    {
        if (!roleManager.Roles.Any())
        {
            await roleManager.CreateAsync(new Role { Name = AppRoles.Developer });
            await roleManager.CreateAsync(new Role { Name = AppRoles.Manager });
            await roleManager.CreateAsync(new Role { Name = AppRoles.Tester });

        }
    }
}
