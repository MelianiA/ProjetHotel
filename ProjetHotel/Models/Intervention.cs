using System;
using System.Collections.Generic;

namespace ProjetHotel.Models
{
    public class Intervention : Modele<Intervention>
    {
        public Intervention() { }
        //public Intervention(Etat etat)
        //{
        //    CreateNew(Data.ViewEnum.Intervention);
        //}
        public string Libelle { get; set; }
        public bool IsModele { get; set; }
        public string ModeleLibelle { get; set; }
        public DateTime Date1 { get; internal set; }
        public List<InterventionDetail> Details { get; set; }
        internal void Restore()
        {
            Id = Origin.Id;
            Libelle = Origin.Libelle;
            Commentaire = Origin.Commentaire;
        }

        internal void SetModified()
        {
            if (Id == default ||
                Libelle != Origin.Libelle ||
                //Etat.Id != Origin.Etat.Id ||
                ModeleLibelle != Origin.ModeleLibelle ||
                IsModele != Origin.IsModele ||
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

        public override Intervention Clone()
        {
            return new Intervention
            {
                Id = this.Id,
                Libelle = this.Libelle,
                Commentaire = this.Commentaire,
                ModeleLibelle = this.ModeleLibelle,
                IsModele = this.IsModele
            };
        }
    }

    public class InterventionDetail : Modele<InterventionDetail>
    {
        public InterventionDetail() { }
        //public InterventionDetail(Etat etat)
        //{
        //    CreateNew(Data.ViewEnum.InterventionDetail);
        //    Etat = etat;
        //}
        public bool Selection { get; set; }
        public Etat Etat { get; set; }
        public Employe EmployeAffecte { get; set; }
        public Chambre ChambreAffectee { get; set; }
        public Intervention Intervention { get; set; }
        internal void Restore()
        {
            Id = Origin.Id;
            Commentaire = Origin.Commentaire;
            Etat = Origin.Etat;
            ChambreAffectee = Origin.ChambreAffectee;
            EmployeAffecte = Origin.EmployeAffecte;
        }

        internal void SetModified()
        {
            if (
                Id == default ||
                Etat.Id != Origin.Etat.Id ||
                Commentaire != Origin.Commentaire ||
                ChambreAffectee.Id != Origin.ChambreAffectee.Id ||
                (EmployeAffecte != null && Origin.EmployeAffecte == null) ||
                (EmployeAffecte == null && Origin.EmployeAffecte != null) ||
                (EmployeAffecte != null && Origin.EmployeAffecte != null && EmployeAffecte.Id != Origin.EmployeAffecte.Id)
                )
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

        public override InterventionDetail Clone()
        {
            return new InterventionDetail
            {
                Id = this.Id,
                ChambreAffectee = this.ChambreAffectee,
                EmployeAffecte = this.EmployeAffecte,
                Commentaire = this.Commentaire,
                Etat = this.Etat
            };
        }
    }
}
