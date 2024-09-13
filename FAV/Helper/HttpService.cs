using FAV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace FAV.Helper
{
    public class HttpService
    {
        private readonly HttpClient _client;

        public HttpService(string baseAddress)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(baseAddress);
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<HttpResponseMessage> SendPostRequestAsync<T>(string endpoint, T data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            return await _client.PostAsync(endpoint, content);
        }


        //recuperation produit par id
        public async Task<ProduitModels> GetProduitAsync(string productId)
        {
            try
            {
                var response = await _client.GetAsync($"api/ControllerAPI/GetProduit/{productId}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var produit = JsonSerializer.Deserialize<ProduitModels>(json);
                    return produit;
                }
                else
                {
                    // Gérer les cas où la requête n'a pas abouti (non trouvé, erreur serveur, etc.)
                    return null;
                }
            }
            catch (HttpRequestException)
            {
                // Gérer les erreurs HTTP
                throw;
            }
            catch (Exception)
            {
                // Gérer les autres exceptions
                throw;
            }
        }
        public async Task<ProduitModels> GetProduitOnlyAsync(string productId)
        {
            try
            {
                var response = await _client.GetAsync($"api/ControllerAPI/GetProduitAdd/{productId}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var produit = JsonSerializer.Deserialize<ProduitModels>(json);
                    return produit;
                }
                else
                {
                    // Gérer les cas où la requête n'a pas abouti (non trouvé, erreur serveur, etc.)
                    return null;
                }
            }
            catch (HttpRequestException)
            {
                // Gérer les erreurs HTTP
                throw;
            }
            catch (Exception)
            {
                // Gérer les autres exceptions
                throw;
            }
        }
        //recuperation image par id produit
        public async Task<Image_produitModels> GetImageAsync(string productId)
        {
            try
            {
                var response = await _client.GetAsync($"api/ControllerAPI/GetImage/{productId}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var produit = JsonSerializer.Deserialize<Image_produitModels>(json);
                    return produit;
                }
                else
                {
                    // Gérer les cas où la requête n'a pas abouti (non trouvé, erreur serveur, etc.)
                    return null;
                }
            }
            catch (HttpRequestException)
            {
                // Gérer les erreurs HTTP
                throw;
            }
            catch (Exception)
            {
                // Gérer les autres exceptions
                throw;
            }
        }

        //recuperation tout produit
        public async Task<List<ProduitModels>> GetAllProduitAsync()
        {
            try
            {
                var response = await _client.GetAsync($"api/ControllerAPI/GetAllProduits/");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var produit = JsonSerializer.Deserialize<List<ProduitModels>>(json);
                    return produit;
                }
                else
                {
                    // Gérer les cas où la requête n'a pas abouti (non trouvé, erreur serveur, etc.)
                    return null;
                }
            }
            catch (HttpRequestException)
            {
                // Gérer les erreurs HTTP
                throw;
            }
            catch (Exception)
            {
                // Gérer les autres exceptions
                throw;
            }
        }
        public async Task<List<UtilisateurModels>> GetAllUserAsync()
        {
            try
            {
                var response = await _client.GetAsync($"api/ControllerAPI/GetAllUser/");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var listeUtilisateur = JsonSerializer.Deserialize<List<UtilisateurModels>>(json);
                    return listeUtilisateur;
                }
                else
                {
                    // Gérer les cas où la requête n'a pas abouti (non trouvé, erreur serveur, etc.)
                    return null;
                }
            }
            catch (HttpRequestException)
            {
                // Gérer les erreurs HTTP
                throw;
            }
            catch (Exception)
            {
                // Gérer les autres exceptions
                throw;
            }
        }
        public async Task<List<Log_UtilisateurModels>> GetAllUserLoginAsync()
        {
            try
            {
                var response = await _client.GetAsync($"api/ControllerAPI/GetAllLoginUser/");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var userLog = JsonSerializer.Deserialize<List<Log_UtilisateurModels>>(json);
                    return userLog;
                }
                else
                {
                    // Gérer les cas où la requête n'a pas abouti (non trouvé, erreur serveur, etc.)
                    return null;
                }
            }
            catch (HttpRequestException)
            {
                // Gérer les erreurs HTTP
                throw;
            }
            catch (Exception)
            {
                // Gérer les autres exceptions
                throw;
            }
        }
        public async Task<List<Uti_vendeurModels>> GetAllUserVendeurAsync()
        {
            try
            {
                var response = await _client.GetAsync($"api/ControllerAPI/GetAllUserVendeur/");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var vendeur = JsonSerializer.Deserialize<List<Uti_vendeurModels>>(json);
                    return vendeur;
                }
                else
                {
                    // Gérer les cas où la requête n'a pas abouti (non trouvé, erreur serveur, etc.)
                    return null;
                }
            }
            catch (HttpRequestException)
            {
                // Gérer les erreurs HTTP
                throw;
            }
            catch (Exception)
            {
                // Gérer les autres exceptions
                throw;
            }
        }
        public async Task<List<Image_produitModels>> GetAllImageAsync()
        {
            try
            {
                var response = await _client.GetAsync($"api/ControllerAPI/GetAllImage/");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var image = JsonSerializer.Deserialize<List<Image_produitModels>>(json);
                    return image;
                }
                else
                {
                    // Gérer les cas où la requête n'a pas abouti (non trouvé, erreur serveur, etc.)
                    return null;
                }
            }
            catch (HttpRequestException)
            {
                // Gérer les erreurs HTTP
                throw;
            }
            catch (Exception)
            {
                // Gérer les autres exceptions
                throw;
            }
        }

        public async Task<List<PubliciteModels>> GetAllPubliciteAsync()
        {
            try
            {
                var response = await _client.GetAsync($"api/ControllerAPI/GetAllPublicite/");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var pub = JsonSerializer.Deserialize<List<PubliciteModels>>(json);
                    return pub;
                }
                else
                {
                    // Gérer les cas où la requête n'a pas abouti (non trouvé, erreur serveur, etc.)
                    return null;
                }
            }
            catch (HttpRequestException)
            {
                // Gérer les erreurs HTTP
                throw;
            }
            catch (Exception)
            {
                // Gérer les autres exceptions
                throw;
            }
        }
        public async Task<List<CommandeModels>> GetAllCommandesAsync()
        {
            try
            {
                var response = await _client.GetAsync($"api/ControllerAPI/GetAllCommande/");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var commandes = JsonSerializer.Deserialize<List<CommandeModels>>(json);
                    return commandes;
                }
                else
                {
                    // Gérer les cas où la requête n'a pas abouti (non trouvé, erreur serveur, etc.)
                    return null;
                }
            }
            catch (HttpRequestException)
            {
                // Gérer les erreurs HTTP
                throw;
            }
            catch (Exception)
            {
                // Gérer les autres exceptions
                throw;
            }
        }
        public async Task<List<StatMois>> GetAllStatMoisAsync()
        {
            try
            {
                var response = await _client.GetAsync($"api/ControllerAPI/GetAllStat/");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var stat = JsonSerializer.Deserialize<List<StatMois>>(json);
                    return stat;
                }
                else
                {
                    // Gérer les cas où la requête n'a pas abouti (non trouvé, erreur serveur, etc.)
                    return null;
                }
            }
            catch (HttpRequestException)
            {
                // Gérer les erreurs HTTP
                throw;
            }
            catch (Exception)
            {
                // Gérer les autres exceptions
                throw;
            }
        }
    }
}
