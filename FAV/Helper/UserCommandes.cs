using FAV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FAV.Helper
{
    public class UserCommandes
    {
        public UtilisateurModels? utilisateurCommande { get; set; } = new();
        public CommandeModels? commandeUser { get; set; } = new();
        public List<ProduitModels>? produits { get; set; } = new();
        public List<int>? quantite { get; set; } = new();
        public List<BitmapImage>? imagesDesProduit { get; set; } = new();
        public BitmapImage? imageUser { get; set; } = new();
    }
}
