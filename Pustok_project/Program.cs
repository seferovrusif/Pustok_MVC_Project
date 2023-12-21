using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pustok_project.Contexts;
using Pustok_project.ExternalServices.Implements;
using Pustok_project.ExternalServices.Interfaces;
using Pustok_project.Helpers;
using Pustok_project.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<PustokDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionStrings:MSSql"]);
}).AddIdentity<AppUser, IdentityRole>(opt => {
    opt.SignIn.RequireConfirmedEmail = false;
    opt.User.RequireUniqueEmail = true;
    opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz0123456789._-";
    opt.Lockout.MaxFailedAccessAttempts = 5;
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequiredLength = 4;
}).AddDefaultTokenProviders().AddEntityFrameworkStores<PustokDbContext>();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = new PathString("/Auth/Login");
    options.LogoutPath = new PathString("/Auth/Logout");
    options.AccessDeniedPath = new PathString("/Home/AccessDenied");
    options.AccessDeniedPath = new PathString("/Home/AccessDenied");

    options.Cookie = new()
    {
        Name = "IdentityCookie",
        HttpOnly = true,
        SameSite = SameSiteMode.Lax,
        SecurePolicy = CookieSecurePolicy.Always
    };
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
});
builder.Services.AddSession();
//builder.Services.AddHttpContextAcce
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<FooterServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Slider}/{action=Index}/{id?}"
);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
PathConstants.RootPath = builder.Environment.WebRootPath;

app.Run();
