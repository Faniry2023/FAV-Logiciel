using System.ComponentModel.DataAnnotations;

namespace FAV.Models
{
    public class CommentaireModels
    {
        [Key]
        public Guid Id_com {get; set;}
        private string? id_utilisateur;
        private string? id_produit;
        private string? commentaire;

        public CommentaireModels() { }

        public CommentaireModels(string? id_utilisateur,string? id_produit,string? commentaire)
        {
            this.id_utilisateur = id_utilisateur;
            this.id_produit = id_produit;
            this.commentaire = commentaire;
        }

        public string? Id_utilisateur { get { return id_utilisateur; } set { id_utilisateur = value; } }
        public string? Id_produit { get { return id_produit;} set {  id_produit = value; } }
        public string? Commentaire { get {  return commentaire; } set {  commentaire = value; } }
    }
}
