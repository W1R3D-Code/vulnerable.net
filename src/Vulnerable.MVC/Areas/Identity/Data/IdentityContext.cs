using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vulnerable.MVC.Areas.Identity.Data;

namespace Vulnerable.MVC.Data;

public class IdentityContext : IdentityDbContext<User>
{
    private readonly IPasswordHasher<User> _passwordHasher;

    public IdentityContext(DbContextOptions<IdentityContext> options, IPasswordHasher<User> passwordHasher)
        : base(options)
    {
        _passwordHasher = passwordHasher;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}