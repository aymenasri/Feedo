using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Feedo.Data;
using Feedo.Entities;

namespace Feedo.Scripts
{
    public class OrderDiagnostics
    {
        public static void Run(ApplicationDbContext context)
        {
            Console.WriteLine("--- DIAGNOSTICS: ORDERS ---");
            var orders = context.Orders.ToList();
            Console.WriteLine($"Total Orders: {orders.Count}");
            foreach (var o in orders)
            {
                Console.WriteLine($"Order #{o.Id}: Status={o.Status}, LivreurId={o.LivreurId}, Suppr={o.EstSupprimer}");
            }
            Console.WriteLine("---------------------------");
        }
    }
}
