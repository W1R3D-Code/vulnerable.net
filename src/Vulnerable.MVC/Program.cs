using Microsoft.EntityFrameworkCore;
using Vulnerable.MVC;
using Vulnerable.MVC.Areas.Identity.Data;
using Vulnerable.MVC.Data;


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("IdentityContextConnection");

builder.Services.AddDbContext<IdentityContext>(options
        => options.UseSqlite(connectionString)
    );

builder.Services.AddDefaultIdentity<User>(options 
        => options.SignIn.RequireConfirmedAccount = false)
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

app.MapRazorPages();

app.CreateAndMigrateDatabase();

app.Run();