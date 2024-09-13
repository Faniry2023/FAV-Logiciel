using FAV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAV.Helper
{
    public class DonneConnectedModel
    {
        public List<ProduitModels>? produitModels { get; set; } = new();
        public List<Image_produitModels>? imageProduitModels { get; set; } = new();
        public List<PubliciteModels>? publiciteModels { get; set; } = new();
        public List<CommandeModels>? toutCommande {  get; set; } = new();
        public List<StatMois>? listStatMois { get; set; } = new();
        public string? IdUserConnected { get; set; }
        public string? nomEntreprise {  get; set; }
        public string? IdVendeurConnected { get; set; }
    }
}
