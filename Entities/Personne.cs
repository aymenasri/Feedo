using System.ComponentModel.DataAnnotations;

namespace Feedo.Entities // <--- CORRIGÉ (C'était Entity)
{
    public class Personne : Base
    {
        public string Prenom { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string? Nom { get; set; }

        public string NumeroTelephone { get; set; } = string.Empty;
        
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; } = string.Empty;
        
        public string Compte { get; set; } = string.Empty;
        public string MotPasse { get; set; } = string.Empty;
    }
}