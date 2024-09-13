using FAV.Helper;
using FAV.ListModels;
using FAV.Models;
using System;
using System.Collections.Generic;
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

namespace FAV.UCs.addPublicite
{
    /// <summary>
    /// Logique d'interaction pour UCAddPub.xaml
    /// </summary>
    public partial class UCAddPub : UserControl
    {
        private string? idUtilisateur;
        private List<ListBoxAddDescri> listeDescri;
        private string filePathCouv;
        public UCAddPub(string idUtilisateur)
        {
            InitializeComponent();
            listeDescri = new();
            this.idUtilisateur = idUtilisateur;
            filePathCouv = string.Empty;
        }

        public void ClearAllControll()
        {
            txtDescription.Text = string.Empty;
            txtNomPub.Text = string.Empty;
            ListBoxDescri.Items.Clear();
        }
        private string RecuperationDuDescriptionDuListBoxDescri()
        {
            if (ListBoxDescri != null && ListBoxDescri.Items.Count > 0)
            {
                StringBuilder items = new StringBuilder();
                foreach (var item in ListBoxDescri.Items)
                {
                    var listBoxItemData = item as ListBoxAddDescri;
                    if (listBoxItemData != null)
                    {
                        items.AppendLine(listBoxItemData.descri);
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

        private void btnAjoutDescriPre_Click(object sender, RoutedEventArgs e)
        {
            if (txtDescriTitre.Text != String.Empty || txtDescriValeur.Text != String.Empty)
            {
                ListBoxDescri.ItemsSource = null;
                string description = txtDescriTitre.Text + " : " + txtDescriValeur.Text;
                listeDescri.Add(new ListBoxAddDescri() { descri = description });
                ListBoxDescri.ItemsSource = listeDescri;
                description = String.Empty;
            }
            txtDescriTitre.Text = String.Empty;
            txtDescriValeur.Text = String.Empty;
        }

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

        private async void btnPublier_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (new WaitProgressRing(progressRing))
                {
                    PubliciteModels publicite = new();
                    publicite.Photo = filePathCouv != null ? File.ReadAllBytes(filePathCouv) : null;
                    publicite.Nom_pub = txtNomPub.Text;
                    publicite.Id_utilisateur = idUtilisateur.ToUpper();
                    if (listeDescri.Count > 0)
                    {
                        publicite.Autre_descri = RecuperationDuDescriptionDuListBoxDescri();
                    }
                    publicite.Descri_pub = txtDescription.Text;
                     var httpService = new HttpService("https://localhost:7104/api/ControllerAPI/");
                    //var httpService = new HttpService("http://favsite.runasp.net/api/ControllerAPI/");
                    var response_1 = await httpService.SendPostRequestAsync("AjoutPublicite", publicite);
                    MessageBox.Show("Votre annonce a été publiée avec succes", "Publication", MessageBoxButton.OK, MessageBoxImage.Information);
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
