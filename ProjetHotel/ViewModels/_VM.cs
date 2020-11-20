﻿using Makrisoft.Makfi.Dal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

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
    public class HotelEmploye_VM
    {
        public Guid Employe { get; set; }
     }
    public class GroupeChambre_VM:ViewModelBase
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

        public ObservableCollection<ChambreByGroupe_VM> ChambreCurrentGroupe
        {
            get { return chambreCurrentGroupe; }
            set
            {
                chambreCurrentGroupe = value;
                OnPropertyChanged("ChambreCurrentGroupe");

            }
        }
        private ObservableCollection<ChambreByGroupe_VM> chambreCurrentGroupe;
        public ObservableCollection<ChambreByGroupe_VM> ChambreNotCurrentGroupe
        {
            get { return chambreNotCurrentGroupe; }
            set
            {
                chambreNotCurrentGroupe = value;
                OnPropertyChanged("ChambreNotCurrentGroupe");

            }
        }
        private ObservableCollection<ChambreByGroupe_VM> chambreNotCurrentGroupe;
        public ListCollectionView ChambreCurrentGroupeListview
        {
            get { return chambreCurrentGroupeListview; }
            set { chambreCurrentGroupeListview = value; OnPropertyChanged("ChambreCurrentGroupeListview"); }
        }
        private ListCollectionView chambreCurrentGroupeListview;
        public ListCollectionView ChambreNotCurrentGroupeListview
        {
            get { return chambreNotCurrentGroupeListview; }
            set { chambreNotCurrentGroupeListview = value; OnPropertyChanged("ChambreNotCurrentGroupeListview"); }
        }
        private ListCollectionView chambreNotCurrentGroupeListview;
        public ChambreByGroupe_VM CurrentChambreCG
        {
            get { return currentChambreCG; }
            set
            {
                currentChambreCG = value;
                OnPropertyChanged("CurrentChambreCG");
            }
        }
        private ChambreByGroupe_VM currentChambreCG;
        public ChambreByGroupe_VM CurrentNotChambreCG
        {
            get { return currentNotChambreCG; }
            set
            {
                currentNotChambreCG = value;
                OnPropertyChanged("CurrentNotChambreCG");
            }
        }
        private ChambreByGroupe_VM currentNotChambreCG;

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
    public class ChambreByGroupe_VM : ViewModelBase
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
    public class Intervention_VM :ViewModelBase
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
        public GroupeChambre_VM GroupeChambre 
        {
            get { return groupeChambre; }
            set
            {
                groupeChambre = value;
                SaveColor = "Red";
                OnPropertyChanged("GroupeChambre");
            }
        }
        private GroupeChambre_VM groupeChambre;

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
