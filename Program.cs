using Feedo.Data;
using Feedo.Repository;
using Feedo.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. D'ABORD : On récupère la chaîne de connexion depuis appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. ENSUITE : On ajoute les services (DbContext, Controllers, etc.)
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Utilisation de la variable connectionString qui est maintenant bien définie
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 3. Configuration du Repository Pattern et Service Layer
// Generic Repository - fonctionne pour toutes les entités
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Client-specific Repository et Service
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IClientService, ClientService>();

// Authentication services
builder.Services.AddScoped<IUtilisateurRepository, UtilisateurRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Order Management Service
builder.Services.AddScoped<IOrderService, OrderService>();

// Session and Authentication
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(2);
        options.SlidingExpiration = true;
    });


var app = builder.Build();

// Configuration du pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Utilise UseStaticFiles pour la compatibilité standard

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// Ensure admin account exists
using (var scope = app.Services.CreateScope())
{
    var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
    await authService.EnsureAdminExistsAsync();
    
    // Run Diagnostics
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    Feedo.Scripts.OrderDiagnostics.Run(context);
}

// Configuration des routes
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();