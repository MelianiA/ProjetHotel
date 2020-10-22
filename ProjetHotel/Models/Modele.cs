 
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ProjetHotel.Models
{
    public class Modele<T> where T : new()
    {
       // public Guid Id { get; set; }
        public Guid Id { get; set; }
        public string Commentaire { get; set; }
        public bool CanDelete { get; set; } = false;
        public DateTime DateModified { get; set; }
        public string DateModifiedIcone { get; set; } = "None";
        public string Erreur { get; set; }
        public string Couleur { get; set; } = "Navy";
        public T Origin { get; set; }
        //public void CreateNew(ViewEnum entityType)
        //{
        //    Id = default;
        //    //if (entityType != ViewEnum.None) Etat = Transition.GetEtatNew(entityType);
        //    Origin = Clone();
        //    DateModified = DateTime.Now;
        //    DateModifiedIcone = "Update";
        //    Commentaire = "(Nouveau)";
        //    CanDelete = true;
        //}
        public virtual T Clone() { return default; }
 }


    public class Etat
    {
        public Guid Id { get; set; }
        public string Icone { get; set; }
        public string Libelle { get; set; }
        public Brush Couleur { get; set; }
    }
}
