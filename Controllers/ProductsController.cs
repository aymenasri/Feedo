using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Feedo.Data;
using Feedo.Entities;
using Feedo.Shared;
using System.Linq;

namespace Feedo.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Products/Categories
        public IActionResult Categories() => View();

        // GET: Products
        public async Task<IActionResult> Index(string location = null)
        {
            var query = _context.Products.AsNoTracking();
            
            // Filter by location if specified
            if (!string.IsNullOrEmpty(location))
            {
                if (Enum.TryParse<ProductDisplayLocation>(location, out var displayLocation))
                {
                    query = query.Where(p => p.DisplayLocation == displayLocation);
                    ViewData["FilterTitle"] = displayLocation == ProductDisplayLocation.HomeCarousel 
                        ? "Hero Promotions" 
                        : displayLocation == ProductDisplayLocation.MenuPage
                            ? "Menu Page Items"
                            : "Product Catalog";
                }
            }
            else
            {
                ViewData["FilterTitle"] = "All Products";
            }
            
            var products = await query.ToListAsync();
            return View(products);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create() => View();

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile? imageFile)
        {
            // Log for debugging
            Console.WriteLine($"Form submitted - Name: {product.Name}, Price: {product.Price}");
            Console.WriteLine($"Discount: {product.DiscountPercentage}%");
            Console.WriteLine($"Image file: {imageFile?.FileName ?? "No file"}");
            Console.WriteLine($"ModelState Valid: {ModelState.IsValid}");
            
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Validation Error: {error.ErrorMessage}");
                }
                return View(product);
            }

            // Calculate OldPrice from Price and DiscountPercentage
            if (product.DiscountPercentage.HasValue && product.DiscountPercentage.Value > 0)
            {
                // Price is the discounted price
                // OldPrice = Price / (1 - Discount%)
                // Example: If Price = 80 and Discount = 20%, then OldPrice = 80 / 0.8 = 100
                var discountFactor = 1 - (product.DiscountPercentage.Value / 100);
                product.OldPrice = Math.Round(product.Price / discountFactor, 2);
            }
            else
            {
                product.OldPrice = null;
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                // Generate unique filename
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");
                
                // Create folder if not exists
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);
                    
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                product.ImageUrl = "/images/products/" + fileName;
            }

            product.CreationA = DateTime.Now;
            _context.Add(product);
            await _context.SaveChangesAsync();
            
            Console.WriteLine("Product saved successfully!");
            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product, IFormFile? imageFile)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Handle Image Upload
                     if (imageFile != null && imageFile.Length > 0)
                    {
                        // Generate unique filename
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");
                        
                        // Create folder if not exists
                        if (!Directory.Exists(uploadsFolder))
                            Directory.CreateDirectory(uploadsFolder);
                            
                        string filePath = Path.Combine(uploadsFolder, fileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(fileStream);
                        }

                        // Delete old image if exists and different
                         if (!string.IsNullOrEmpty(product.ImageUrl) && product.ImageUrl.StartsWith("/images/products/"))
                        {
                            // Optional: Cleanup old image logic could go here
                        }

                        product.ImageUrl = "/images/products/" + fileName;
                    }
                    else 
                    {
                        // Keep existing image if no new file uploaded
                        // We need to detach the entity or use AsNoTracking to get original, OR simple hidden field in View
                        // For now assuming the View passes back the hidden ImageUrl, otherwise we might lose it.
                        // Better approach: Get original from DB to preserve ImageUrl if not in form
                        var originalProduct = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
                        if (originalProduct != null && string.IsNullOrEmpty(product.ImageUrl))
                        {
                            product.ImageUrl = originalProduct.ImageUrl;
                        }
                    }

                    // Recalculate OldPrice logic
                    if (product.DiscountPercentage.HasValue && product.DiscountPercentage.Value > 0)
                    {
                        var discountFactor = 1 - (product.DiscountPercentage.Value / 100);
                        product.OldPrice = Math.Round(product.Price / discountFactor, 2);
                    }
                    else
                    {
                        product.OldPrice = null;
                    }

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        // POST: Products/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            
            if (product == null)
            {
                return NotFound();
            }

            // Delete the image file if it exists
            if (!string.IsNullOrEmpty(product.ImageUrl) && product.ImageUrl.StartsWith("/images/products/"))
            {
                string fileName = Path.GetFileName(product.ImageUrl);
                string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products", fileName);
                
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }
    }
}