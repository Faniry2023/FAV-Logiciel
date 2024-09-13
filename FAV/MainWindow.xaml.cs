using FAV.Helper;
using FAV.Models;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

using System.Windows.Shapes;

namespace FAV
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string? idUserConnected;
        private static string idAd;
        private static DonneConnectedModel? donneConnectedModel;
        public MainWindow(string? idUser)
        {
            InitializeComponent();
            idAd = idUser.ToUpper();
            ChargementToutDonner();
        }
        public async void ChargementToutDonner()
        {
            try
            {
                using (new WaitProgressRing(progrssRing))
                {
                    HttpService httpService = new("https://localhost:7104");
                    //HttpService httpService = new("http://favsite.runasp.net/");
                    //Recuperation info personnel du vendeur
                    donneConnectedModel = new();
                    
                    var toutVendeur = await httpService.GetAllUserVendeurAsync();
                    var toutUserViaHttp = await httpService.GetAllUserAsync();
                    string idVendeurCOn = string.Empty;
                    if(toutVendeur.Count > 0 && toutVendeur != null && toutUserViaHttp.Count > 0 && toutUserViaHttp != null)
                    {
                        idUserConnected = toutUserViaHttp.FirstOrDefault(u => u.Id_ad.ToUpper().Equals(idAd)).Id_ut.ToString().ToUpper();
                        var Vende = toutVendeur.FirstOrDefault(v => v.Id_uti.ToUpper().Equals(idUserConnected));
                        donneConnectedModel.IdVendeurConnected = Vende.Id_uti_ven.ToString().ToUpper();
                        idVendeurCOn = Vende.Id_uti_ven.ToString().ToUpper();
                        donneConnectedModel.nomEntreprise = Vende.Nom_Societe;
                        donneConnectedModel.IdUserConnected = idUserConnected;
                    }

                    //recuperation de produit du vendeur
                    var produitViaHttp = await httpService.GetAllProduitAsync();
                    var mesProduits = new List<ProduitModels>();
                    if (produitViaHttp.Count > 0 && produitViaHttp != null)
                    {
                       mesProduits  = produitViaHttp.Where(p => p.Id_utilisateur.ToUpper().Equals(idUserConnected)).ToList();
                        donneConnectedModel.produitModels = mesProduits;
                    }
                    //recuperation des image 
                    var toutImageViaHttp = await httpService.GetAllImageAsync();

                    if(toutImageViaHttp.Count > 0 && toutImageViaHttp != null)
                    {
                        if(mesProduits.Count> 0 && mesProduits != null)
                        {
                            foreach (var item in mesProduits)
                            {
                                donneConnectedModel.imageProduitModels.Add(toutImageViaHttp.FirstOrDefault(i => i.Id_produit.ToUpper().Equals(item.Id_produit.ToString().ToUpper())));
                            }
                        }
                    }

                    //recuperation publicité
                    var toutPubViaHttp = await httpService.GetAllPubliciteAsync();
                    if(toutPubViaHttp.Count > 0 && toutPubViaHttp != null)
                    {
                        donneConnectedModel.publiciteModels = toutPubViaHttp.Where(u => u.Id_utilisateur.ToUpper().Equals(idUserConnected)).ToList();
                    }
                    

                    //recuperation commade
                    var toutCommandeViaHttp = await httpService.GetAllCommandesAsync();
                    if(toutCommandeViaHttp.Count > 0 && toutCommandeViaHttp != null)
                    {
                        donneConnectedModel.toutCommande = toutCommandeViaHttp.Where(c => c.Id_vendeur.ToUpper().Equals(idVendeurCOn)).ToList();
                    }

                    //recuperation statistique
                    var toutStatViaHttp = await httpService.GetAllStatMoisAsync();
                    foreach(var item in mesProduits)
                    {
                        donneConnectedModel.listStatMois.Add(toutStatViaHttp.FirstOrDefault(s => s.id_produit.ToUpper().Equals(item.Id_produit.ToString().ToUpper())));
                    }


                }
                FenetrePrincipale fenetrePrincipale = new(donneConnectedModel);
                fenetrePrincipale.Show();
                this.Close();
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
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerAsync();
        }

        private void worker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            if (progressBar.Value == 100)
            {
                
            }
        }

        private void worker_DoWork(object? sender, DoWorkEventArgs e)
        {
            for (int i = 0; i <= 100; i++)
            {
                if (sender is not null)
                {
                    (sender as BackgroundWorker).ReportProgress(i);
                    Thread.Sleep(5000);
                }
            }
        }
    }
}