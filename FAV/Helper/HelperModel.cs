using FAV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FAV.Helper
{
    public class HelperModel
    {
        [JsonPropertyName("produit")]
        public ProduitModels? Produit { get; set; }
        [JsonPropertyName("image")]
        public Image_produitModels? Image { get; set; }
    }
}
