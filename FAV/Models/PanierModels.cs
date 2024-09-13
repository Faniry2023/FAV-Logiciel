using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FAV.Models
{
    public class PanierModels
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("id_produit")]
        public string? Id_produit { get; set; }
        [JsonPropertyName("id_acheteur")]
        public string? id_acheteur { get; set; }
        [JsonPropertyName("id_vendeur")]
        public string? id_vendeur { get; set; }
        [JsonPropertyName("prix_total")]
        public double prix_total { get; set; }
        [JsonPropertyName("quantite")]
        public int quantite { get; set; }
    }
}
