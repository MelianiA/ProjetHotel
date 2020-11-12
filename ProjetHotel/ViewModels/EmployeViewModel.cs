﻿using Makrisoft.Makfi.Dal;
using Makrisoft.Makfi.Models;
using Makrisoft.Makfi.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Makrisoft.Makfi.ViewModels
{
    public class EmployeViewModel : ViewModelBase
    {

        #region Constructeur
        public EmployeViewModel()
        {
            // Icommand
            EmployeModifiedSaveCommand = new RelayCommand(p => OnSaveCommand(), p => OnCanExecuteSaveCommand());
            EmployeSelectedAddCommand = new RelayCommand(p => OnAddCommand(), p => true);
            EmployeSelectedDeleteCommand = new RelayCommand(p => OnDeleteCommand(), p => OnCanExecuteDeleteCommand());
            FilterClearCommand = new RelayCommand(p => OnFilterClearCommand(), p => OnCanExecuteFilterClearCommand());

            // ListeView
            Load_Etat();
            Load_Employes();
            Load_EtatEmploye();
        }
        #endregion

        #region Binding
        //Employe
        public ObservableCollection<Employe_VM> Employes
        {
            get { return employes; }
            set
            {
                employes = value;
                OnPropertyChanged("Employes");

            }
        }
        private ObservableCollection<Employe_VM> employes;
        public ListCollectionView EmployeCollectionView
        {
            get { return employeCollectionView; }
            set { employeCollectionView = value; OnPropertyChanged("EmployeCollectionView"); }
        }
        private ListCollectionView employeCollectionView;
        public Employe_VM CurrentEmploye
        {
            get
            {

                return currentEmploye;

            }
            set
            {
                currentEmploye = value;
                OnPropertyChanged("CurrentEmploye");
            }
        }
        private Employe_VM currentEmploye;

        //Etat
        public ObservableCollection<Etat_VM> EtatList
        {
            get { return etatList; }
            set
            {
                etatList = value;
                OnPropertyChanged("EtatList");
            }
        }
        private ObservableCollection<Etat_VM> etatList;
        public ListCollectionView EtatListCollectionView
        {
            get { return etatListCollectionView; }
            set { etatListCollectionView = value; OnPropertyChanged("EtatListCollectionView"); }
        }
        private ListCollectionView etatListCollectionView;

        //EtatEmploye
        public ObservableCollection<Etat_VM> EtatEmploye
        {
            get { return etatEmploye; }
            set
            {
                etatEmploye = value;
                OnPropertyChanged("EtatEmploye");
            }
        }
        private ObservableCollection<Etat_VM> etatEmploye;
        public ListCollectionView EtatEmployeCollectionView
        {
            get { return etatEmployeCollectionView; }
            set { etatEmployeCollectionView = value; OnPropertyChanged("EtatEmployeCollectionView"); }
        }
        private ListCollectionView etatEmployeCollectionView;

        //Filter
        public Etat_VM CurrentFilter
        {
            get
            {

                return currentFilter;

            }
            set
            {
                currentFilter = value;
                EmployeCollectionView.Filter = FilterEmployes;
                OnPropertyChanged("CurrentFilter");


            }
        }
        private Etat_VM currentFilter;



        #endregion

        #region Commands
        //ICommand
        public ICommand EmployeModifiedSaveCommand { get; set; }
        public ICommand EmployeSelectedAddCommand { get; set; }
        public ICommand EmployeSelectedDeleteCommand { get; set; }
        public ICommand FilterClearCommand { get; set; }

        // Méthodes OnCommand
        private void OnSaveCommand()
        {
            if (CurrentEmploye.Nom == null || CurrentEmploye.Prenom == null || CurrentEmploye.Etat == null)
            {
                MessageBox.Show($"Impossible de sauvgarder l'employeur actuel !", "Remarque !");
                return;
            }
            Guid? monID = null;
            if (CurrentEmploye.Id != default) monID = CurrentEmploye.Id;
            var param = $@"
                    <employe>
                        <id>{monID}</id>
                        <nom>{CurrentEmploye.Nom}</nom>
                        <prenom>{CurrentEmploye.Prenom}</prenom>
                        <etat>{CurrentEmploye.Etat.Id}</etat>
                        <commentaire>{CurrentEmploye.Commentaire}</commentaire>       
                    </employe>";
            var ids = MakfiData.Employe_Save(param);
            if (ids.Count == 0) throw new Exception("Rien n'a été sauvgardé ! ");
            CurrentEmploye.SaveColor = "Navy";
            Employes.Clear();
            Load_Employes();
            CurrentEmploye = Employes.Where(u => u.Id == ids[0].Id).SingleOrDefault();
        }
        private void OnAddCommand()
        {
            CurrentEmploye = new Employe_VM { Nom = "(A définir)" };
            Employes.Add(CurrentEmploye);
            EmployeCollectionView.Refresh();
        }
        private void OnDeleteCommand()
        {
            var canDeletes = MakfiData.Employe_CanDelete($"<employe><id>{CurrentEmploye.Id}</id></employe>");
            if (canDeletes.Count() == 0)
            {
                var param = MakfiData.Employe_Delete($"<employe><id>{CurrentEmploye.Id}</id></employe>");
                if (param)
                {
                    Employes.Remove(CurrentEmploye);
                }
            }
            else
            {
                MessageBox.Show($" Suppression impossible de l'employé : {CurrentEmploye.Nom }", "Remarque !");
            }
        }
        private void OnFilterClearCommand()
        {
            CurrentFilter = null;
        }

        // Méthodes OnCanExecuteCommand
        private bool OnCanExecuteSaveCommand()
        {
            if (CurrentEmploye != null) return true;
            else return false;
        }
        private bool OnCanExecuteDeleteCommand()
        {
            if (CurrentEmploye != null) return true;
            else return false;
        }
        private bool OnCanExecuteFilterClearCommand()
        {
            if (CurrentFilter != null) return true;
            else return false;
        }

        // Divers
        public bool FilterEmployes(object item)
        {
            if(item is Employe_VM employe)
            {
                if (CurrentFilter == null) return true;
                if (CurrentFilter.Libelle == "Disponible")
                    return (employe.Etat.Libelle == "Disponible");
                if (CurrentFilter.Libelle == "Non disponible")
                    return (employe.Etat.Libelle == "Non disponible");
                if (CurrentFilter.Libelle == "Arrêt maladie")
                    return (employe.Etat.Libelle == "Arrêt maladie");
            }
            return true;
        }

        #endregion

        #region Load
        private void Load_Employes()
        {
            Employes = new ObservableCollection<Employe_VM>(
              MakfiData.Employe_Read()
              .Select(x => new Employe_VM
              {
                  Id = x.Id,
                  Nom = x.Nom,
                  //Image = x.Image != null ? $"/Makrisoft.Makfi;component/Assets/hotels/{x.Image}" : $"/Makrisoft.Makfi;component/Assets/hotels/hotel.png",
                  Prenom = x.Prenom,
                  Etat = EtatList.Where(e => e.Id == x.Etat.Id).SingleOrDefault(),
                  Commentaire = x.Commentaire,
                  SaveColor = "Navy"
              }).OrderBy(x => x.Nom).ToList());
            EmployeCollectionView = new ListCollectionView(Employes);
            EmployeCollectionView.Refresh();
        }

        private void Load_Etat()
        {
            EtatList = new ObservableCollection<Etat_VM>(
              MakfiData.Etat_Read()
              .Select(x => new Etat_VM
              {
                  Id = x.Id,
                  Libelle = x.Libelle,
                  Icone = x.Icone,
                  Couleur = x.Couleur,
                  Entite = x.Entite
              }).ToList()); ;
            EtatListCollectionView = new ListCollectionView(EtatList);
            EtatListCollectionView.Refresh();
        }
        private void Load_EtatEmploye()
        {
            EtatEmploye = new ObservableCollection<Etat_VM>(
               EtatList.Where(x => x.Entite == EntiteEnum.Employe).ToList());
            EtatEmployeCollectionView = new ListCollectionView(EtatEmploye);
            EtatEmployeCollectionView.Refresh();
            CurrentFilter = null;
        }

        #endregion
    }
}
