using FAV.Helper;
using FAV.ListModels;
using FAV.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FAV.UCs.addProduit
{
    /// <summary>
    /// Logique d'interaction pour UCAddProduit.xaml
    /// </summary>
    public partial class UCAddProduit : UserControl
    {
        private string? idUser;
        private string? idVendeur;
        private string? filePath1;
        private string? filePath2;
        private string? filePath3;
        private string? filePath4;
        private string? filePathCouv;
        private List<ListBoxAddDescri> listeDescri;
        public UCAddProduit(string? idUser, string? idVendeur)
        {
            InitializeComponent();
            ChargementToutDonnee();
            this.idUser = idUser;
            this.idVendeur = idVendeur;
            listeDescri = new();
        }

        //Methode

        #region Mes méthodes
        public void ClearAllControll()
        {
            txtNomProduit.Text = string.Empty;
            txtMarque.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtNbTotalProd.Text = string.Empty;
            txtPrix.Text = string.Empty;
            txtPromo.Text = string.Empty;
            ImgCouv.Source = null;
            Img1.Source = null;
            Img2.Source = null;
            Img3.Source = null;
            Img4.Source = null;
            ListBoxDescri.ItemsSource = null;
            listeDescri = new();
            
        }
        public void ChargementToutDonnee()
        {
            GetAllCmbBX();
            
        }

        public void GetAllCmbBX()
        {
            List<String> catégorie = new() { "Produit", "Electronique", "Bijoux","Vêtement","Accessoires de mode","Véhicules","Produits alimentaire","Maison et Jardin","Santé","Animaux"};
            
            cmbAddCategorie.ItemsSource = catégorie;
        } 

        private string   RecuperationDuDescriptionDuListBoxDescri()
        {
            if (ListBoxDescri != null && ListBoxDescri.Items.Count > 0)
            {
                StringBuilder items = new StringBuilder();
                foreach (var item in ListBoxDescri.Items)
                {
                    var listBoxItemData = item as ListBoxAddDescri;
                    if (listBoxItemData != null)
                    {
                        items.AppendLine(listBoxItemData.descri + "/");
                    }
                }
                return items.ToString();
            }
            return "Le ListBox est vide ou null.";

            /*
            if(ListBoxDescri.Items.Count >= 0 )
            {   
                String lesItems = String.Empty;
                int nbTableau = ListBoxDescri.Items.Count;
                for (int i = 0; i < nbTableau; i++)
                {
                    lesItems += ListBoxDescri.Items[i] + ";";
                }
                return lesItems.Substring(0, lesItems.Length - 1);

            }
            return string.Empty;*/
        }

        private async Task<ProduitModels> GetProduitByIdAsync(string productId)
        {
            using (new WaitProgressRing(progressring)) 
            { 
                try
                {
                    //mode dev
                    //var httpService = new HttpService("https://localhost:7104");
                    //mode prod
                    var httpService = new HttpService("http://favsite.runasp.net/");
                    var produit = await httpService.GetProduitAsync(productId);

                    if (produit != null)
                    {
                        MessageBox.Show("nom produit : " + produit.Nom_produit + ".Voila");
                        return produit;
                    }
                    else
                    {
                        MessageBox.Show("Le produit avec cet ID n'a pas été trouvé.");
                        return null;
                    }
                }
                catch (HttpRequestException httpEx)
                {
                    MessageBox.Show($"Une erreur s'est produite lors de la récupération du produit : {httpEx.Message}");
                    return null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Une erreur inattendue s'est produite : {ex.Message}");
                    return null;
                }
            }
        }
        private ProduitModels CreateProduit()
        {
            ProduitModels produit = new();
            int nbTotalProd = int.Parse(txtNbTotalProd.Text);
            produit.Id_vendeur = idVendeur;
            produit.Id_utilisateur = idUser;
            produit.Nom_produit = txtNomProduit.Text;
            produit.Marque = txtMarque.Text;
            produit.Description_produit = txtDescription.Text;
            produit.Nb_total_prod = nbTotalProd;
            produit.Nb_produit_reste = 0;
            produit.Prix = double.Parse(txtPrix.Text);
            produit.Autre_description = RecuperationDuDescriptionDuListBoxDescri();
            produit.Categorie = cmbAddCategorie.SelectedItem as string;
            produit.Type = cmbAddType.SelectedItem as string;
            produit.Promotion = Promo.IsChecked == true;
            produit.Prix_promo = produit.Promotion ? double.Parse(txtPromo.Text) : 0;
            if(produit.Categorie == null)
            {
                produit.Categorie = "produit";
            }
            if(produit.Type == null)
            {
                produit.Type = "produits";
            }
            if (Promo.IsChecked == true)
            {
                double prixTemps = 0;
                prixTemps = produit.Prix;
                double pourcentage = ((produit.Prix * produit.Prix_promo) / 100);
                produit.Prix = prixTemps - pourcentage;
                produit.Val_prix_promo = prixTemps;
            }

            return produit;
        }

        private Image_produitModels CreateImages()
        {
            Image_produitModels images = new();

            if (ImgCouv != null && Img1 != null && Img2 != null && Img3 != null && Img4 != null)
            {
                images.Image_couv = filePathCouv != null ? File.ReadAllBytes(filePathCouv) : null;
                images.Image_1 = filePath1 != null ? File.ReadAllBytes(filePath1) : null;
                images.Image_2 = filePath2 != null ? File.ReadAllBytes(filePath2) : null;
                images.Image_3 = filePath3 != null ? File.ReadAllBytes(filePath3) : null;
                images.Image_4 = filePath4 != null ? File.ReadAllBytes(filePath4) : null;
            }

            return images;
        }

        private async void ProduitToAddInBd()
        {
            using (new WaitProgressRing(progressring))
            {
                try
                {
                    var produits = new ProduitModels();
                    var images = new Image_produitModels();
                    produits = CreateProduit();
                    images = CreateImages();
                    var httpService = new HttpService("https://localhost:7104/api/ControllerAPI/");
                    //var httpService = new HttpService("http://favsite.runasp.net/api/ControllerAPI/");
                    var response_1 = await httpService.SendPostRequestAsync("PostAddNewProduct", produits);

                    var httpService_2 = new HttpService("https://localhost:7104/api/ControllerAPI/");
                    var response_2 = await httpService_2.SendPostRequestAsync("PostAddNewImage", images);

                    if (response_1.IsSuccessStatusCode && response_2.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Votre produit a été publiée avec succes", "Publication éfféctuer", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        if (!response_2.IsSuccessStatusCode)
                        {
                            MessageBox.Show("Error : ici respons 2 = :" + response_1.StatusCode);
                        }
                        if (!response_1.IsSuccessStatusCode)
                        {
                            MessageBox.Show("Error : ici: response 1 = " + response_2.StatusCode);
                        }
                    }

                    listeDescri = null;
                }
                catch (HttpRequestException httpEx)
                {
                    MessageBox.Show("An error occurred while sending the request: " + httpEx.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An unexpected error occurred: " + ex.Message);
                }
            }
        }
        #endregion

        //Fin du methode

        private void CnvsImgDrapDropCouv_Drop(object sender, DragEventArgs e)
        {
            var filename = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
            var fileInfo = new FileInfo(filename);
            if (fileInfo.Exists)
            {
                filePathCouv = filename;
                var isFilterOk = (fileInfo.Extension == ".jpg") || (fileInfo.Extension == ".png") || (fileInfo.Extension == ".gif");
                if (isFilterOk)
                {
                    var img = new BitmapImage(new Uri(filename));
                    ImgCouv.Source = img;
                }
            }
        }

        private void CnvsImgDrapDrop1_Drop(object sender, DragEventArgs e)
        {
            var filename = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
            var fileInfo = new FileInfo(filename);
            if (fileInfo.Exists)
            {
                filePath1 = filename;
                var isFilterOk = (fileInfo.Extension == ".jpg") || (fileInfo.Extension == ".png") || (fileInfo.Extension == ".gif");
                if (isFilterOk)
                {
                    var img = new BitmapImage(new Uri(filename));
                    Img1.Source = img;
                }
            }
        }

        private void CnvsImgDrapDrop3_Drop(object sender, DragEventArgs e)
        {
            var filename = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
            var fileInfo = new FileInfo(filename);
            if (fileInfo.Exists)
            {
                filePath3 = filename;
                var isFilterOk = (fileInfo.Extension == ".jpg") || (fileInfo.Extension == ".png") || (fileInfo.Extension == ".gif");
                if (isFilterOk)
                {
                    var img = new BitmapImage(new Uri(filename));
                    Img3.Source = img;
                }
            }
        }

        private void CnvsImgDrapDrop2_Drop(object sender, DragEventArgs e)
        {
            var filename = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
            var fileInfo = new FileInfo(filename);
            if (fileInfo.Exists)
            {
                filePath2 = filename;
                var isFilterOk = (fileInfo.Extension == ".jpg") || (fileInfo.Extension == ".png") || (fileInfo.Extension == ".gif");
                if (isFilterOk)
                {
                    var img = new BitmapImage(new Uri(filename));
                    Img2.Source = img;
                }
            }
        }

        private void CnvsImgDrapDrop4_Drop(object sender, DragEventArgs e)
        {
            var filename = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
            var fileInfo = new FileInfo(filename);
            if (fileInfo.Exists)
            {
                filePath4 = filename;
                var isFilterOk = (fileInfo.Extension == ".jpg") || (fileInfo.Extension == ".png") || (fileInfo.Extension == ".gif");
                if (isFilterOk)
                {
                    var img = new BitmapImage(new Uri(filename));
                    Img4.Source = img;
                }
            }
        }

        private void Promo_Checked(object sender, RoutedEventArgs e)
        {
            if (Promo.IsChecked == true)
            {
                txtPromo.IsEnabled = true;
            }
            if (Promo.IsChecked == false)
            {
                txtPromo.IsEnabled = false;
            }
        }

        private void Promo_Click(object sender, RoutedEventArgs e)
        {
            if (Promo.IsChecked == false)
            {
                txtPromo.Text = String.Empty;
                txtPromo.IsEnabled = false;
            }
            if (Promo.IsChecked == true)
            {
                Promo.Content = "Désactiver le promotion";
            }
            else
            {
                Promo.Content = "Activer le mode promotion";
            }
        }

        private void txtPrix_TextChanged(object sender, TextChangedEventArgs e)
        {
            double prix = 0;
            if (!double.TryParse(txtPrix.Text, out prix))
            {
                txtPrix.Text = String.Empty;
            }
        }

        private void txtPromo_TextChanged(object sender, TextChangedEventArgs e)
        {
            double prix = 0;
            if (!double.TryParse(txtPromo.Text, out prix))
            {
                txtPromo.Text = String.Empty;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(txtDescriTitre.Text != String.Empty || txtDescriValeur.Text != String.Empty) {
                ListBoxDescri.ItemsSource = null;
                string description = txtDescriTitre.Text + " : " + txtDescriValeur.Text;
                if(listeDescri == null)
                {
                    listeDescri = new List<ListBoxAddDescri>();
                }
                listeDescri.Add(new ListBoxAddDescri() { descri = description});
                ListBoxDescri.ItemsSource = listeDescri;
                description = String.Empty;
            }
            txtDescriTitre.Text = String.Empty;
            txtDescriValeur.Text = String.Empty;
        }

        private void SupDescri_Click(object sender, RoutedEventArgs e)
        {
            if (ListBoxDescri.SelectedIndex > -1)
            {
                var selCity = ListBoxDescri.SelectedItem as ListBoxAddDescri;
                if (selCity != null)
                {
                    var result = MessageBox.Show($"Voulez vous supprimer la déscription {selCity.descri} ?", "Supréssion", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        listeDescri.Remove(selCity);
                        ListBoxDescri.ItemsSource = null;
                        ListBoxDescri.ItemsSource = listeDescri;
                    }
                }
            }
        }

        

        private void btnPublier_Click(object sender, RoutedEventArgs e)
        {
            
            ProduitToAddInBd();
            ClearAllControll();
        }

        private  async void Button_Click_1(object sender, RoutedEventArgs e)
        {
           await GetProduitByIdAsync("87205280-6170-4048-F992-08DC9C168767");
        }

        private void cmbAddCategorie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<String> typeElectronique = new() { "Ordinateur portable", "Ordinateur bureau", "Moniteur", "Clavier et Souris", "Imprimente et scanner", "Disque dur et Stockage", "Accessoires pour ordinateur",
                                                     "Téléviseurs","Projecteur","Lecteurs DVD et Blu-ray","Support TV et accessoires","Casque et écouteur","Platine  vinyles et tourne disque","Radio et tuner",
                                                   "Appareil photo numérique","Eclairage et studion","Microphone","Console de jeux video","Accessoire de jeux","Smartphone","Accessoire pour téléphone","Drone","Appareil de santé et fitness"};

            List<String> typeBijoux = new() { "Produit", "Boucle", "Anneau de couple", "Collier", "Bracelets" };
            List<String> typeVoiture = new() { "Produit", "Plaisire", "Moto", "bicyclette", "Quoid", "Camion", "Camionette", "4x4", "tri_cycle" };
            List<String> typeVetement = new() { "Produit", "Tee-shirt", "Chemise", "Pantalon", "Pantalon Jean", "Jean", "Short", "Short jean", "Robe", "Robe longue", "Jupe", "Blouse", "Pull", "Sweat-shirt" ,
                                                "Veste","Costume","Débaedeur","Cardigan","Pyjama","Tee-shirt de sport","Maillot de bain","Survêtement","Salopette","Chemisier","Legging"};
            List<String> typeAccessoirDemode = new() { "Produit", "Chapeaux", "Ceinture", "Echarpes et foulard", "Gants", "Sacs à main et pochettes", "Lunettes de soleil", "Montres", "Bonnets et bandeaux",
                                                        "Chaussettes","Collants","Broches et pin's","Epingle a cheveux","Serre-tête","Papapluies","Portefeuilles","Sac à dos",};
            List<string> typeProduitAlimentaire = new() { "Produit","Fruit","Légumes","Viande","Poissons","Céréales et grains","Pâtes et nouilles","Huille et graisses","Produit surgelés",
            "produit en conserve","Produits sec","Condiments et Sauce","Herbes et épices","Boisson","Confiseries"};
            List<String> typeLivre = new() { "Physique", "Numérique" };
            List<String> typeSante = new() { "Produit","Médicament en vente libre","Produit de premier soin","Suppléments nutritionnels","Produit d'hygiène personnelle","Appareil médicaux",
                                            "Produit de soin de la peau","Produit de soin capillaires","Produit de soins bucco-dentaire","Produit pour les allergies","Désinfection","Produit de soins des yeux"};
            List<String> typeMaisonJardin = new() { "Produit", "Mobilier", "Textiles de maison", "Décoration intérieur", "Ustensiles de cuisine", "Vase", "Fleure" };
            List<String> typeAnimaux = new() { "Produit", "Lits et couchage", "Jouets", "Vêtements et accessoires", "Hygiène et toilettage", "transport et voyage", "Equipements d'entraînement", "Nourriture" };
            List<String> typeProd = new() { "Produit" };
            String selItem = cmbAddCategorie.SelectedItem as string;
            switch (selItem)
            {
                case "Produit":
                    cmbAddType.ItemsSource = typeProd;
                    break;
                case "Electronique":
                    cmbAddType.ItemsSource = typeElectronique;
                    break;
                case "Bijoux":
                    cmbAddType.ItemsSource = typeBijoux;
                    break;
                case "Vêtement":
                    cmbAddType.ItemsSource = typeVetement;
                    break;
                case "Accessoires de mode":
                    cmbAddType.ItemsSource = typeAccessoirDemode;
                    break;
                case "Véhicules":
                    cmbAddType.ItemsSource = typeVoiture;
                    break;
                case "Produits alimentaire":
                    cmbAddType.ItemsSource = typeProduitAlimentaire;
                    break;
                case "Santé":
                    cmbAddType.ItemsSource = typeSante;
                    break;
                case "Animaux":
                    cmbAddType.ItemsSource = typeAnimaux;
                    break;
                case "Maison et Jardin":
                    cmbAddType.ItemsSource = typeMaisonJardin;
                    break;
                default:
                    break;
            }
        }
    }
}
