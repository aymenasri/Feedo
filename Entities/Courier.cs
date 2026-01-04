using System.ComponentModel.DataAnnotations;

namespace Feedo.Entities
{
    public class Courier : Personne
    {
        [Required]
        public string VehicleType { get; set; } = "Bike"; // Bike, Scooter, Car
        
        public string? LicensePlate { get; set; }
        
        [Required]
        public bool IsAvailable { get; set; } = true;
        
        public DateTime? LastDeliveryAt { get; set; }
        
        public int TotalDeliveries { get; set; } = 0;
        
        [Range(0, 5)]
        public decimal Rating { get; set; } = 0;
        
        // Navigation property
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
