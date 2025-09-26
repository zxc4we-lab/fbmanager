using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FacebookManager.Data;
using FacebookManager.Models;

namespace FacebookManager;

// The Program class configures and runs the ASP.NET Core application.  In
// .NET 6/7 the Startup class has been merged into Program.cs for a simpler
// single file configuration.
public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Load configuration from appsettings.json
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        // Register the EF Core DbContext with a SQLite provider.  SQLite is
        // chosen here because it requires no external database server and
        // simplifies setup in this environment.
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(connectionString));

        // Register ASP.NET Core Identity using the custom ApplicationUser
        // class.  Identity manages user authentication, password hashing and
        // cookie issuance for us.
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        // Configure cookie authentication settings.  This controls how
        // long sessions last and the name of the authentication cookie.
        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Account/Login";
            options.LogoutPath = "/Account/Logout";
            options.AccessDeniedPath = "/Account/AccessDenied";
            options.ExpireTimeSpan = TimeSpan.FromHours(1);
            options.SlidingExpiration = true;
        });

        // Add controllers and views with Razor runtime compilation to
        // simplify development.  In production you would typically
        // precompile views for performance.
        builder.Services.AddControllersWithViews();

        // Register API controllers for JSON endpoints.
        builder.Services.AddControllers();

        var app = builder.Build();

        // Apply pending EF Core migrations and create the database.  This
        // ensures the database schema is created on first run.  In
        // production you would handle migrations separately.
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.EnsureCreated();
        }

        // Configure the HTTP request pipeline.  Use developer friendly
        // exception pages and enable static file serving and routing.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
        }
        else
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        // Define default routing patterns for MVC controllers and API
        // controllers.  The default route directs to Home/Index when no
        // controller or action is specified.
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapControllers();

        app.Run();
    }
}