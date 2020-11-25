using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makrisoft.Makfi.Models
{
    public class Intervention
    {
        public Guid Id;
        public string Libelle;
        public Guid? Etat;
        public DateTime Date1;
        public string Commentaire;
        public Guid? GroupeChambre;

        public bool Model;


    }
}
