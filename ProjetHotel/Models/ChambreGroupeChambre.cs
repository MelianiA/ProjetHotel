using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makrisoft.Makfi.Models
{
    public class ChambreGroupeChambre
    {
        public Guid Id;
        public string Nom;
        public Guid Etat;
        public string Commentaire;
        public string GroupeChambre;

    }

    public class ChambreByGroupe
    {
        public Guid? GroupeChambre;
        public string Nom;
        public Guid IdDelaChambre;
        public string NomChambre;
 
    }
}
