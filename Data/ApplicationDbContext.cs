using Feedo.Entities; // On garde seulement celui-ci
using Microsoft.EntityFrameworkCore;

namespace Feedo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Livreur> Livreurs { get; set; }
        public DbSet<Utilisateur> Utilisateurs { get; set; }
        // public DbSet<Commande> Commandes { get; set; } // Removed as we use Order
        public DbSet<Product> Products { get; set; }
        
        // Order Management System
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        // public DbSet<Courier> Couriers { get; set; } // Removed as we are using Livreur
    }
}