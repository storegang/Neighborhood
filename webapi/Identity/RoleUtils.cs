using Microsoft.AspNetCore.Identity;

namespace webapi.Identity;

public static class RoleUtils
{
    public static async void InitializeRolesAsync(IServiceProvider service)
    {
        using IServiceScope scope = service.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        foreach (IdentityRole role in UserRoles.IdentityRoleArray)
        {
            if (!await roleManager.RoleExistsAsync(role.Name))
            {
                await roleManager.CreateAsync(role);
            }
        }
    }
}
