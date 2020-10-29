using Makrisoft.Makfi.Dal;
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
        public string Nom
        {
            get { return nom; }
            set
            {
                nom = value;
                OnPropertyChanged("Nom");
            }
        }
        public string nom;

        public string Image { get; set; }
        public Guid Gouvernante { get; set; }
        public byte Statut { get; set; }
        public DateTime DateModified { get; set; }



        public bool IsAdmin
        {
            get
            {
                return this.Statut == 1; ;
            }
            set
            {
                isAdmin = this.Statut == 1;
                OnPropertyChanged("IsAdmin");
            }
        }
        private bool isAdmin;
        public RoleEnum Role
        {
            get { return role; }
            set
            {
                role = value;
                if (role == RoleEnum.Admin) this.Statut = 1;
                if (role == RoleEnum.Gouvernante) this.Statut = 2;
                if (role == RoleEnum.Reception) this.Statut = 4;
                OnPropertyChanged("Role");

            }
        }
        private RoleEnum role { get; set; }

        private bool canChangeUtilisateur = true;

        public bool CanChangeUtilisateur
        {
            get { return canChangeUtilisateur; }
            set
            {
                canChangeUtilisateur = value;
                OnPropertyChanged("CanChangeUtilisateur");
            }
        }



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
