using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetHotel.Models
{
    public class Hotel : Modele<Hotel>
    {
        public Hotel() { }
        //public Hotel(Etat etat)
        ////{
        ////    CreateNew(Data.ViewEnum.Hotel);
        ////}
        public string Nom { get; set; }
        public Models.Utilisateur Gouvernante { get; set; }
        public Models.Utilisateur Reception { get; set; }
        public ObservableCollection<ChambreGroupe> ChambreGroupes { get; set; } = new ObservableCollection<ChambreGroupe>();
        public ChambreGroupe ChambreGroupeSelected { get; set; }

        internal void Restore()
        {
            Id = Origin.Id;
            Nom = Origin.Nom;
            Commentaire = Origin.Commentaire;
        }

        internal void SetModified()
        {
            if (Id == default || 
                Nom != Origin.Nom || 
                Commentaire != Origin.Commentaire || 
                (Gouvernante != null && Gouvernante.Nom != Origin.Gouvernante.Nom) || 
                (Reception != null && Reception.Nom != Origin.Reception.Nom))
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

        public override Hotel Clone()
        {
            return new Hotel
            {
                Id = this.Id,
                Nom = this.Nom,
                Commentaire = this.Commentaire,
                Gouvernante = Gouvernante==null ? null : new Utilisateur { Id = this.Gouvernante.Id, Nom = this.Gouvernante.Nom },
                Reception = Reception == null ? null : new Utilisateur { Id = this.Reception.Id, Nom = this.Reception.Nom }
            };
        }
    }
}
