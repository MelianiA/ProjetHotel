using Makrisoft.Makfi.Dal;
using System;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace Makrisoft.Makfi.ViewModels
{
    public class Chambre_VM : ViewModelBase
    {
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
        public Hotel_VM Hotel
        {
            get { return hotel; }
            set
            {
                hotel = value;
                SaveColor = "Red";
                OnPropertyChanged("Hotel");
            }
        }
        private Hotel_VM hotel;
    }
    public class Employe_VM : ViewModelBase
    {
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
    }
    public class Etat_VM : ViewModelBase
    {
        public string Icone { get; set; }
        public string Libelle { get; set; }
        public string Couleur { get; set; }
        public EntiteEnum Entite { get; set; }
        public bool EtatEtat { get; set; }

    }
    public class Hotel_VM : ViewModelBase
    {
        public string Nom
        {
            get { return nom; }
            set
            {
                nom = value;
                Image = $"/Makrisoft.Makfi;component/Assets/hotels/{Nom.ToLower()}.png";
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
        public string Image
        {
            get { return image; }
            set
            {
                image = value;
                OnPropertyChanged("Image");
            }
        }
        private string image;
    }
    public class Utilisateur_VM : ViewModelBase
    {


        public string Nom
        {
            get { return nom; }
            set
            {
                nom = value;
                Image = $"/Makrisoft.Makfi;component/Assets/Photos/{nom.ToLower()}.png";
                SaveColor = "Red";
                OnPropertyChanged("Nom");
            }
        }
        private string nom;


        public string Image
        {
            get { return image; }
            set
            {
                image = value;
                OnPropertyChanged("Image");
            }
        }
        private string image;

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
                return Statut == RoleEnum.Admin;
            }
            set
            {
                OnPropertyChanged("IsAdmin");
            }
        }

        public string CodePin
        {
            get { return codePin; }
            set
            {
                codePin = value;
            }
        }
        private string codePin;


    }
    public class Etage_VM : ViewModelBase
    {
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

        public ObservableCollection<Chambre_VM> Chambres
        {
            get { return chambres; }
            set
            {
                chambres = value;
                OnPropertyChanged("Chambres");
            }
        }
        private ObservableCollection<Chambre_VM> chambres;

        public ObservableCollection<Chambre_VM> AutresChambres
        {
            get { return autresChambres; }
            set
            {
                autresChambres = value;
                OnPropertyChanged("AutresChambres");

            }
        }
        private ObservableCollection<Chambre_VM> autresChambres;
        public ListCollectionView ChambresListview
        {
            get { return chambreCurrentGroupeListview; }
            set { chambreCurrentGroupeListview = value; OnPropertyChanged("ChambreCurrentGroupeListview"); }
        }
        private ListCollectionView chambreCurrentGroupeListview;
        public ListCollectionView AutresChambresListview
        {
            get { return chambreNotCurrentGroupeListview; }
            set { chambreNotCurrentGroupeListview = value; OnPropertyChanged("ChambreNotCurrentGroupeListview"); }
        }
        private ListCollectionView chambreNotCurrentGroupeListview;
        //Les chambres associées à ce groupe de chambre 
        public Chambre_VM CurrentChambreCG
        {
            get { return currentChambreCG; }
            set
            {
                currentChambreCG = value;
                OnPropertyChanged("CurrentChambreCG");
            }
        }
        private Chambre_VM currentChambreCG;
        public Chambre_VM CurrentNotChambreCG
        {
            get { return currentNotChambreCG; }
            set
            {
                currentNotChambreCG = value;
                OnPropertyChanged("CurrentNotChambreCG");
            }
        }
        private Chambre_VM currentNotChambreCG;
    }
    public class Intervention_VM : ViewModelBase
    {
        public string Libelle
        {
            get { return libelle; }
            set
            {
                libelle = value;
                SaveColor = "Red";
                OnPropertyChanged("Libelle");
            }
        }
        private string libelle;

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

        public DateTime Date1
        {
            get { return date1; }
            set
            {
                date1 = value;
                if (libelle != null && libelle.EndsWith(Properties.Settings.Default.Autocar))
                    Libelle = $"Intervention du {date1.ToShortDateString()}{Properties.Settings.Default.Autocar}";

                SaveColor = "Red";
                OnPropertyChanged("Date1");
            }
        }
        private DateTime date1;

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
        public bool Model
        {
            get { return model; }
            set
            {
                model = value;
                SaveColor = "Red";
                OnPropertyChanged("Model");
            }
        }
        private bool model;


    }
    public class InterventionDetail_VM : ViewModelBase
    {

        public string Libelle
        {
            get { return libelle; }
            set
            {
                libelle = value;
                SaveColor = "Red";
                OnPropertyChanged("Libelle");
            }
        }
        private string libelle;

        public Employe_VM Employe
        {
            get { return employe; }
            set
            {
                employe = value;
                SaveColor = "Red";
                OnPropertyChanged("Employe");
            }
        }
        private Employe_VM employe;

        public Chambre_VM Chambre
        {
            get { return chambre; }
            set
            {
                chambre = value;
                SaveColor = "Red";
                OnPropertyChanged("Chambre");
            }
        }
        private Chambre_VM chambre;


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
    }
    public class Message_VM : ViewModelBase
    {
        public Guid? IdHisto;
        public string Libelle
        {
            get { return libelle; }
            set
            {
                libelle = value;
                SaveColor = "Red";
                OnPropertyChanged("Libelle");
            }
        }
        private string libelle;

        public DateTime DateCreation
        {
            get { return date; }
            set
            {
                date = value;
                SaveColor = "Red";
                OnPropertyChanged("DateCreation");
            }
        }
        private DateTime date;

        public Utilisateur_VM De
        {
            get { return de; }
            set
            {
                de = value;
                SaveColor = "Red";
                OnPropertyChanged("De");
            }
        }
        private Utilisateur_VM de;

        public Utilisateur_VM A
        {
            get { return a; }
            set
            {
                a = value;
                SaveColor = "Red";
                OnPropertyChanged("A");
            }
        }
        private Utilisateur_VM a;

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

        public string Objet
        {
            get { return objet; }
            set
            {
                objet = value;
                SaveColor = "Red";
                OnPropertyChanged("Objet");
            }
        }
        private string objet;
    }
    public class Info_VM : ViewModelBase
    {
        public string Cle { get; set; }
        public string Valeur { get; set; }

    }
}
