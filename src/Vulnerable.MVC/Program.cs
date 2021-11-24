using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Vulnerable.MVC;
using Vulnerable.MVC.Areas.Identity;
using Vulnerable.MVC.Areas.Identity.Data;
using Vulnerable.MVC.Data;


var builder = WebApplication.CreateBuilder(args);

//  Register our weak password hasher so that all our password hashes are MD5
builder.Services.AddScoped<IPasswordHasher<User>, WeakPasswordHasher<User>>();

var connectionString = builder.Configuration.GetConnectionString("IdentityContextConnection");

builder.Services.AddDbContext<IdentityContext>(options
        => options.UseSqlite(connectionString)
    );

builder.Services.AddDefaultIdentity<User>(options => 
        {
            //  Don't require account confirmation of any kind
            options.SignIn.RequireConfirmedAccount = false;
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;

            //  Weak password policy
            options.Password.RequiredLength = 1;
            options.Password.RequiredUniqueChars = 1;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;

            //  Lockout should not enabled (see PasswordSignInAsync method)
            //  BUT this is a week lockout policy in case users do become locked out            
            options.Lockout.AllowedForNewUsers = false;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(1);
        })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<IdentityContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    //app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "search",
    pattern: "Search/{q}",
    new { controller = "Search", action = "Index" }
    );

app.MapControllerRoute(
    name: "admin",
    pattern: "Admin/{action=Index}",
    new { controller = "UserAdmin", action = "Index" }
    );

app.MapRazorPages();

app.EnsureDatabaseCreatedAndSeeded();

app.Run();