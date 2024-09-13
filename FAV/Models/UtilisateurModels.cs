using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FAV.Models
{
    public class UtilisateurModels
    {
        [Key]
        [JsonPropertyName("id_ut")]
        public Guid Id_ut { get; set; }
        private string? id_ad;
        private string? nom_ut;
        private string? prenom_ut;
        private string? contact_ut;
        private string? pays_ut;
        private string? ville_ut;
        private string? nationnalite_ut;
        private string? type_ut;
        private byte[]? photo;

        public UtilisateurModels(string id_ad, string nom_ut, string prenom_ut, string contact_ut, string pays_ut, string ville_ut, string nationnalite_ut, string type_ut, byte[] photo)
        {
            this.id_ad = id_ad;
            this.nom_ut = nom_ut;
            this.prenom_ut = prenom_ut;
            this.contact_ut = contact_ut;
            this.pays_ut = pays_ut;
            this.ville_ut = ville_ut;
            this.nationnalite_ut = nationnalite_ut;
            this.type_ut = type_ut;
            this.photo = photo;
        }
        public UtilisateurModels()
        {
            nom_ut = "inconnue";
            prenom_ut = "inconnue";
            contact_ut = "inconnue";
            pays_ut = "inconnue";
            ville_ut = "inconnue";
            nationnalite_ut = "inconnue";
            type_ut = "inconnue";
        }
        [JsonPropertyName("id_ad")]
        public string? Id_ad { get { return id_ad; } set { id_ad = value; } }
        [JsonPropertyName("nom_ut")]
        public string? Nom_ut { get { return nom_ut; } set { nom_ut = value; } }
        [JsonPropertyName("prenom_ut")]
        public string? Prenom_ut { get { return prenom_ut; } set { prenom_ut = value; } }
        [JsonPropertyName("contact_ut")]
        public string? Contact_ut { get { return contact_ut; } set { contact_ut = value; } }
        [JsonPropertyName("pays_ut")]
        public string? Pays_ut { get { return pays_ut; } set { pays_ut = value; } }
        [JsonPropertyName("ville_ut")]
        public string? Ville_ut { get { return ville_ut; } set { ville_ut = value; } }
        [JsonPropertyName("nationnelite_ut")]
        public string? Nationnelite_ut { get { return nationnalite_ut; } set { nationnalite_ut = value; } }
        [JsonPropertyName("type_ut")]
        public string? Type_ut { get { return type_ut; } set { type_ut = value; } }
        [JsonPropertyName("photo")]
        public byte[] Photo { get { return photo; } set { photo = value; } }

    }
}
