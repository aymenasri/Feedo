using Microsoft.AspNetCore.Mvc;
using Feedo.Data;
using Feedo.Models;
using Feedo.Services;
using Feedo.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Feedo.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IOrderService _orderService;
        
        public CartController(ApplicationDbContext context, IOrderService orderService)
        {
            _context = context;
            _orderService = orderService;
        }
        
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity, List<string> extras)
        {
            var product = await _context.Products.FindAsync(productId);
            
            if (product == null || !product.IsAvailable)
            {
                return NotFound();
            }
            
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            
            string finalName = product.Name;
            decimal finalPrice = product.Price;

            if (extras != null && extras.Any())
            {
                finalName += " (" + string.Join(", ", extras) + ")";
                
                // Simple logic for extras pricing (Demo purposes)
                foreach(var extra in extras)
                {
                    if (extra.Contains("Cheese")) finalPrice += 1.00m;
                    else if (extra.Contains("Boisson")) finalPrice += 2.00m;
                    else if (extra.Contains("Bacon")) finalPrice += 1.50m;
                }
            }

            cart.AddItem(product.Id, finalName, finalPrice, quantity, product.ImageUrl);
            
            HttpContext.Session.SetObjectAsJson("Cart", cart);
            
            return Json(new { success = true, cartCount = cart.TotalItems });
        }
        
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            return View(cart);
        }
        
        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            cart.UpdateQuantity(productId, quantity);
            HttpContext.Session.SetObjectAsJson("Cart", cart);
            
            return Json(new { success = true, total = cart.TotalAmount });
        }
        
        [HttpPost]
        public IActionResult RemoveItem(int productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            cart.RemoveItem(productId);
            HttpContext.Session.SetObjectAsJson("Cart", cart);
            
            return Json(new { success = true });
        }
        
        [HttpPost]
        public async Task<IActionResult> Checkout(string deliveryAddress, string? notes)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            
            if (cart == null || !cart.Items.Any())
            {
                return RedirectToAction("Index", "Home");
            }
            
            // Get current user email from claims
            var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            
            if (string.IsNullOrEmpty(userEmail))
            {
                return BadRequest("User email not found in claims");
            }
            
            // First try to find existing client
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Email == userEmail);
            
            // If no client exists, create one from the current Utilisateur
            if (client == null)
            {
                var utilisateur = await _context.Utilisateurs.FirstOrDefaultAsync(u => u.Email == userEmail);
                
                if (utilisateur == null)
                {
                    return BadRequest("User not found");
                }
                
                // Create a client record from the utilisateur
                client = new Feedo.Entities.Client
                {
                    Prenom = utilisateur.Prenom,
                    Nom = utilisateur.Nom,
                    Email = utilisateur.Email,
                    Compte = utilisateur.Compte,
                    NumeroTelephone = utilisateur.NumeroTelephone,
                    MotPasse = utilisateur.MotPasse,
                    CreationA = DateTime.Now,
                    EstSupprimer = false,
                    SupperimeA = DateTime.Now,
                    SupperimerPar = 0
                };
                
                _context.Clients.Add(client);
                await _context.SaveChangesAsync();
            }
            
            var order = await _orderService.CreateOrderAsync(client.Id, cart, deliveryAddress, notes);
            
            // Clear the cart
            HttpContext.Session.Remove("Cart");
            
            return RedirectToAction("OrderConfirmation", new { orderId = order.Id });
        }
        
        public async Task<IActionResult> OrderConfirmation(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            
            if (order == null)
            {
                return NotFound();
            }
            
            return View(order);
        }
    }
}
