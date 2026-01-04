using Feedo.Data;
using Feedo.Entities;
using Feedo.Models;
using Microsoft.EntityFrameworkCore;

namespace Feedo.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(int clientId, ShoppingCart cart, string deliveryAddress, string? notes);
        Task<bool> AssignCourierAsync(int orderId);
        Task<Order?> GetOrderByIdAsync(int orderId);
        Task<List<Order>> GetPendingOrdersAsync();
        Task<List<Order>> GetOrdersByClientAsync(int clientId);
        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus);
    }
    
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        
        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<Order> CreateOrderAsync(int clientId, ShoppingCart cart, string deliveryAddress, string? notes)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                // Create the order
                var order = new Order
                {
                    ClientId = clientId,
                    TotalAmount = cart.TotalAmount,
                    DeliveryAddress = deliveryAddress,
                    DeliveryNotes = notes,
                    Status = OrderStatus.Pending,
                    CreationA = DateTime.Now,
                    EstSupprimer = false,
                    SupperimeA = DateTime.Now,
                    SupperimerPar = 0
                };
                
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                
                // Create order items
                foreach (var cartItem in cart.Items)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = order.Id,
                        ProductId = cartItem.ProductId,
                        Quantity = cartItem.Quantity,
                        UnitPrice = cartItem.Price,
                        Subtotal = cartItem.Subtotal,
                        CreationA = DateTime.Now,
                        EstSupprimer = false,
                        SupperimeA = DateTime.Now,
                        SupperimerPar = 0
                    };
                    
                    _context.OrderItems.Add(orderItem);
                }
                
                await _context.SaveChangesAsync();
                
                // Try to assign a courier automatically
                await AssignCourierAsync(order.Id);
                
                await transaction.CommitAsync();
                
                return order;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        
        public async Task<bool> AssignCourierAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            
            if (order == null || order.CourierId != null)
                return false;
            
            // Find available courier with best rating
            var availableCourier = await _context.Couriers
                .Where(c => c.IsAvailable && !c.EstSupprimer)
                .OrderByDescending(c => c.Rating)
                .ThenBy(c => c.TotalDeliveries)
                .FirstOrDefaultAsync();
            
            if (availableCourier != null)
            {
                order.CourierId = availableCourier.Id;
                order.Status = OrderStatus.Assigned;
                order.AssignedAt = DateTime.Now;
                
                availableCourier.IsAvailable = false; // Mark as busy
                
                await _context.SaveChangesAsync();
                return true;
            }
            
            return false;
        }
        
        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Courier)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }
        
        public async Task<List<Order>> GetPendingOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Courier)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Where(o => o.Status == OrderStatus.Pending || o.Status == OrderStatus.Assigned)
                .OrderByDescending(o => o.CreationA)
                .ToListAsync();
        }
        
        public async Task<List<Order>> GetOrdersByClientAsync(int clientId)
        {
            return await _context.Orders
                .Include(o => o.Courier)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Where(o => o.ClientId == clientId)
                .OrderByDescending(o => o.CreationA)
                .ToListAsync();
        }
        
        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus)
        {
            var order = await _context.Orders
                .Include(o => o.Courier)
                .FirstOrDefaultAsync(o => o.Id == orderId);
            
            if (order == null)
                return false;
            
            order.Status = newStatus;
            
            if (newStatus == OrderStatus.Delivered)
            {
                order.DeliveredAt = DateTime.Now;
                
                if (order.Courier != null)
                {
                    order.Courier.IsAvailable = true;
                    order.Courier.TotalDeliveries++;
                    order.Courier.LastDeliveryAt = DateTime.Now;
                }
            }
            
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
