using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TourWebsite.Areas.Identity.Data;
using TourWebsite.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("TourWebsiteContextConnection") ?? throw new InvalidOperationException("Connection string 'TourWebsiteContextConnection' not found.");;

builder.Services.AddDbContext<TourWebsiteContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<TourWebsiteUser>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<TourWebsiteRole>().AddEntityFrameworkStores<TourWebsiteContext>();


builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<TourWebsiteRole>>();

    var roles = new[] { "Admin", "Editor", "User" };

    foreach (var role in roles)
    {
        if(!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new TourWebsiteRole(role));
    }
}


using (var scope = app.Services.CreateScope()) //TODO DELETE THIS
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<TourWebsiteUser>>();

    string email = "admin@admin.com";
    string pwd = "Admin1234!";


    if (await userManager.FindByEmailAsync(email) == null) {
        var user = new TourWebsiteUser();
        user.UserName = email;
        user.Email = email;
        user.EmailConfirmed = true;

        await userManager.CreateAsync(user, pwd);

        await userManager.AddToRoleAsync(user, "Admin");
    }

}
app.MapRazorPages();

app.Run();
