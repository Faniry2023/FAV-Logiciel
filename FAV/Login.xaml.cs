using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FAV.Helper;
using System.Net.Http;
using FAV.Models;

namespace FAV
{
    /// <summary>
    /// Logique d'interaction pour Login.xaml
    /// </summary>
    public partial class Login : MetroWindow
    {
        string? textClaire;
        string? mdp;
        public Login()
        {
            InitializeComponent();
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
            else
            {
                if (sender is ToggleButton senderTgglBtn)
                {
                    if (senderTgglBtn is not null)
                    {
                        if (senderTgglBtn.IsChecked == true)
                        {
                            txtpassword.Visibility = Visibility.Collapsed;
                            txtpasswordClaire.Visibility = Visibility.Visible;
                            txtpasswordClaire.Text = txtpassword.Password;
                            txtpasswordClaire.Focus();
                            txtpasswordClaire.SelectionStart = txtpasswordClaire.Text.Length;
                        }
                        else
                        {
                            txtpassword.Visibility = Visibility.Visible;
                            txtpasswordClaire.Visibility = Visibility.Collapsed;
                        }
                    }
                }
            }
        }

        private void txtpassword_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            BtnWindowrestore.Visibility = Visibility.Visible;
        }

        private void txtpassword_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if(txtpassword.Password.Length > 0)
            {
                BtnWindowrestore.Visibility = Visibility.Visible;
            }
            else
            {
                BtnWindowrestore.Visibility = Visibility.Hidden;
            }
        }

        private void txtCodeReferenceLogiciel_GotFocus(object sender, RoutedEventArgs e)
        {
            BtnWindowrestore.Visibility = Visibility.Hidden;
        }

        private void txtEmail_GotFocus(object sender, RoutedEventArgs e)
        {
            BtnWindowrestore.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            textClaire = txtpasswordClaire.Text;
            mdp = txtpassword.Password;
            MessageBox.Show($"text claire : {textClaire} et mdp : {mdp}");
            BtnWindowrestore.Visibility = Visibility.Hidden;
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BtnWindowrestore.Visibility = Visibility.Hidden;
        }

        private void txtpasswordClaire_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            BtnWindowrestore.Visibility = Visibility.Visible;
        }

        private async void btnSeConnecter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using(new WaitProgressRing(progressRing))
                {
                    btnSeConnecter.IsEnabled = false;
                    HttpService httpService = new("https://localhost:7104");
                    //HttpService httpService = new("http://favsite.runasp.net/");
                    var toutUserLogin = await httpService.GetAllUserLoginAsync();
                    var toutVendeur = await httpService.GetAllUserVendeurAsync();
                    bool vEmail = false, vPass = false, vCode = false, Validation = false;
                    var userConnected = new Log_UtilisateurModels();
                    if (txtEmail.Text != string.Empty && txtpassword.Password != string.Empty && txtCodeReferenceLogiciel.Text != string.Empty)
                    {
                        foreach (var item in toutUserLogin)
                        {
                            if (txtEmail.Text.Equals(item.Email))
                            {
                                emailE.Content = string.Empty;

                                vEmail = true;
                            }
                            if (txtpassword.Password.Equals(item.Password))
                            {
                                pas.Content = string.Empty;
                                vPass = true;
                            }
                            if (vEmail && vPass)
                            {
                                    foreach (var item2 in toutVendeur)
                                    {
                                        if (txtCodeReferenceLogiciel.Text.Equals(item2.RefLogiciel))
                                        {
                                            codeE.Content = string.Empty;
                                            vCode = true;
                                            break;
                                        }
                                    }
                            }
                            if (vEmail && vPass && vCode)
                            {
                                Validation = true;
                                userConnected = toutUserLogin.FirstOrDefault(p => p.Email.Equals(txtEmail.Text) && p.Password.Equals(txtpassword.Password));
                            }
                        }

                    }
                    if (Validation)
                    {
                        MainWindow mainWindow = new(userConnected.Id_log.ToString());
                        mainWindow.Show();
                        this.Close();
                    }
                    if (!vCode)
                    {
                        codeE.Content = "Votre code de reference est invalide";
                    }
                    if (!vEmail)
                    {
                        emailE.Content = "On n'a pas pu trouver votre email";
                    }
                    if (!vPass)
                    {
                        pas.Content = "Votre mot de passe est incorrecte";
                    }
                }
                btnSeConnecter.IsEnabled = true;
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
