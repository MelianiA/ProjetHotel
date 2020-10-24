using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makrisoft.Makfi.ViewModels
{
    public class Utilisateur_VM : ViewModelBase
    {
        public Guid Id { get; set; }
        public string Nom { get; internal set; }
        public string Image { get; set; }
        public Guid Gouvernante { get; set; }

    }

    public class Hotel_VM : ViewModelBase
    {
        public Guid Id { get; set; }
        public string Nom { get; internal set; }
        public Guid Reception { get; set; }
        public Guid Gouvernante { get; set; }
        public string Commentaire { get; internal set; }
        public string Image { get; set; }
    }
}
