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
        public Utilisateur_VM Reception
        {
            get { return reception; }
            set
            {
                reception = value;
                SaveColor = "Red";
                OnPropertyChanged("Reception");
            }
        }
        private Utilisateur_VM reception;
        public Utilisateur_VM Gouvernante
        {
            get { return gouvernante; }
            set
            {
                gouvernante = value;
                SaveColor = "Red";
                OnPropertyChanged("Gouvernante");
            }
        }
        private Utilisateur_VM gouvernante;
        public string Commentaire
        {
            get { return commentaire; }
            set
            {
                commentaire = value;
                SaveColor = "Red";
                OnPropertyChanged("Commentaire");
            }
        }
        private string commentaire;
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
        public Guid Id
        {
            get
            {
                return id;
            }
            set { id = value; }
        }
        private Guid id;

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

        // public Guid Gouvernante { get; set; }

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

    public class Etat_VM
    {
        public Guid Id { get; set; }
        public string Icone { get; set; }
        public string Libelle { get; set; }
        public string Couleur { get; set; }
        public EntiteEnum Entite { get; set; }
    }
    public class Employe_VM : ViewModelBase
    {
        public Guid Id { get; set; }
        public Etat_VM Etat
        {
            get { return etat; }
            set
            {
                etat = value;
                SaveColor = "Red";
                OnPropertyChanged("Etat");
            }
        }
        private Etat_VM etat;
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
        public string Prenom
        {
            get { return prenom; }
            set
            {
                prenom = value;
                SaveColor = "Red";
                OnPropertyChanged("Prenom");
            }
        }
        private string prenom;
        public string Commentaire
        {
            get { return commentaire; }
            set
            {
                commentaire = value;
                SaveColor = "Red";
                OnPropertyChanged("Commentaire");
            }
        }
        private string commentaire;
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
}
