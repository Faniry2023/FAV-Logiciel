using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FAV.Models
{
    public class Log_UtilisateurModels
    {
        [Key]
        [JsonPropertyName("id_log")]
        public Guid Id_log {get; set; }
        private string? email;
        private string? password;

        public Log_UtilisateurModels()
        {
            email = "inconnue";
            password = "inconnue";
        }

        public Log_UtilisateurModels(string? email, string? password)
        {
            this.email = email;
            this.password = password;
        }
        [JsonPropertyName("email")]
        public string? Email { get { return email; } set { email = value; } }
        [JsonPropertyName("password")]
        public string? Password { get { return password; } set {  password = value; } }
    }
}
