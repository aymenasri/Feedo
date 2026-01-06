using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Feedo.Data;
using System.Security.Claims;

namespace Feedo.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Orders/MyOrders
        public async Task<IActionResult> MyOrders()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            
            //Find client by email
            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.Email == userEmail);

            if (client == null)
            {
                ViewBag.Orders = new List<Feedo.Entities.Order>();
                return View();
            }

            // Get all orders for this client
            var orders = await _context.Orders
                .Where(o => o.ClientId == client.Id && !o.EstSupprimer)
                .Include(o => o.Livreur)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.CreationA)
                .ToListAsync();

            ViewBag.Orders = orders;
            return View();
        }
        // POST: Orders/DeleteOrder/5
        [HttpPost]
        [Route("Orders/DeleteOrder/{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            Console.WriteLine($"[DeleteOrder] Request received for Order ID: {id}");
            try
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var client = await _context.Clients
                    .FirstOrDefaultAsync(c => c.Email == userEmail);

                if (client == null)
                    return RedirectToAction("MyOrders");

                var order = await _context.Orders.FindAsync(id);

                if (order == null || order.ClientId != client.Id)
                    return RedirectToAction("MyOrders");

                // Check status
                if (order.Status != Feedo.Entities.OrderStatus.Delivered && order.Status != Feedo.Entities.OrderStatus.Pending)
                {
                    TempData["Error"] = "Impossible de supprimer cette commande (statut invalide).";
                    return RedirectToAction("MyOrders");
                }

                // Soft delete
                order.EstSupprimer = true;
                order.SupperimeA = DateTime.Now;
                
                await _context.SaveChangesAsync();
                Console.WriteLine("[DeleteOrder] Order marked as deleted.");
                
                TempData["Success"] = "Commande supprimée avec succès.";
                return RedirectToAction("MyOrders");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DeleteOrder] EXCEPTION: {ex.Message}");
                TempData["Error"] = "Erreur serveur lors de la suppression.";
                return RedirectToAction("MyOrders");
            }
        }
    }
}
