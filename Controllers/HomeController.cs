using Microsoft.AspNetCore.Mvc;
using Feedo.Data;
using Feedo.Entities;
using Feedo.Shared;
using Microsoft.EntityFrameworkCore;

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