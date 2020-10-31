using Makrisoft.Makfi.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makrisoft.Makfi.ViewModels
{
    public class Hotel_VM : ViewModelBase
    {
        public Guid Id { get; set; }
        public string Nom { get;  set; }
        public Utilisateur_VM Reception { get; set; }
        public Utilisateur_VM Gouvernante { get; set; }
        public string Commentaire { get;  set; }
        public string Image { get; set; }
        public string SaveColor
        {
            get
            { return saveColor; }
            set
            {
                saveColor = value;
                OnPropertyChanged("SaveColor");
            }
        }
        private string saveColor = "Navy";
    }
    public class Utilisateur_VM : ViewModelBase
    {
        public Guid Id { get; set; }

        public string Nom
        {
            get { return nom; }
            set
            {
                nom = value;
                SaveColor = "Red";
                OnPropertyChanged("Nom");
            }
        }
        private string nom;
        public string SaveColor
        {
            get 
            { return saveColor; }
            set
            {
                saveColor = value;
                OnPropertyChanged("SaveColor");
            }
        }
        private string saveColor = "Navy";

        public string Image { get; set; }

        public Guid Gouvernante { get; set; }

        public RoleEnum Statut
        {
            get { return statut; }
            set
            {
                statut = value;
                SaveColor = "Red";
                OnPropertyChanged("Statut");
            }
        }
        private RoleEnum statut;
        public DateTime DateModified { get; set; }

        public bool IsAdmin
        {
            get
            {
                return this.Statut == RoleEnum.Admin;
            }
            set
            {
                isAdmin = this.Statut == RoleEnum.Admin;
                OnPropertyChanged("IsAdmin");
            }
        }
        private bool isAdmin;

        public string CodePin
        {
            get { return codePin; }
            set { codePin = value; }
        }
        private string codePin;


    }

}
