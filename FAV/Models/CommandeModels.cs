using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FAV.Models
{
    public class CommandeModels
    {
        [Key]
        [JsonPropertyName("id_commande")]
        public Guid Id_commande { get; set; }
        [JsonPropertyName("id_acheteur")]
        public string? Id_acheteur { get; set; }
        [JsonPropertyName("id_vendeur")]
        public string? Id_vendeur { get; set; }
        [JsonPropertyName("lesIdProduit")]
        public string? lesIdProduit { get; set; }
        [JsonPropertyName("quantite")]
        public string? quantite { get; set; }
        [JsonPropertyName("prixTotal")]
        public double prixTotal { get; set; }
        [JsonPropertyName("date_pub")]
        public DateTime? Date_pub { get; set; }
        [JsonPropertyName("lieu")]
        public string? Lieu { get; set; }
        [JsonPropertyName("latitude")]
        public double latitude { get; set; }
        [JsonPropertyName("longitude")]
        public double longitude { get; set; }
        [JsonPropertyName("is_livrer")]
        public bool is_livrer { get; set; }
    }
}
