using System.ComponentModel.DataAnnotations;

namespace FAV.Models
{
    public class HistoriqueModels
    {
        [Key]
        public Guid Id { get; set; }
        public string? Id_acheteur {  get; set; }
        public string? Id_vendeur { get; set; }
        public string? Id_produit { get; set; }
        public string? Id_commande { get; set; }
        public double Prix_a_payser { get; set; }
        public string? Date_achat { get; set; }
        public string? Nom_produit { get; set; }
        public int Quantite { get; set; }
    }
}
