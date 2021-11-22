using Microsoft.EntityFrameworkCore;
using Vulnerable.MVC.Data;

namespace Vulnerable.MVC;
public static class DbSelfMigrator
{
    public static IApplicationBuilder CreateAndMigrateDatabase(this IApplicationBuilder builder, bool throwOnError = false)
    {
        using var scope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<IdentityContext>();

        context.Database.EnsureCreated();

        try
        {
            context.Database.Migrate();
        }
        catch
        {
            if (throwOnError)
                throw;
        }

        return builder;
    }
}