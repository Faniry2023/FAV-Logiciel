using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FAV.Models
{
    public class PubliciteModels
    {
        [Key]
        [JsonPropertyName("id_pub")]
        public Guid Id_pub { get; set; }
        [JsonPropertyName("id_utilisateur")]
        public string? Id_utilisateur { get; set; }
        [JsonPropertyName("nom_pub")]
        public string? Nom_pub { get; set; }
        [JsonPropertyName("descri_pub")]
        public string? Descri_pub { get; set; }
        [JsonPropertyName("autre_descri")]
        public string? Autre_descri { get; set; }
        [JsonPropertyName("photo")]
        public byte[]? Photo { get; set; }
        [JsonPropertyName("photoUrl")]
        public string? PhotoUrl { get; set; }
        [JsonPropertyName("date_pub")]
        public DateTime Date_pub { get; set; }
    }
}
