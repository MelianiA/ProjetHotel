using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ProjetHotel.Models
{
    public class Employe : Modele<Employe>
    {
        public Employe() { }
        //public Employe(Etat etat)
        //{
        //    CreateNew( Data.ViewEnum.Employe);
        //    Etat = etat;
        //}
        public Etat Etat { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        internal void Restore()
        {
            Id = Origin.Id;
            Nom = Origin.Nom;
            Commentaire = Origin.Commentaire;
            Etat = Origin.Etat;
        }

        internal void SetModified()
        {
            if (Id == default || Nom != Origin.Nom ||
                           Etat.Id != Origin.Etat.Id ||
                           Commentaire != Origin.Commentaire)
            {
                DateModified = DateTime.Now;
                DateModifiedIcone = "Update";
            }
            else
            {
                DateModified = default;
                DateModifiedIcone = "None";
            }
        }

        public override Employe Clone()
        {
            return new Employe
            {
                Id = this.Id,
                Nom = this.Nom,
                Commentaire = this.Commentaire,
                Etat = this.Etat
            };
        }
    }
}
