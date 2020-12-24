using Makrisoft.Makfi.Dal;
using System;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace Makrisoft.Makfi.ViewModels
{
    public class ChambreByEtage_VM : ViewModelBase
    {
        public Guid? GroupeChambre;
        public string Nom;
        public Guid IdDelaChambre;
        public string NomChambre
        {
            get
            { return nomChambre; }
            set
            {
                nomChambre = value;
                OnPropertyChanged("NomChambre");
            }
        }
        private string nomChambre;
    }
    public class Chambre_VM : ViewModelBase
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
    public class Etat_VM
    {
        public Guid Id { get; set; }
        public string Icone { get; set; }
        public string Libelle { get; set; }
        public string Couleur { get; set; }
        public EntiteEnum Entite { get; set; }
        public Boolean EtatEtat { get; set; }

    }
    public class Hotel_VM : ViewModelBase
    {
        public Guid Id { get; set; }
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
    public class HotelEmploye_VM
    {
        public Guid Employe { get; set; }
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
                Image = $"/Makrisoft.Makfi;component/Assets/Photos/{nom.ToLower()}.png";
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
        public Guid? Id;
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

        public ObservableCollection<ChambreByEtage_VM> Chambres
        {
            get { return chambreCurrentGroupe; }
            set
            {
                chambreCurrentGroupe = value;
                OnPropertyChanged("ChambreCurrentGroupe");
            }
        }
        private ObservableCollection<ChambreByEtage_VM> chambreCurrentGroupe;

        public ObservableCollection<ChambreByEtage_VM> AutresChambres
        {
            get { return chambreNotCurrentGroupe; }
            set
            {
                chambreNotCurrentGroupe = value;
                OnPropertyChanged("ChambreNotCurrentGroupe");

            }
        }
        private ObservableCollection<ChambreByEtage_VM> chambreNotCurrentGroupe;
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
        public ChambreByEtage_VM CurrentChambreCG
        {
            get { return currentChambreCG; }
            set
            {
                currentChambreCG = value;
                OnPropertyChanged("CurrentChambreCG");
            }
        }
        private ChambreByEtage_VM currentChambreCG;
        public ChambreByEtage_VM CurrentNotChambreCG
        {
            get { return currentNotChambreCG; }
            set
            {
                currentNotChambreCG = value;
                OnPropertyChanged("CurrentNotChambreCG");
            }
        }
        private ChambreByEtage_VM currentNotChambreCG;

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
    public class ChambreGroupeChambre_VM : ViewModelBase
    {
        public Guid Id;
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
        public string GroupeChambre
        {
            get { return groupeChambre; }
            set
            {
                groupeChambre = value;
                SaveColor = "Red";
                OnPropertyChanged("GroupeChambre");
            }
        }
        private string groupeChambre;
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
    public class Intervention_VM : ViewModelBase
    {
        public Guid Id { get; set; }
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
                if (libelle!=null && libelle.EndsWith(Properties.Settings.Default.Autocar))
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
    public class InterventionDetail_VM : ViewModelBase
    {
        public Guid Id;

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
    public class Message_VM  : ViewModelBase
    {
        public Guid Id;
        public Guid MessageInitial;
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

        public string ColorEtat
        {
            get
            { return colorEtat; }
            set
            {
                colorEtat = value;
                OnPropertyChanged("ColorEtat");
            }
        }
        private string colorEtat;

    }
    public class Info_VM
    {
        public Guid Id;
        public string Cle { get; set; }
        public string Valeur { get; set; }

    }
}
