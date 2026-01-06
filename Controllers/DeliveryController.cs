using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Feedo.Data;
using Feedo.Entities;
using Feedo.Shared;
using System.Security.Claims;

namespace Feedo.Controllers
{
    [Authorize(Roles = "Livreur")]
    public class DeliveryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DeliveryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Delivery/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var livreur = await _context.Livreurs
                .FirstOrDefaultAsync(l => l.Email == userEmail);

            if (livreur == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Livreur = livreur;
            
            // Get available orders (orders without a livreur assigned and not deleted)
            var availableOrders = await _context.Orders
                .Where(o => o.LivreurId == null && o.Status == OrderStatus.Pending && !o.EstSupprimer)
                .Include(o => o.Client)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .OrderBy(o => o.CreationA)
                .ToListAsync();

            var myDeliveries = await _context.Orders
                .Where(o => o.LivreurId == livreur.Id && 
                           (o.Status == OrderStatus.InDelivery || o.Status == OrderStatus.Confirmed || o.Status == OrderStatus.Pending || o.Status == OrderStatus.Assigned) &&
                           !o.EstSupprimer)
                .Include(o => o.Client)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .OrderBy(o => o.CreationA)
                .ToListAsync();

            ViewBag.AvailableOrders = availableOrders;
            ViewBag.MyDeliveries = myDeliveries;

            return View();
        }

        // POST: Delivery/AcceptOrder/5
        [HttpPost]
        [Route("Delivery/AcceptOrder/{id}")]
        public async Task<IActionResult> AcceptOrder(int id)
        {
            Console.WriteLine($"[DeliveryController] AcceptOrder called for ID: {id}");
            try
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var livreur = await _context.Livreurs
                    .FirstOrDefaultAsync(l => l.Email == userEmail);

                if (livreur == null)
                {
                    Console.WriteLine("[DeliveryController] Livreur not found");
                    return Json(new { success = false, message = "Livreur non trouvé" });
                }

                var order = await _context.Orders.FindAsync(id);
                if (order == null || order.LivreurId != null)
                {
                    Console.WriteLine($"[DeliveryController] Order {id} not available or already assigned");
                    return Json(new { success = false, message = "Commande non disponible" });
                }

                // Assign order to livreur
                order.LivreurId = livreur.Id;
                order.Status = OrderStatus.InDelivery;
                order.AssignedAt = DateTime.Now; // Ensure this is set
                
                // Update livreur status
                livreur.Status = LivreurStatus.Busy;
                
                await _context.SaveChangesAsync();
                Console.WriteLine($"[DeliveryController] Order {id} accepted by {livreur.Nom}");

                return Json(new { success = true, message = "Commande acceptée" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DeliveryController] Error in AcceptOrder: {ex.Message}");
                return Json(new { success = false, message = "Erreur: " + ex.Message });
            }
        }

        // POST: Delivery/RefuseOrder/5
        [HttpPost]
        [Route("Delivery/RefuseOrder/{id}")]
        public async Task<IActionResult> RefuseOrder(int id)
        {
            Console.WriteLine($"[DeliveryController] RefuseOrder called for ID: {id}");
            try
            {
                var order = await _context.Orders.FindAsync(id);
                
                if (order == null)
                {
                    return Json(new { success = false, message = "Commande non trouvée" });
                }

                // Mark order as cancelled and deleted (soft delete)
                order.Status = OrderStatus.Cancelled;
                order.EstSupprimer = true;
                order.SupperimeA = DateTime.Now;
                
                await _context.SaveChangesAsync();
                Console.WriteLine($"[DeliveryController] Order {id} refused and deleted");

                return Json(new { success = true, message = "Commande refusée et supprimée" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DeliveryController] Error in RefuseOrder: {ex.Message}");
                return Json(new { success = false, message = "Erreur: " + ex.Message });
            }
        }

        // POST: Delivery/CompleteDelivery/5
        [HttpPost]
        [Route("Delivery/CompleteDelivery/{id}")]
        public async Task<IActionResult> CompleteDelivery(int id)
        {
            Console.WriteLine($"[DeliveryController] CompleteDelivery called for ID: {id}");
            try
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var livreur = await _context.Livreurs
                    .FirstOrDefaultAsync(l => l.Email == userEmail);

                if (livreur == null)
                {
                    return Json(new { success = false, message = "Livreur non trouvé" });
                }

                var order = await _context.Orders.FindAsync(id);
                if (order == null || order.LivreurId != livreur.Id)
                {
                    return Json(new { success = false, message = "Commande non trouvée ou non assignée" });
                }

                // Mark order as delivered
                order.Status = OrderStatus.Delivered;
                order.DeliveredAt = DateTime.Now;
                
                // Update livreur stats
                livreur.TotalDeliveries++;
                livreur.LastDeliveryAt = DateTime.Now;
                
                // Check if livreur has other active deliveries
                var hasOtherDeliveries = await _context.Orders
                    .AnyAsync(o => o.LivreurId == livreur.Id && 
                                  o.Status == OrderStatus.InDelivery && 
                                  o.Id != id);
                
                if (!hasOtherDeliveries)
                {
                    livreur.Status = LivreurStatus.Available;
                }
                
                await _context.SaveChangesAsync();
                Console.WriteLine($"[DeliveryController] Order {id} completed");

                return Json(new { success = true, message = "Livraison terminée" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DeliveryController] Error in CompleteDelivery: {ex.Message}");
                return Json(new { success = false, message = "Erreur: " + ex.Message });
            }
        }

        // POST: Delivery/ToggleAvailability
        [HttpPost]
        public async Task<IActionResult> ToggleAvailability()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var livreur = await _context.Livreurs
                .FirstOrDefaultAsync(l => l.Email == userEmail);

            if (livreur == null)
            {
                return Json(new { success = false });
            }

            // Toggle between Available and Offline (can't toggle if Busy)
            if (livreur.Status == LivreurStatus.Available)
            {
                livreur.Status = LivreurStatus.Offline;
            }
            else if (livreur.Status == LivreurStatus.Offline)
            {
                livreur.Status = LivreurStatus.Available;
            }
            
            await _context.SaveChangesAsync();

            return Json(new { success = true, newStatus = livreur.Status.ToString() });
        }
    }
}
