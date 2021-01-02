using Makrisoft.Makfi.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makrisoft.Makfi.Models
{
    public class Message : Modele
    {
        public Guid? IdHisto;
        public Guid? De;
        public Guid? A;
        public DateTime DateEnvoi;
        public Guid Etat;
        public string Libelle;
        public string Objet;
    }
}
