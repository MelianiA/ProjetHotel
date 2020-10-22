using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ProjetHotel.Models
{
   public class MessageErreur
    {
        public string Message { get; set; }
        public Visibility Visibilite { get; set; }
        public Brush Couleur { get; set; }
        public DateTime Date { get; set; }
    }
}
