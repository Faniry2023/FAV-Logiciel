using FAV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FAV.Helper
{
    class ListProdCom
    {
        public ProduitModels? lesProduits { get; set; } = new();
        public BitmapImage lesImages { get; set; } = new();
        public int quantite {  get; set; }
    }
}
