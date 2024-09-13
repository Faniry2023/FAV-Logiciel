using FAV.Helper;
using FAV.UCs.Accueil;
using FAV.UCs.addProduit;
using FAV.UCs.addPublicite;
using FAV.UCs.CommandeFait;
using FAV.UCs.ModifierProduit;
using FAV.UCs.ProduitALivrer;
using FAV.UCs.Statistique;
using FAV.UCs.UCBrowser;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace FAV
{
    /// <summary>
    /// Logique d'interaction pour FenetrePrincipale.xaml
    /// </summary>
    public partial class FenetrePrincipale : MetroWindow
    {
        private UCAccueil? uCAccueil;
        private DonneConnectedModel? donneConnectedModel;
        private string? nomEntreprise;
        public FenetrePrincipale(DonneConnectedModel donneConnectedModel)
        {
            InitializeComponent();
            this.donneConnectedModel = donneConnectedModel;
            this.nomEntreprise = donneConnectedModel.nomEntreprise;
            lblName.Content = nomEntreprise;
        }

        //METHODE

        private void OpenCloseFlayout(int iFlayoutIndex)
        {

            try
            {
                var flayout = this.Flyouts.Items[iFlayoutIndex] as Flyout;
                if (flayout is not null)
                {
                    flayout.IsOpen = !flayout.IsOpen;
                }
            }
            catch (System.ArgumentOutOfRangeException ex) { }
        }

        //cette mmethode a pour but de deplacer le border a gauche de chaque item dans le menu
        private void MoveMenuCurser(int iListViewItemIndex)
        {
            if (iListViewItemIndex >= 0)
            {
                BorderCursor.Margin = new Thickness(0, (ListViewItemAccueil.Height) * iListViewItemIndex + 60, 0, 0);
            }
        }

        private void ShowUcOnUCPlaceHolderGrid(UserControl userControl)
        {
            gridContent.Children.Clear();
            gridContent.Children.Add(userControl);
        }
        //FIN METHODE

        private void Grid_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        private async void BtnWindowMinimize_Click(object sender, RoutedEventArgs e)
        {
            //var Btn = sender as Button;
            if (sender is Button sendeBtn)
            {
                if (sendeBtn is not null)
                {
                    if (sendeBtn.Name == BtnWindowMinimize.Name)
                    {
                        this.WindowState = WindowState.Minimized;
                    }
                    else
                    {
                        var dialoSetting = new MetroDialogSettings()
                        {
                            AffirmativeButtonText = "Oui",
                            NegativeButtonText = "Non",
                            AnimateShow = true,
                            AnimateHide = true,
                        };
                        var result = await this.ShowMessageAsync("Quitter FAV",
                            "Voulez-vous vraiment quitter FAV",
                            MessageDialogStyle.AffirmativeAndNegative, dialoSetting);
                        if (result == MessageDialogResult.Affirmative)
                        {
                            Application.Current.Shutdown();
                        }
                    }
                }
            }
        }
        
        private void TglBtnMenuOpenClose_Click(object sender, RoutedEventArgs e)
        {
            OpenCloseFlayout(0);
        }

        private void MainMenuListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = MainMenuListView.SelectedIndex;
            MoveMenuCurser(index);
            TglBtnMenuOpenClose.IsChecked = false;
            OpenCloseFlayout(0);
            switch (index)
            {
                case 0:
                    gridContent.Children.Clear();
                    gridContent.Children.Add(new UCAccueil(donneConnectedModel));
                    break;
                case 1:
                    gridContent.Children.Clear();
                    gridContent.Children.Add(new UCModProduit(donneConnectedModel));
                    break;
                case 2:
                    gridContent.Children.Clear();
                    gridContent.Children.Add(new UCAddPub(donneConnectedModel.IdUserConnected));
                    break;
                case 3:
                    gridContent.Children.Clear();
                    gridContent.Children.Add(new UCAddProduit(donneConnectedModel.IdUserConnected.ToUpper(), donneConnectedModel.IdVendeurConnected.ToUpper()));
                    break;
                case 4:
                    gridContent.Children.Clear();
                    gridContent.Children.Add(new UCCommandeFait(donneConnectedModel));
                    break;
                case 5:
                    gridContent.Children.Clear();
                    gridContent.Children.Add(new UCCommande(donneConnectedModel));
                    break;
                case 6:
                    gridContent.Children.Clear();
                    gridContent.Children.Add(new UCStatistique(donneConnectedModel));
                    break;
                case 8:
                    gridContent.Children.Clear();
                    gridContent.Children.Add(new UCBrows());
                    break;
                
            }
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ShowUcOnUCPlaceHolderGrid(new UCAccueil(donneConnectedModel));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var resultat = MessageBox.Show("Êtes vous sûr de se déconnecter ? ", "Déconnexion", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(resultat == MessageBoxResult.Yes)
            {
                Login log = new();
                log.Show();
                this.Close();
            }
        }

    }
}
