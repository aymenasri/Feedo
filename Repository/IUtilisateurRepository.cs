using Feedo.Entities;

namespace Feedo.Repository
{
    /// <summary>
    /// Repository interface specific to Utilisateur entity.
    /// Extends the generic repository with Utilisateur-specific operations.
    /// </summary>
    public interface IUtilisateurRepository : IRepository<Utilisateur>
    {
        /// <summary>
        /// Retrieves a user by their email address.
        /// </summary>
        /// <param name="email">The email to search for</param>
        /// <returns>The user if found, otherwise null</returns>
        Task<Utilisateur?> GetByEmailAsync(string email);
    }
}
