using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FAV.Models
{
    public class Image_produitModels
    {
        [Key]
        [JsonPropertyName("id_Image")]
        public string? Id_image { get; set; }
        [JsonPropertyName("id_produit")]
        public string? Id_produit { get; set; }
        [JsonPropertyName("image_couv")]
        public byte[]? Image_couv { get; set; }
        [JsonPropertyName("image_1")]
        public byte[]? Image_1 { get; set; }
        [JsonPropertyName("image_2")]
        public byte[]? Image_2 { get; set; }
        [JsonPropertyName("image_3")]
        public byte[]? Image_3 { get; set; }
        [JsonPropertyName("image_4")]
        public byte[]? Image_4 { get; set; }
    }
}
