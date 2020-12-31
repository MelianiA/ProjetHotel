using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makrisoft.Makfi.Models
{
    public class Employe
    {
        public Guid Id { get; set; }
        public Guid Etat { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Commentaire { get; set; }
    }
}
