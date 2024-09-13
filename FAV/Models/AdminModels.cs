using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAV.Models
{
    public class AdminModels
    {
        [Key]
        public Guid Id { get; set; }
        private string? email;
        private string? mdp;
        private string? contact;
        private string? num_banque;

        public AdminModels() { }
        public AdminModels(string? email, string? mdp, string? contact, string? num_banque)
        {
            this.email = email;
            this.mdp = mdp;
            this.contact = contact;
            this.num_banque = num_banque;
        }

        public string? Email { get { return email; } set { email = value; } }
        public string? Mdp { get { return mdp; } set { mdp = value; } }
        public string? Contact { get { return contact; } set { contact = value; } }
        public string? Num_banque { get { return num_banque; } set { num_banque = value; } }
    }
}
