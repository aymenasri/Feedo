using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Feedo.Data;
using Feedo.Entities;

namespace Feedo.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Active Deliveries (InDelivery)
            var activeDeliveries = await _context.Orders
                .CountAsync(o => o.Status == OrderStatus.InDelivery && !o.EstSupprimer);

            // Pending Orders (Pending)
            var pendingOrders = await _context.Orders
                .CountAsync(o => o.Status == OrderStatus.Pending && !o.EstSupprimer);

            // Success Rate (Delivered / (Delivered + Cancelled))
            var deliveredCount = await _context.Orders
                .CountAsync(o => o.Status == OrderStatus.Delivered && !o.EstSupprimer);
            var cancelledCount = await _context.Orders
                .CountAsync(o => o.Status == OrderStatus.Cancelled && !o.EstSupprimer);
            
            var totalCompleted = deliveredCount + cancelledCount;
            double successRate = totalCompleted > 0 
                ? (double)deliveredCount / totalCompleted * 100 
                : 100; // Default to 100% if no completed/cancelled orders yet

            // Daily Revenue (Delivered today)
            var today = DateTime.Today;
            var dailyRevenue = await _context.Orders
                .Where(o => o.Status == OrderStatus.Delivered && !o.EstSupprimer && o.DeliveredAt >= today)
                .SumAsync(o => o.TotalAmount);

            ViewBag.ActiveDeliveries = activeDeliveries;
            ViewBag.PendingOrders = pendingOrders;
            ViewBag.SuccessRate = successRate.ToString("0.0") + "%";
            ViewBag.DailyRevenue = dailyRevenue.ToString("C");

            // Recent Orders for Live Tracking
            var recentOrders = await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Livreur)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Where(o => !o.EstSupprimer)
                .OrderByDescending(o => o.CreationA)
                .Take(5)
                .ToListAsync();

            return View(recentOrders);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                // Hard delete as requested ("supprime reelement")
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
