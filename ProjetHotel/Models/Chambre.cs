using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makrisoft.Makfi.Models
{
    public class Chambre : Modele
    {
        public string Nom;
        public Guid Etat;
        public string Commentaire;
        public Hotel Hotel;
    }
}
