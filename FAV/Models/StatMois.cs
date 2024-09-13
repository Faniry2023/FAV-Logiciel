using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FAV.Models
{
    public class StatMois
    {
        [Key]
        [JsonPropertyName("id")]
        public Guid id { get; set; }
        [JsonPropertyName("id_produit")]
        public string? id_produit { get; set; }
        [JsonPropertyName("jan")]
        public int jan { get; set; }
        [JsonPropertyName("fev")]
        public int fev { get; set; }
        [JsonPropertyName("mar")]
        public int mar { get; set; }
        [JsonPropertyName("avr")]
        public int avr { get; set; }
        [JsonPropertyName("mai")]
        public int mai { get; set; }
        [JsonPropertyName("jui")]
        public int jui { get; set; }
        [JsonPropertyName("juill")]
        public int juill { get; set; }
        [JsonPropertyName("aou")]
        public int aou { get; set; }
        [JsonPropertyName("sep")]
        public int sep { get; set; }
        [JsonPropertyName("oct")]
        public int oct { get; set; }
        [JsonPropertyName("nov")]
        public int nov { get; set; }
        [JsonPropertyName("dec")]
        public int dec { get; set; }
    }
}
