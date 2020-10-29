using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makrisoft.Makfi.Models
{
    class Chambre
    {
        public Guid Id;
        public string Nom;
        public Guid Etat;
        public string Commentaire;
        public Guid Hotel;

        //pas dans la BDD
        public Etat LastEtat { get; set; }


    }
}
