using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FAV.Models
{
    public class Uti_vendeurModels
    {
        [Key]
        [JsonPropertyName("id_uti_ven")]
        public Guid Id_uti_ven { get; set; }
        private string? id_uti;
        private string? id_ad;
        private string? nom_societe;
        private string? lieuVen;
        private string? refLogiciel;

        public Uti_vendeurModels(string id_uti, string id_ad, string nom_societe, string lieuVen, string refLogiciel)
        {
            this.id_uti = id_uti;
            this.id_ad = id_ad;
            this.nom_societe = nom_societe;
            this.lieuVen = lieuVen;
            this.refLogiciel = refLogiciel;
        }

        public Uti_vendeurModels()
        {
            id_uti = "inconnue";
            nom_societe = "inconnue";
            lieuVen = "inconnue";
            refLogiciel = "inconnue";
        }
        [JsonPropertyName("id_uti")]
        public string? Id_uti { get { return id_uti; } set { id_uti = value; } }
        [JsonPropertyName("id_ad")]
        public string? Id_ad { get { return id_ad; } set { id_ad = value; } }
        [JsonPropertyName("nom_Societe")]
        public string? Nom_Societe { get { return nom_societe; } set { nom_societe = value; } }
        [JsonPropertyName("lienVen")]
        public string? LienVen { get { return lieuVen; } set { lieuVen = value; } }
        [JsonPropertyName("refLogiciel")]
        public string? RefLogiciel { get { return refLogiciel; } set { refLogiciel = value; } }
    }
}
