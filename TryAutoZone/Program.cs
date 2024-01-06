using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TryAutoZone.Data;

var builder = WebApplication.CreateBuilder(args);

// Culture set
var cultureInfo = new CultureInfo("pl-PL");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();



await Task.Run(async () =>
{
    using (var scope = app.Services.CreateScope())
    {
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var roleExists = await roleManager.RoleExistsAsync("Administrator");
        if (!roleExists)
        {
            await roleManager.CreateAsync(new IdentityRole("Administrator"));
        }
    }
});


// Demonstracyjne konto admina
await Task.Run(async () =>
{
    using (var scope = app.Services.CreateScope())
    {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        string email = "admin@admin.com";
        string password = "Test1234,";

        if (await userManager.FindByEmailAsync(email) == null)
        {
            var user = new IdentityUser();
            user.UserName = email;
            user.Email = email;
            user.EmailConfirmed = true;

            await userManager.CreateAsync(user, password);

            await userManager.AddToRoleAsync(user, "Administrator");
        }
    }
});



app.Run();
