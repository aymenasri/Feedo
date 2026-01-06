using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Feedo.Shared;

namespace Feedo.Entities // <--- CORRIGÉ (C'était Entity)
{
    [Table("Livreur")]
    public class Livreur : Personne
    {
        [Required]
        public string VehicleType { get; set; } = "Bike"; // Bike, Scooter, Car
        
        public string? LicensePlate { get; set; }
        
        [Required]
        public LivreurStatus Status { get; set; } = LivreurStatus.Offline;
        
        // Computed property for backward compatibility or ease of use (optional, but good for views)
        [NotMapped]
        public bool IsAvailable => Status == LivreurStatus.Available;
        
        public DateTime? LastDeliveryAt { get; set; }
        
        public int TotalDeliveries { get; set; } = 0;
        
        [Range(0, 5)]
        public decimal Rating { get; set; } = 0;

        // Navigation property for Order (if referenced) - Keeping it optional for now to avoid circular dependency issues during migration if Order is not updated yet.
        // But since we are updating Order.cs next, we can anticipate it.
        // public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}