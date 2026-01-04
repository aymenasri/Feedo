using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Feedo.Entities // <--- CORRIGÉ (C'était Entity)
{
    [Table("Utilisateur")]
    public class Utilisateur : Personne
    {
        [MaxLength(50)]
        public string Role { get; set; } = "User"; // "Admin" ou "User"
    }
}