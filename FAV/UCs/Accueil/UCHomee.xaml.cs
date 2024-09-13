using FAV.Helper;
using LiveCharts;
using LiveCharts.Wpf;
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

namespace FAV.UCs.Accueil
{
    /// <summary>
    /// Logique d'interaction pour UCHomee.xaml
    /// </summary>
    public partial class UCHomee : UserControl
    {
        public UCHomee(ProdAndImgBitMap produitEtImage)
        {
            InitializeComponent();
            this.DataContext = produitEtImage;
            ShowPieChartData(produitEtImage.produitModels.Nb_total_prod, produitEtImage.produitModels.Nb_produit_reste, produitEtImage.produitModels.Nb_total_prod - produitEtImage.produitModels.Nb_produit_reste);
        }

        private void ShowPieChartData(int total, int reste, int vendue)
        {
            var PorSerieCollection = new SeriesCollection()
            {
                new PieSeries
                {
                    Title = "Reste du produit",
                    Values = new ChartValues<double> {reste },
                    Fill = Brushes.Green,
                    DataLabels = true,
                    FontSize = 40,
                    Foreground = Brushes.DarkBlue,
                },
                new PieSeries
                {
                    Title = "Produit Vendus",
                    Values = new ChartValues<double> {vendue },
                    Fill = Brushes.White,
                    DataLabels = true,
                    FontSize = 40,
                    Foreground = Brushes.DarkBlue,
                },
            };
            PieChart.Series = PorSerieCollection;
        }

        private void PieChart_DataClick(object sender, LiveCharts.ChartPoint chartPoint)
        {

        }
    }
}
