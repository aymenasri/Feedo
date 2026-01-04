using System.ComponentModel.DataAnnotations.Schema;

namespace Feedo.Entities // <--- CORRIGÉ (C'était Entity)
{
    [Table("Livreur")]
    public class Livreur : Personne
    {
    }
}