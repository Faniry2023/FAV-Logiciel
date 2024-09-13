using FAV.Helper;
using FAV.Models;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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

namespace FAV.UCs.ModifierProduit
{
    /// <summary>
    /// Logique d'interaction pour UCModProduit.xaml
    /// </summary>
    public partial class UCModProduit : UserControl
    {
        private static DonneConnectedModel? dataConnectedModel;
        private static ProduitModels? produit;
        public UCModProduit(DonneConnectedModel connectedModel)
        {
            InitializeComponent();
            dataConnectedModel = connectedModel;
            InitializeListBoxProduit();
        }
        public void InitializeListBoxProduit()
        {
            
            using (new WaitProgressRing(progressRing))
            {
                if(dataConnectedModel.produitModels != null)
                {
                    List<HelperModel> listHerperModel = new();
                    var mesProduit = dataConnectedModel.produitModels;
                    var mesImage = dataConnectedModel.imageProduitModels;
                    foreach(var item in mesProduit)
                    {
                        if(item != null)
                        {
                            HelperModel h = new();
                            h.Produit = item;
                            var image = mesImage.FirstOrDefault(i => i.Id_produit.ToUpper().Equals(item.Id_produit.ToString().ToUpper()));
                            h.Image = image;
                            listHerperModel.Add(h);
                        }
                    }
                    ListBoxProduit.ItemsSource = listHerperModel;
                }
                
            }
        }

        private void ListBoxProduit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selPro = ListBoxProduit.SelectedItem as HelperModel;
            if(selPro != null )
            {
                produit = new();
                produit = selPro.Produit;
                imgCouv.Source = AffichageImage.GetBitmapImage(selPro.Image.Image_couv);
                lblNom.Content = selPro.Produit.Nom_produit;
                lblMarque.Content = selPro.Produit.Marque;
                lblPrix.Content = selPro.Produit.Prix;
                txtDescriValeur.Text = selPro.Produit.Description_produit;
                txtPromo.IsEnabled = false;
                if (selPro.Produit.Promotion)
                {
                    txtPromo.IsEnabled = true;
                    txtPromo.Text = selPro.Produit.Prix_promo.ToString();
                    Promo.IsChecked = true;
                    txtNbTotalProd.Text = selPro.Produit.Nb_total_prod.ToString();
                }
            }
        }

        private async void btnModifier_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                produit.Nb_total_prod = int.Parse(txtNbTotalProd.Text);
                if (Promo.IsChecked == true)
                {
                    produit.Promotion = true;
                    produit.Prix_promo = int.Parse(txtPromo.Text);
                }
                var httpService = new HttpService("https://localhost:7104/api/ControllerAPI/");
                //var httpService = new HttpService("http://favsite.runasp.net/api/ControllerAPI/");
                var response_1 = await httpService.SendPostRequestAsync("PostModNewProduct", produit);


                if (response_1.IsSuccessStatusCode)
                {
                    MessageBox.Show("Votre produit a été modifié avec succes", "Publication éfféctuer", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    if (!response_1.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Error : ici: response 1 = " + response_1.StatusCode);
                    }
                }

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

        private void Promo_Checked(object sender, RoutedEventArgs e)
        {
            if(Promo.IsChecked == true)
            {
                produit.Promotion = true;
            }
        }

        private void Promo_Click(object sender, RoutedEventArgs e)
        {
            if(Promo.IsChecked == false)
            {
                txtPromo.Text = string.Empty;
                txtPromo.IsEnabled = false;
            }
        }
    }
}
