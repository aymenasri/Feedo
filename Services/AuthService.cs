using BCrypt.Net;
using Feedo.Entities;
using Feedo.Models;
using Feedo.Repository;

namespace Feedo.Services
{
    /// <summary>
    /// Service implementation for authentication operations.
    /// Handles user registration, login, and password management.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IUtilisateurRepository _utilisateurRepository;

        public AuthService(IUtilisateurRepository utilisateurRepository)
        {
            _utilisateurRepository = utilisateurRepository;
        }

        public async Task<Utilisateur?> AuthenticateAsync(string email, string password)
        {
            // Find user by email
            var user = await _utilisateurRepository.GetByEmailAsync(email);
            
            if (user == null)
                return null;

            // Verify password using BCrypt
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.MotPasse);
            
            if (!isPasswordValid)
                return null;

            return user;
        }

        public async Task<Utilisateur> RegisterAsync(RegisterViewModel model)
        {
            // Check if email already exists
            if (await EmailExistsAsync(model.Email))
            {
                throw new InvalidOperationException("Un compte avec cet email existe déjà.");
            }

            // Create new user
            var user = new Utilisateur
            {
                Prenom = model.FirstName,
                Nom = model.LastName,
                Email = model.Email,
                Compte = model.Email, // Also store in Compte for compatibility
                NumeroTelephone = model.PhoneNumber,
                MotPasse = HashPassword(model.Password),
                CreationA = DateTime.Now
            };

            // Save to database
            return await _utilisateurRepository.AddAsync(user);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            var user = await _utilisateurRepository.GetByEmailAsync(email);
            return user != null;
        }

        public async Task EnsureAdminExistsAsync()
        {
            // Check if admin account already exists
            var admin = await _utilisateurRepository.GetByEmailAsync("admin@feedo.com");
            
            if (admin == null)
            {
                // Create admin account
                var adminUser = new Utilisateur
                {
                    Prenom = "Admin",
                    Nom = "Feedo",
                    Email = "admin@feedo.com",
                    Compte = "admin@feedo.com",
                    NumeroTelephone = "0000000000",
                    MotPasse = HashPassword("Admin@123"),
                    Role = "Admin",
                    CreationA = DateTime.Now,
                    EstSupprimer = false
                };
                
                await _utilisateurRepository.AddAsync(adminUser);
            }
        }

        /// <summary>
        /// Hashes a password using BCrypt.
        /// </summary>
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
