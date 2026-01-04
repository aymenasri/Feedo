using Feedo.Entities;
using Feedo.Models;

namespace Feedo.Services
{
    /// <summary>
    /// Service interface for authentication operations.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Authenticates a user with email and password.
        /// </summary>
        /// <param name="email">User's email</param>
        /// <param name="password">User's password (plain text)</param>
        /// <returns>The authenticated user if successful, otherwise null</returns>
        Task<Utilisateur?> AuthenticateAsync(string email, string password);

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="model">Registration data</param>
        /// <returns>The newly created user</returns>
        Task<Utilisateur> RegisterAsync(RegisterViewModel model);

        /// <summary>
        /// Check if an email is already in use.
        /// </summary>
        /// <param name="email">Email to check</param>
        /// <returns>True if email exists, false otherwise</returns>
        Task<bool> EmailExistsAsync(string email);
        
        /// <summary>
        /// Ensures that an admin account exists in the database.
        /// </summary>
        Task EnsureAdminExistsAsync();
    }
}
