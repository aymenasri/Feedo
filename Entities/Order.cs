using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Feedo.Entities
{
    public class Order : Base
    {
        [Required]
        public int ClientId { get; set; }
        
        [ForeignKey("ClientId")]
        public virtual Client? Client { get; set; }
        
        public int? LivreurId { get; set; }
        
        [ForeignKey("LivreurId")]
        public virtual Livreur? Livreur { get; set; }
        
        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        
        public string? DeliveryAddress { get; set; }
        
        public string? DeliveryNotes { get; set; }
        
        public DateTime? AssignedAt { get; set; }
        
        public DateTime? DeliveredAt { get; set; }
        
        // Navigation property
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
    
    public enum OrderStatus
    {
        Pending = 0,        // En attente d'affectation de livreur
        Assigned = 1,       // Affecté à un livreur
        InProgress = 2,     // En cours de livraison
        Delivered = 3,      // Livré
        Cancelled = 4       // Annulé
    }
}
