using System.ComponentModel.DataAnnotations.Schema;
using Feedo.Shared;
namespace Feedo.Entities
{
    [Table("Commande")]

    public class Commande : Base
    {
        public Commande() { }
        public Decimal TotalHT { get; set; }
        public Decimal TotalTTC { get; set; }
        public int NCommande { get; set; }
        public int ClientId { get; set; }
        public int LivreurId { get; set; }
        public TypeCommande Statut { get; set; }
    }

    [Table("CommandeDetail")]

    public class CommandeDetail : Base
    {
        public CommandeDetail() { }
        public int CommandeId { get; set; }
        public int ProduitId { get; set; }
        public int Quantite { get; set; }
        public Decimal PrixUnitaire { get; set; }
        public Decimal TotalLigne { get; set; }
    }

}
