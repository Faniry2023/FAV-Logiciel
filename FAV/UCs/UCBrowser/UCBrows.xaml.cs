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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FAV.UCs.UCBrowser
{
    /// <summary>
    /// Logique d'interaction pour UCBrows.xaml
    /// </summary>
    public partial class UCBrows : UserControl
    {
        public UCBrows()
        {
            InitializeComponent();
            InitializeWebView();
        }
        private async void InitializeWebView()
        {
            // Assure-toi que le composant WebView2 est prêt
            await myWebView.EnsureCoreWebView2Async(null);
            // Définit l'URL de la page à afficher
            myWebView.CoreWebView2.Navigate("https://localhost:7104/");
        }
    }
}
