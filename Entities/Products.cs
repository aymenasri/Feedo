using System.ComponentModel.DataAnnotations;
using Feedo.Shared;

namespace Feedo.Entities
{
    public class Product : Base
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        
        [Required]
        public decimal Price { get; set; }
        
        public decimal? DiscountPercentage { get; set; }
        
        // Calculated from Price and DiscountPercentage
        public decimal? OldPrice { get; set; }
        
        public string? ImageUrl { get; set; }
        public bool IsAvailable { get; set; } = true;
        public ProductDisplayLocation DisplayLocation { get; set; } = ProductDisplayLocation.Default;
    }
}