using CorporateAssetManager.Data; 
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CorporateAssetManager.Models;
using CorporateAssetManager.Services;
using Microsoft.AspNetCore.Identity.UI.Services; 

var builder = WebApplication.CreateBuilder(args);

// --- 1. CONFIGURACI�N DE LA CONEXI�N A BASE DE DATOS ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Inyectamos el DbContext usando SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// --- 2. CONFIGURACI�N DE IDENTITY (Login) ---
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); // Necesario para Identity UI (Login, Register, etc.)

// Registrar IEmailSender (mock para desarrollo)
builder.Services.AddSingleton<IEmailSender, EmailSender>();

var app = builder.Build();

// Seeding the database with initial data
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        await DbInitializer.Initialize(dbContext, roleManager, userManager);
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

// Importante: Authentication debe ir ANTES de Authorization
app.UseAuthentication();
app.UseAuthorization();

// Importante: Razor Pages debe ir ANTES de ControllerRoute
app.MapRazorPages();

// Importante: Static Assets debe ir ANTES de ControllerRoute
app.MapStaticAssets();

// Importante: ControllerRoute debe ir AL FINAL
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();