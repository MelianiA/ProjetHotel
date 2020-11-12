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
        public Etat Etat;
        public string Commentaire;
        public Hotel Hotel;

        //pas dans la BDD
        public Etat LastEtat { get; set; }


    }
}
