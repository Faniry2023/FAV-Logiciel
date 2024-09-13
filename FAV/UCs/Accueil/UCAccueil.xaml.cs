using FAV.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using System.Net.Http;
using System.Windows.Shapes;

namespace FAV.UCs.Accueil
{
    /// <summary>
    /// Logique d'interaction pour UCAccueil.xaml
    /// </summary>
    public partial class UCAccueil : UserControl
    {
        private DonneConnectedModel? connectedModel;
        private static string? idVendeur;
        public UCAccueil(DonneConnectedModel? connectedModel)
        {
            InitializeComponent();
            this.connectedModel = connectedModel;
            AfficheProd();
            idVendeur = connectedModel.IdVendeurConnected.ToUpper();
            
        }
        public async void AfficheProd()
        {
            try
            {
                using (new WaitProgressRing(progressRing))
                {
                    GetPersonToListBoxPerson();
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
        public async void GetPersonToListBoxPerson()
        {
            if(connectedModel != null)
            {
                using(new WaitProgressRing(progressRing))
                {

                    var allProd = connectedModel.produitModels;
                    var allImage = connectedModel.imageProduitModels;
                    List<ProdAndImgBitMap> prodAndImgBitMaps = new();
                    foreach(var item in allProd)
                    {
                        ProdAndImgBitMap prodImg = new();
                        var img = connectedModel.imageProduitModels.FirstOrDefault(i => i.Id_produit.ToUpper().Equals(item.Id_produit.ToString().ToUpper()));
                        prodImg.imageCouveBit = GetBitmapImage(img.Image_couv);
                        prodImg.image1Bit = GetBitmapImage(img.Image_1);
                        prodImg.image2Bit = GetBitmapImage(img.Image_2);
                        prodImg.image3Bit = GetBitmapImage(img.Image_3);
                        prodImg.image4Bit = GetBitmapImage(img.Image_4);
                        prodImg.produitModels = item;
                        prodAndImgBitMaps.Add(prodImg);
                    }
                    lsBxPerson.ItemsSource = prodAndImgBitMaps;
                }
            }
        }
        /*public BitmapImage bitmapImage
        {
            get { return GetBitmapImage(PPhoto); }
            set
            {
                if (value is not null && value.UriSource is not null)
                {
                    PPhoto = File.ReadAllBytes(value.UriSource.OriginalString);
                }
            }
        }
        */
        private BitmapImage GetBitmapImage(byte[]? iPhoto)
        {
            var img = new BitmapImage();
            if (iPhoto is not null || iPhoto?.Length is not 0)
            {
                if (iPhoto != null)
                {
                    using (var stream = new MemoryStream(iPhoto))
                    {
                        stream.Position = 0;
                        img.BeginInit();
                        img.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                        img.CacheOption = BitmapCacheOption.OnLoad;
                        img.StreamSource = stream;
                        img.EndInit();
                    }
                }
            }
            return img;
        }

        private void RpBtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            if (lsBxPerson.Items.Count > 0)
            {
                if (lsBxPerson.SelectedIndex > 0)
                {
                    lsBxPerson.SelectedIndex--;
                    //ce code dit qu'il doit y aller chez le ittem seléctionné en changeant l'affichage si c'est pas visible'
                    lsBxPerson.ScrollIntoView(lsBxPerson.SelectedItem);
                }
                else
                {
                    lsBxPerson.SelectedIndex = lsBxPerson.Items.Count - 1;
                    lsBxPerson.ScrollIntoView(lsBxPerson.SelectedItem);
                }
            }
        }

        private void btnGoForward_Click(object sender, RoutedEventArgs e)
        {
            if (lsBxPerson.Items.Count > 0)
            {
                if (lsBxPerson.SelectedIndex < lsBxPerson.Items.Count - 1)
                {
                    lsBxPerson.SelectedIndex++;
                    lsBxPerson.ScrollIntoView(lsBxPerson.SelectedItem);
                }
                else
                {
                    lsBxPerson.SelectedIndex = 0;
                    lsBxPerson.ScrollIntoView(lsBxPerson.SelectedItem);
                }
            }
        }

        private void lsBxPerson_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if(lsBxPerson.SelectedItem != null)
                {
                    if(lsBxPerson.SelectedItem is ProdAndImgBitMap selProd)
                    {
                        GridProdPlace.Children.Clear();
                        GridProdPlace.Children.Add(new UCHomee(selProd));
                    }
                }
            }catch(Exception ex) { }
        }

        private async void Actualiser_Click(object sender, RoutedEventArgs e)
        {
            using(new WaitProgressRing(progressRing))
            {
                try
                {
                    HttpService httpService = new("https://localhost:7104");
                    var toutProduit = await httpService.GetAllProduitAsync();
                    var touImage = await httpService.GetAllImageAsync();
                    var mesProduit = toutProduit.Where(p => p.Id_vendeur.ToUpper().Equals(idVendeur)).ToList();
                    if(mesProduit.Count > 0 && mesProduit != null && touImage.Count > 0 && touImage != null)
                    {
                        connectedModel.produitModels = new();
                        connectedModel.produitModels = mesProduit;
                        connectedModel.imageProduitModels = new();
                        foreach(var item in mesProduit)
                        {
                            connectedModel.imageProduitModels.Add(touImage.FirstOrDefault(i => i.Id_produit.ToUpper().Equals(item.Id_produit.ToString().ToUpper())));
                        }
                    }
                    GetPersonToListBoxPerson();
                    FenetrePrincipale refreshFenetre = new FenetrePrincipale(connectedModel);

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
    }
}
