
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ProjetHotel.Models
{
    public class ChambreGroupe : Modele<ChambreGroupe>
    {
        public ChambreGroupe() : base() { }
        public string Libelle { get; set; }
        public List<Chambre> Chambres { get; set; }
        internal void SetModified()
        {
            if (Id == default || Libelle != Origin.Libelle || Commentaire != Origin.Commentaire)
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
        internal void Restore()
        {
            Id = Origin.Id;
            Libelle = Origin.Libelle;
            Commentaire = Origin.Commentaire;
        }

        public override ChambreGroupe Clone()
        {
            return new ChambreGroupe
            {
                Id = this.Id,
                Libelle = this.Libelle,
                Commentaire = this.Commentaire,
            };
        }
        public bool Contains(Chambre chambre)
        {
            return Chambres.Any(g => g.Id == chambre.Id);
        }
    }

    public class Chambre : Modele<Chambre>
    {
        public Chambre() { }
        //public Chambre(ChambreGroupe groupe)
        //{
        //    CreateNew(ViewEnum.Chambre);
        //    Groupe = groupe;
        //}

        public string Libelle { get; set; }
        public Etat LastEtat { get; set; }
        public List<InterventionDetail> InterventionDetails { get; set; }
        public ChambreGroupe Groupe { get; set; }
        internal void Restore()
        {
            Id = Origin.Id;
            Libelle = Origin.Libelle;
            Commentaire = Origin.Commentaire;
            LastEtat = Origin.LastEtat;
        }

        internal void SetModified()
        {
            if (
                Id == default ||
                Libelle != Origin.Libelle ||
                (Groupe == null && Origin.Groupe != null) || (Groupe != null && Origin.Groupe == null) || (Groupe != null && Groupe.Id != Origin.Groupe.Id) ||
                Commentaire != Origin.Commentaire
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

        public override Chambre Clone()
        {
            return new Chambre
            {
                Id = this.Id,
                Libelle = this.Libelle,
                Commentaire = this.Commentaire,
                LastEtat = this.InterventionDetails.Last().Etat,
                Groupe = Groupe == null ? null : this.Groupe.Clone()
            };
        }
    }
    public class Decoupage : Modele<Decoupage>
    {
        public Decoupage() : base() { }
        public string Libelle { get; set; }
        internal void SetModified()
        {
            if (DateModified.Year == 2000)
            {
                DateModifiedIcone = "Delete";
            }
            else if (Id == default || Libelle != Origin.Libelle)
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
        internal void Restore()
        {
            DateModified = default;
            Libelle = Origin.Libelle;
        }

        public override Decoupage Clone()
        {
            return new Decoupage
            {
                Id = this.Id,
                Libelle = this.Libelle
            };
        }
    }
}
