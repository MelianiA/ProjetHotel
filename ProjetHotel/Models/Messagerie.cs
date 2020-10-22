 
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetHotel.Models
{
    public class Message 
    {
        public Guid Id { get; set; }
        public string Expediteur { get; set; }
        public string Objet { get; set; }
        public DateTime DateRecu { get; set; }
        public Guid LeDestinataire { get; set; }
    }

    public class Destinataire
    {
        public Guid Id { get; set; }
        public Utilisateur LaGouvernante { get; set; }
        public Hotel LHotel { get; set; }
        public string Autre { get; set; }
    }
}
