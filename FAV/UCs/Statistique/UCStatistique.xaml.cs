using FAV.Helper;
using FAV.Models;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Wpf.Charts.Base;
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

namespace FAV.UCs.Statistique
{
    /// <summary>
    /// Logique d'interaction pour UCStatistique.xaml
    /// </summary>
    public partial class UCStatistique : UserControl
    {
        private static List<ProduitModels>? mesProduit;
        private static string? idUser;
        private static DonneConnectedModel? donneConnectedModel;
        private static List<StatMois>? statMois;
        public UCStatistique(DonneConnectedModel? donneConnectedModelP)
        {
            InitializeComponent();
            donneConnectedModel = donneConnectedModelP;
            RecStat();
            idUser = donneConnectedModel.IdUserConnected;
            InitData();
            InitializeChart();


        }
        public async void InitData()
        {
            using (new WaitProgressRing(progressRing))
            {
                mesProduit = donneConnectedModel.produitModels;
                ListBoxProduit.ItemsSource = mesProduit;
                ShowBarChartData(1, 1, 1);
            }
        }
        private async void RecStat()
        {
            statMois = donneConnectedModel.listStatMois;
        }
        private void ShowBarChartData(int total, int reste, int vendu)
        {
            if (reste > 5)
            {
                var BarSerieCollection = new SeriesCollection()
                {
                    new ColumnSeries
                    {
                    Title = $"Nombre total du produit {total}",
                    Values = new ChartValues<double>{total},
                    Fill = Brushes.Blue,
                    DataLabels = true,
                    FontSize = 20,
                    MaxColumnWidth = 100,
                    ColumnPadding = 50,
                    Foreground = Brushes.White,
                    },
                    new ColumnSeries
                    {
                    Title = $"Nombre du produit restant {reste}",
                    Values = new ChartValues<double>{reste},
                    Fill = Brushes.Green,
                    DataLabels = true,
                    FontSize = 20,
                    MaxColumnWidth = 100,
                    ColumnPadding = 50,
                    Foreground = Brushes.White,
                    },
                    new ColumnSeries
                    {
                    Title = $"Nombre des produits vendus {vendu}",
                    Values = new ChartValues<double>{vendu },
                    Fill = Brushes.White,
                    DataLabels = true,
                    FontSize = 20,
                    MaxColumnWidth = 100,
                    ColumnPadding = 50,
                    Foreground = Brushes.White,
                    },
                };
                BarChart.Series = BarSerieCollection;
                BarAxisY.MaxValue = total + 15;
            }
            if (reste < 5)
            {
                var BarSerieCollection = new SeriesCollection()
                {
                    new ColumnSeries
                    {
                    Title = $"Nombre total du produit {total}",
                    Values = new ChartValues<double>{total},
                    Fill = Brushes.Blue,
                    DataLabels = true,
                    FontSize = 20,
                    MaxColumnWidth = 100,
                    ColumnPadding = 50,
                    Foreground = Brushes.White,
                    },
                    new ColumnSeries
                    {
                    Title = $"Nombre du produit restant {reste}",
                    Values = new ChartValues<double>{reste},
                    Fill = Brushes.Red,
                    DataLabels = true,
                    FontSize = 20,
                    MaxColumnWidth = 100,
                    ColumnPadding = 50,
                    Foreground = Brushes.White,
                    },
                    new ColumnSeries
                    {
                    Title = $"Nombre des produits vendus {vendu}",
                    Values = new ChartValues<double>{vendu },
                    Fill = Brushes.White,
                    DataLabels = true,
                    FontSize = 20,
                    MaxColumnWidth = 100,
                    ColumnPadding = 50,
                    Foreground = Brushes.White,
                    },
                };
                BarChart.Series = BarSerieCollection;
                BarAxisY.MaxValue = total + 15;
            }
        }

        private void InitializeChart()
        {
            // Créer une nouvelle série de lignes
            var lineSeries = new LineSeries
            {
                StrokeThickness = 2,
                Stroke = Brushes.Yellow,
                PointGeometrySize = 5,
                Values = new ChartValues<double> { 50, 50, 50.50, 50, 50, 50, 50, 50, 50, 50, 50, 50 },
                Title = "Statistique de l'année",
                Fill = new LinearGradientBrush
                {
                    GradientStops = new GradientStopCollection
                    {
                        new GradientStop { Color = Colors.Transparent, Offset = 0 },
                        new GradientStop { Color = Colors.Transparent, Offset = 2 }
                    }
                }
            };

            // Ajouter la série au graphique
            MyChart.Series.Add(lineSeries);
        }
        private void InitializeChartTest(StatMois statMois)
        {
            // Créer une nouvelle série de lignes
            var lineSeries = new SeriesCollection()
            {
                new LineSeries()
                {
                    StrokeThickness = 2,
                Stroke = Brushes.Yellow,
                PointGeometrySize = 5,
                Values = new ChartValues<double> { statMois.jan, statMois.fev, statMois.mar, statMois.avr, statMois.mai, statMois.jui, statMois.juill, statMois.aou, statMois.sep, statMois.oct, statMois.nov, statMois.dec },
                Title = "Statistique de l'année",
                Fill = new LinearGradientBrush
                {
                    GradientStops = new GradientStopCollection
                    {
                        new GradientStop { Color = Colors.Transparent, Offset = 0 },
                        new GradientStop { Color = Colors.Black, Offset = 2 }
                    }
                }
                }
            };

            // Ajouter la série au graphique

            MyChart.Series = lineSeries;
        }
        private void ListBoxProduit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (new WaitProgressRing(progressRingBar))
            {
                MyChart.Series.Clear();
                var selProd = ListBoxProduit.SelectedItem as ProduitModels;
                if (selProd != null)
                {
                    var produit = mesProduit.FirstOrDefault(p => p.Id_produit.ToString().ToUpper().Equals(selProd.Id_produit.ToString().ToUpper()));
                    if (produit != null)
                    {
                        lblNomProduit.Content = produit.Nom_produit;
                        lblprix.Content = produit.Prix + " Ariary";
                        lblMarque.Content = produit.Marque;
                        lblnbtotal.Content = produit.Prix + " Ariary";
                        lblprixtotal.Content = produit.Prix * produit.Nb_total_prod + " Ariary";
                        lblprixvendu.Content = ((produit.Nb_total_prod - produit.Nb_produit_reste) * produit.Prix + " Ariary");
                        if (produit.Nb_produit_reste < 5)
                        {
                            lblnbtotal.Content = produit.Nb_produit_reste;
                            lblTRest.Foreground = Brushes.Red;
                            lblnbtotal.Foreground = Brushes.Red;
                        }
                        else
                        {
                            lblnbtotal.Content = produit.Nb_produit_reste;
                            lblTRest.Foreground = Brushes.White;
                            lblnbtotal.Foreground = Brushes.White;
                        }
                        txtDescription.Text = produit.Description_produit;
                        int total = produit.Nb_total_prod;
                        int reste = produit.Nb_produit_reste;
                        int vendu = produit.Nb_total_prod - produit.Nb_produit_reste;
                        var statProd = statMois.FirstOrDefault(p => p.id_produit.ToUpper().Equals(produit.Id_produit.ToString().ToUpper()));
                        InitializeChartTest(statProd);
                        ShowBarChartData(total, reste, vendu);
                    }
                }
            }
        }

        private void txtMarque_TextChanged(object sender, TextChangedEventArgs e)
        {
            using (new WaitProgressRing(progressRing))
            {
                var prodSearch = mesProduit.Where(p => p.Nom_produit.ToLower().Contains(txtSeatrch.Text.Trim().ToLower()));
                if (prodSearch != null)
                {
                    ListBoxProduit.ItemsSource = prodSearch;
                }
                if (txtSeatrch.Text.Trim() == string.Empty)
                {
                    ListBoxProduit.ItemsSource = mesProduit;
                }

            }
        }

        private async void Actualiser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HttpService httpService = new HttpService("https://localhost:7104");
                var toutProdViaHttp = await httpService.GetAllProduitAsync();
                var mesProduit = new List<ProduitModels>();
                if (toutProdViaHttp.Count > 0 && toutProdViaHttp != null)
                {
                    mesProduit = toutProdViaHttp.Where(p => p.Id_vendeur.ToUpper().Equals(donneConnectedModel.IdVendeurConnected.ToUpper())).ToList();
                    donneConnectedModel.produitModels = new();
                    donneConnectedModel.produitModels = mesProduit;
                    var statMoisViaHttp = await httpService.GetAllStatMoisAsync();
                    if (statMoisViaHttp.Count > 0 && statMoisViaHttp != null)
                    {
                        donneConnectedModel.listStatMois = new();
                        foreach (var item in mesProduit)
                        {
                            donneConnectedModel.listStatMois.Add(statMoisViaHttp.FirstOrDefault(s => s.id_produit.ToUpper().Equals(item.Id_produit.ToString().ToUpper())));
                        }
                        InitData();
                        FenetrePrincipale refreshFenetre = new FenetrePrincipale(donneConnectedModel);
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
    }
}
