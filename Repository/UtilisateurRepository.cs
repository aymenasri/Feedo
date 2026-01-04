using Feedo.Data;
using Feedo.Entities;
using Microsoft.EntityFrameworkCore;

namespace Feedo.Repository
{
    /// <summary>
    /// Repository implementation for Utilisateur entity.
    /// Provides user-specific data access methods.
    /// </summary>
    public class UtilisateurRepository : Repository<Utilisateur>, IUtilisateurRepository
    {
        public UtilisateurRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Utilisateur?> GetByEmailAsync(string email)
        {
            return await _context.Utilisateurs
                .FirstOrDefaultAsync(u => u.Email == email || u.Compte == email);
        }
    }
}
