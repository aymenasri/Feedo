using Microsoft.AspNetCore.Mvc;
using Feedo.Data;
using Feedo.Entities;
using Feedo.Shared;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Feedo.Controllers; 

public class HomeController : Controller 
{
    private readonly ApplicationDbContext _context; 

    public HomeController(ApplicationDbContext context) 
    {
        _context = context;
    }

    public async Task<IActionResult> Index() 
    {
        // Livreur Earnings Logic
        if (User.Identity.IsAuthenticated && User.IsInRole("Livreur"))
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (!string.IsNullOrEmpty(userEmail))
            {
                var livreur = await _context.Livreurs.FirstOrDefaultAsync(l => l.Email == userEmail);
                if (livreur != null)
                {
                    // Calculate start of current week (Monday)
                    var today = DateTime.Today;
                    var monday = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday);
                    
                    var weeklyOrdersCount = await _context.Orders
                        .Where(o => o.LivreurId == livreur.Id 
                                 && o.Status == OrderStatus.Delivered 
                                 && o.DeliveredAt >= monday)
                        .CountAsync();
                        
                    // Fixed rate: 2.99 € per order
                    var earnings = weeklyOrdersCount * 2.99m;
                    ViewBag.LivreurWeeklyEarnings = earnings;
                }
            }
        }

        // Modified to use _context and filter by DisplayLocation
        var products = await _context.Products
            .Where(p => p.DisplayLocation == ProductDisplayLocation.HomeCarousel)
            .ToListAsync();
        return View(products);
    }

    // GET: /Catalog - Public product catalog
    public async Task<IActionResult> Catalog()
    {
        var products = await _context.Products
            .Where(p => (p.DisplayLocation == ProductDisplayLocation.Default || p.DisplayLocation == ProductDisplayLocation.MenuPage) && p.IsAvailable)
            .OrderBy(p => p.Name)
            .ToListAsync();
        return View(products);
    }
    
    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Welcome(string name, int numTimes = 1)
    {
        ViewData["Message"] = "Hello " + name;
        ViewData["NumTimes"] = numTimes;
        return View();
    }
}