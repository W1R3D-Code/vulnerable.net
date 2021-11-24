using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Vulnerable.MVC.Areas.Identity.Data;
using Vulnerable.MVC.Data;

namespace Vulnerable.MVC;
public static class DbSelfMigrator
{
    public static IApplicationBuilder EnsureDatabaseCreatedAndSeeded(this IApplicationBuilder builder, bool throwOnError = false)
    {
        using var scope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

        var passwordHasher = scope.ServiceProvider.GetService<IPasswordHasher<User>>() ?? throw new InvalidOperationException($"{typeof(IPasswordHasher<User>)} can not be resolved");
        
        using var userManager = scope.ServiceProvider.GetService<UserManager<User>>() ?? throw new InvalidOperationException($"{typeof(UserManager<User>)} can not be resolved");
        using var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>() ?? throw new InvalidOperationException($"{typeof(RoleManager<IdentityRole>)} can not be resolved");
        using var context = scope.ServiceProvider.GetRequiredService<IdentityContext>();

        
        context.Database.EnsureCreated();

        var requiredRoles = new List<string> { "Admin", "User" };

        //  Created default roles
        foreach (var role in requiredRoles.Where(x => !roleManager.RoleExistsAsync(x).Result))
        {
            roleManager.CreateAsync(new IdentityRole(role));
        }

        var adminUser = userManager.FindByNameAsync("Admin").Result;

        if (adminUser != null)
        {
            //  Reset to default values
            adminUser.Email = "admin@vulnerable.site";
            adminUser.NormalizedEmail = "ADMIN@VULNERABLE.SITE";
            adminUser.UserName = "Admin";
            adminUser.NormalizedUserName = "ADMIN";
            adminUser.PhoneNumber = "+111111111111";
            adminUser.EmailConfirmed = true;
            adminUser.PhoneNumberConfirmed = true;
        }
        else
        {
            //  Create default admin user
            adminUser = new User
            {
                Id = Guid.NewGuid().ToString("D"),
                Email = "admin@vulnerable.site",
                NormalizedEmail = "ADMIN@VULNERABLE.SITE",
                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                PhoneNumber = "+111111111111",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            var createResult = userManager.CreateAsync(adminUser).Result;

            if (!createResult.Succeeded)
                throw new Exception(createResult.ToString());
        }

        //  Set/re-set password
        adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "correcthorsebatterystaple");
        _ = userManager.UpdateAsync(adminUser).Result;

        //  Remove from User role if erroniously assinged
        _ = userManager.RemoveFromRoleAsync(adminUser, "User").Result;

        //  Add to Admin role
        _ = userManager.AddToRoleAsync(adminUser, "Admin").Result;

        return builder;
    }
}