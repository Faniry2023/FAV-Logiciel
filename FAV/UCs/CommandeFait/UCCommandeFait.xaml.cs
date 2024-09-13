using FAV.Helper;
using FAV.Models;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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

namespace FAV.UCs.CommandeFait
{
    /// <summary>
    /// Logique d'interaction pour UCCommandeFait.xaml
    /// </summary>
    public partial class UCCommandeFait : UserControl
    {
        private static string? idVendeur;
        private static CommandeModels? commandeLivre;
        private static DonneConnectedModel? donneConnectedModel;
        public UCCommandeFait(DonneConnectedModel conP)
        {
            InitializeComponent();
            donneConnectedModel = conP;
            InitializeAsync();
            idVendeur = conP.IdVendeurConnected.ToUpper();
            InitializCommandeUserToListBox();
        }
        private async void InitializeAsync()
        {
            await GoogleMapBrowser.EnsureCoreWebView2Async(null);
            LoadGoogleMap();
        }

        private void LoadGoogleMap()
        {
            string mapPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "map.html");
            if (File.Exists(mapPath))
            {
                GoogleMapBrowser.Source = new Uri(mapPath);
            }
            else
            {
                MessageBox.Show($"Fichier introuvable : {mapPath}");
            }
        }

        private void GoogleMapBrowser_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
                GoogleMapBrowser.NavigationCompleted += GoogleMapBrowser_NavigationCompleted;
            }
            else
            {
                MessageBox.Show($"Erreur d'initialisation de WebView2 : {e.InitializationException}");
            }
        }

        private void GoogleMapBrowser_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
                // Ensure that the map is centered after loading
                SetMapCenter(-19.8667, 47.0333); // Coordinates for Antsirabe, Madagascar
            }
            else
            {
                MessageBox.Show($"Erreur de chargement : {e.WebErrorStatus}");
            }
        }

        public void SetMapCenter(double latitude, double longitude)
        {
            try
            {
                // Format les coordonnées en utilisant des nombres à virgule flottante avec une précision
                var formattedLatitude = latitude.ToString("F6", CultureInfo.InvariantCulture);
                var formattedLongitude = longitude.ToString("F6", CultureInfo.InvariantCulture);

                // Exécute le script JavaScript avec les coordonnées formatées
                GoogleMapBrowser.ExecuteScriptAsync($"setMapCenter({formattedLatitude}, {formattedLongitude});");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la définition du centre de la carte : {ex.Message}");
            }
        }
        public async void InitializCommandeUserToListBox()
        {
            using (new WaitProgressRing(progressRing))
            {
                List<UserCommandes> listUserCommande = new();
                HttpService httpService = new("https://localhost:7104");
                var toutUser = await httpService.GetAllUserAsync();
                var mesCommande = donneConnectedModel.toutCommande;
                List<UtilisateurModels> acheteurs = new();
                foreach (var mesC in mesCommande)
                {
                    if (mesC.is_livrer)
                    {
                        if (acheteurs.Count > 0)
                        {
                            foreach (var ach in acheteurs)
                            {
                                if (ach.Id_ut.ToString().ToUpper().Equals(mesC.Id_acheteur.ToUpper()))
                                {
                                    UserCommandes updates = new();
                                    updates = listUserCommande.FirstOrDefault(c => c.utilisateurCommande.Id_ut.ToString().ToUpper().Equals(ach.Id_ut.ToString().ToUpper()));
                                    listUserCommande.Remove(updates);
                                    var lesidpro = mesC.lesIdProduit.Substring(0, mesC.lesIdProduit.Length - 1);
                                    var lesquant = mesC.quantite.Substring(0, mesC.quantite.Length - 1);
                                    string[] quant = lesquant.Split(':');
                                    string[] prod = lesidpro.Split(':');
                                    for (int i = 0; i < prod.Length; i++)
                                    {
                                        ProduitModels pr = new();
                                        Image_produitModels im = new();
                                        pr = donneConnectedModel.produitModels.FirstOrDefault(p => p.Id_produit.ToString().ToUpper().Equals(prod[i].ToUpper()));
                                        if (pr != null)
                                        {
                                            im = donneConnectedModel.imageProduitModels.FirstOrDefault(p => p.Id_produit.ToUpper().Equals(prod[i].ToUpper()));
                                            updates.produits.Add(pr);
                                            updates.quantite.Add(int.Parse(quant[i]));
                                            updates.imagesDesProduit.Add(AffichageImage.GetBitmapImage(im.Image_couv));
                                        }
                                    }
                                    listUserCommande.Add(updates);
                                    break;
                                }
                                if (!ach.Id_ut.ToString().ToUpper().Equals(mesC.Id_acheteur.ToUpper()))
                                {
                                    UserCommandes com = new();
                                    UtilisateurModels utilisateur = new();
                                    com.commandeUser = mesC;
                                    utilisateur = toutUser.FirstOrDefault(u => u.Id_ut.ToString().ToUpper().Equals(mesC.Id_acheteur.ToUpper()));
                                    if (acheteurs.Contains(utilisateur))
                                    {
                                        UserCommandes updates = new();
                                        updates = listUserCommande.FirstOrDefault(c => c.utilisateurCommande.Id_ut.ToString().ToUpper().Equals(utilisateur.Id_ut.ToString().ToUpper()));
                                        listUserCommande.Remove(updates);
                                        var leidpro = mesC.lesIdProduit.Substring(0, mesC.lesIdProduit.Length - 1);
                                        var lequant = mesC.quantite.Substring(0, mesC.quantite.Length - 1);
                                        string[] quante = lequant.Split(':');
                                        string[] prode = leidpro.Split(':');
                                        for (int i = 0; i < prode.Length; i++)
                                        {
                                            ProduitModels pr = new();
                                            Image_produitModels im = new();
                                            pr = donneConnectedModel.produitModels.FirstOrDefault(p => p.Id_produit.ToString().ToUpper().Equals(prode[i].ToUpper()));
                                            if (pr != null)
                                            {
                                                im = donneConnectedModel.imageProduitModels.FirstOrDefault(p => p.Id_produit.ToUpper().Equals(prode[i].ToUpper()));
                                                updates.produits.Add(pr);
                                                updates.quantite.Add(int.Parse(quante[i]));
                                                updates.imagesDesProduit.Add(AffichageImage.GetBitmapImage(im.Image_couv));
                                            }

                                        }
                                        listUserCommande.Add(updates);
                                        break;
                                    }
                                    com.utilisateurCommande = utilisateur;
                                    acheteurs.Add(utilisateur);
                                    com.imageUser = AffichageImage.GetBitmapImage(utilisateur.Photo);
                                    var lesidpro = mesC.lesIdProduit.Substring(0, mesC.lesIdProduit.Length - 1);
                                    var lesquant = mesC.quantite.Substring(0, mesC.quantite.Length - 1);
                                    string[] quant = lesquant.Split(':');
                                    string[] prod = lesidpro.Split(':');
                                    for (int i = 0; i < prod.Length; i++)
                                    {
                                        ProduitModels pr = new();
                                        Image_produitModels im = new();
                                        pr = donneConnectedModel.produitModels.FirstOrDefault(p => p.Id_produit.ToString().ToUpper().Equals(prod[i].ToUpper()));
                                        if (pr != null)
                                        {
                                            im = donneConnectedModel.imageProduitModels.FirstOrDefault(p => p.Id_produit.ToUpper().Equals(prod[i].ToUpper()));
                                            com.produits.Add(pr);
                                            com.quantite.Add(int.Parse(quant[i]));
                                            com.imagesDesProduit.Add(AffichageImage.GetBitmapImage(im.Image_couv));
                                        }

                                    }
                                    listUserCommande.Add(com);
                                    break;
                                }
                            }
                        }
                        if (acheteurs.Count == 0)
                        {
                            UserCommandes com = new();
                            UtilisateurModels utilisateur = new();
                            com.commandeUser = mesC;
                            utilisateur = toutUser.FirstOrDefault(u => u.Id_ut.ToString().ToUpper().Equals(mesC.Id_acheteur.ToUpper()));
                            com.utilisateurCommande = utilisateur;
                            acheteurs.Add(utilisateur);
                            com.imageUser = AffichageImage.GetBitmapImage(utilisateur.Photo);
                            var lesidpro = mesC.lesIdProduit.Substring(0, mesC.lesIdProduit.Length - 1);
                            var lesquant = mesC.quantite.Substring(0, mesC.quantite.Length - 1);
                            string[] quant = lesquant.Split(':');
                            string[] prod = lesidpro.Split(':');
                            for (int i = 0; i < prod.Length; i++)
                            {
                                ProduitModels pr = new();
                                Image_produitModels im = new();
                                pr = donneConnectedModel.produitModels.FirstOrDefault(p => p.Id_produit.ToString().ToUpper().Equals(prod[i].ToUpper()));
                                if (pr != null)
                                {
                                    im = donneConnectedModel.imageProduitModels.FirstOrDefault(p => p.Id_produit.ToUpper().Equals(prod[i].ToUpper()));
                                    com.produits.Add(pr);
                                    com.quantite.Add(int.Parse(quant[i]));
                                    com.imagesDesProduit.Add(AffichageImage.GetBitmapImage(im.Image_couv));
                                }
                            }
                            listUserCommande.Add(com);
                        }
                    }
                }
                ListBoxCommande.ItemsSource = listUserCommande;
            }
        }

        private void ListBoxCommande_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (new WaitProgressRing(progresRingList))
            {
                var selUseCommande = ListBoxCommande.SelectedItem as UserCommandes;
                if (selUseCommande != null)
                {
                    commandeLivre = selUseCommande.commandeUser;
                    lblNom.Content = selUseCommande.utilisateurCommande.Nom_ut;
                    lblPrenom.Content = selUseCommande.utilisateurCommande.Prenom_ut;
                    lblContact.Content = selUseCommande.utilisateurCommande.Contact_ut;
                    SetMapCenter(selUseCommande.commandeUser.latitude, selUseCommande.commandeUser.longitude);
                    imageUser.Source = selUseCommande.imageUser;
                    List<ListProdCom> listProdComs = new();
                    for (int i = 0; i < selUseCommande.produits.Count; i++)
                    {
                        ProduitModels p = new();
                        BitmapImage im = new();
                        int quantite = 0;
                        ListProdCom l = new();
                        p = selUseCommande.produits[i];
                        im = selUseCommande.imagesDesProduit[i];
                        quantite = selUseCommande.quantite[i];
                        l.lesProduits = p; l.lesImages = im; l.quantite = quantite; listProdComs.Add(l);
                    }
                    ListBoxProduit.ItemsSource = listProdComs;
                }
            }
        }

        private async void Actualiser_Click(object sender, RoutedEventArgs e)
        {
            using (new WaitProgressRing(progressRing))
            {
                try
                {
                    HttpService httpService = new("https://localhost:7104");
                    var toutCommande = await httpService.GetAllCommandesAsync();
                    donneConnectedModel.toutCommande = toutCommande.Where(c => c.Id_vendeur.ToUpper().Equals(donneConnectedModel.IdVendeurConnected.ToUpper())).ToList();
                    InitializCommandeUserToListBox();
                    FenetrePrincipale refreshFenetre = new FenetrePrincipale(donneConnectedModel);
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

        private async void AnnulerCommande_Click(object sender, RoutedEventArgs e)
        {
            using (new WaitProgressRing(progressRing))
            {
                try
                {
                    var commandes = commandeLivre;
                    var httpService = new HttpService("https://localhost:7104/api/ControllerAPI/");
                    var response = await httpService.SendPostRequestAsync("AnnulerCommande", commandes);

                    if (response.IsSuccessStatusCode)
                    {
                        var httpService2 = new HttpService("https://localhost:7104");
                        var toutCommande = await httpService2.GetAllCommandesAsync();
                        donneConnectedModel.toutCommande = toutCommande
                            .Where(c => c.Id_vendeur.ToUpper().Equals(donneConnectedModel.IdVendeurConnected.ToUpper()))
                            .ToList();

                        InitializCommandeUserToListBox();

                        FenetrePrincipale refreshFenetre = new FenetrePrincipale(donneConnectedModel);
                        MessageBox.Show($"Produit livrée", "Publication éfféctuer", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Error : ici response 2 = :" + response.StatusCode);
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
        }
    }
}
