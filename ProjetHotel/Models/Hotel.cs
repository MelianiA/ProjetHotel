using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makrisoft.Makfi.Models
{
    public class Hotel : Modele
    {
        public string Nom;   
        public Guid? Reception;
        public Guid? Gouvernante;
        public string Commentaire;
        public string Image;
    }
}
