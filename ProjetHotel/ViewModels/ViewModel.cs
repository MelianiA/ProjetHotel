using Makrisoft.Makfi.Dal;
using Makrisoft.Makfi.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Makrisoft.Makfi.ViewModels
{
    [Flags]
    public enum LoadEnum
    {
        None = 0, Etats = 1, Employes = 2, Chambres = 4, Etages = 8,
        DateDebut = 16,
        DateFin = 32
    }

    public class ViewModel<Dg_VM> : ViewModelBase where Dg_VM : ViewModelBase
    {
        #region Propriétés
        protected EntiteEnum EtatType;
        protected SortDescription[] SortDescriptions;
        public LoadEnum MustLoad = LoadEnum.None;
        #endregion

        #region Constructeur
        public void Init()
        {
            // Icommand
            SaveCommand = new RelayCommand(p => OnSaveCommand(), p => OnCanExecuteSaveCommand());
            AddCommand = new RelayCommand(p => OnAddCommand(), p => true);
            DeleteCommand = new RelayCommand(p => OnDeleteCommand(), p => OnCanExecuteDeleteCommand());
            FilterClearCommand = new RelayCommand(p => OnFilterClearCommand());
            ChangeView = new RelayCommand(p => OnChangeViewCommand(), p => OnCanExecuteChangeView());

            // Load
            if (Reference_ViewModel.Header.CurrentHotel != null)
            {
                if (MustLoad.HasFlag(LoadEnum.Etats)) Load_Etats();
                if (MustLoad.HasFlag(LoadEnum.Employes)) Load_Employes(Reference_ViewModel.Header.CurrentHotel.Id);
                if (MustLoad.HasFlag(LoadEnum.Chambres)) Load_Chambres(Reference_ViewModel.Header.CurrentHotel.Id);
                if (MustLoad.HasFlag(LoadEnum.Etages)) Load_Etages(Reference_ViewModel.Header.CurrentHotel.Id);
                Load_DgSource();
            }

            // DgSourceCollectionView
            DgSourceCollectionView = new ListCollectionView(DgSource);
            DgSourceCollectionView.Filter = DgSourceFilter;
            foreach (var d in SortDescriptions) DgSourceCollectionView.SortDescriptions.Add(d);
            CurrentDgSource = DgSource.FirstOrDefault();
        }
        #endregion

        #region Binding

        // DgSource
        public ObservableCollection<Dg_VM> DgSource
        {
            get { return dgSource; }
            set { dgSource = value; OnPropertyChanged("DgSource"); }
        }
        private ObservableCollection<Dg_VM> dgSource;
        public Dg_VM CurrentDgSource
        {
            get { return currentDgSource; }
            set
            {
                currentDgSource = value;
                if (currentDgSource == null) IsModifierEnabled = false;
                else IsModifierEnabled = true;
                OnPropertyChanged("CurrentDgSource");
            }
        }
        private Dg_VM currentDgSource;
        public ListCollectionView DgSourceCollectionView
        {
            get { return dgSourceCollectionView; }
            set
            {
                dgSourceCollectionView = value;
                OnPropertyChanged("DgSourceCollectionView");
            }
        }
        private ListCollectionView dgSourceCollectionView;

        // IsModifierEnabled
        public bool IsModifierEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                OnPropertyChanged("IsModifierEnabled");
            }
        }
        private bool isEnabled;

        // Etats
        public ObservableCollection<Etat_VM> Etats
        {
            get { return etats; }
            set
            {
                etats = value;
                OnPropertyChanged("Etats");
            }
        }
        private ObservableCollection<Etat_VM> etats;

        // Etats
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
        public ObservableCollection<Etage_VM> Etages
        {
            get { return etages; }
            set
            {
                etages = value;
                OnPropertyChanged("Etages");
            }
        }
        private ObservableCollection<Etage_VM> etages;

        //Filter
        public Etat_VM FilterEtat
        {
            get { return filterEtat; }
            set
            {
                filterEtat = value;
                OnPropertyChanged("FilterEtat");
                DgSourceCollectionView.Refresh();
            }
        }
        private Etat_VM filterEtat;
        public Etage_VM FilterEtage
        {
            get { return filterEtage; }
            set
            {
                filterEtage = value;
                if (FilterEtage != null)
                {
                    filterEtage.Chambres = new ObservableCollection<Chambre_VM>(
                        MakfiData.Chambre_Read($"<chambres><groupeChambre>{filterEtage.Id}</groupeChambre></chambres>")
                        .Select(x => new Chambre_VM
                        {
                            Id = x.Id
                        }));
                }
                OnPropertyChanged("FilterEtage");
                DgSourceCollectionView.Refresh();
            }
        }
        private Etage_VM filterEtage;
        public Employe_VM FilterEmploye
        {
            get { return filterEmploye; }
            set
            {
                filterEmploye = value;
                OnPropertyChanged("FilterEmploye");
                DgSourceCollectionView.Refresh();
            }
        }
        private Employe_VM filterEmploye;
        public DateTime? FilterDateDebut
        {
            get { return filterDateDebut; }
            set
            {
                filterDateDebut = value;
                OnPropertyChanged("FilterDateDebut");
                DgSourceCollectionView.Refresh();

            }
        }
        private DateTime? filterDateDebut = null;
        public DateTime? FilterDateFin
        {
            get { return filterDateFin; }
            set
            {
                filterDateFin = value;
                OnPropertyChanged("FilterDateFin");
                DgSourceCollectionView.Refresh();
            }
        }
        private DateTime? filterDateFin = null;

        //Retour à cette page; 
        public bool RevientIci
        {
            get { return revientIci; }
            set { revientIci = value; }
        }
        public bool revientIci = false;

        #endregion

        #region Commands
        //ICommand
        public ICommand SaveCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand FilterClearCommand { get; set; }
        public ICommand ChangeView { get; set; }

        // Méthodes OnCommand
        public void OnSaveCommand()
        {
            if (Reference_ViewModel.Header.CurrentHotel == null)
            {
                MessageBox.Show($"Aucun hôtel ne vous a été assigné  ", "Impossible d'enregistrer  !");
                DgSource.Remove(CurrentDgSource);
                return;
            }

            DgSource_Save();
            DgSourceCollectionView.Refresh();
        }
        public virtual void OnAddCommand()
        {
        }
        public virtual void OnDeleteCommand()
        {
        }
        private void OnFilterClearCommand()
        {
            FilterEtat = null;
            FilterDateDebut = null;
            FilterDateFin = null;
            FilterEtage = null;
            FilterEmploye = null;
        }
        public virtual void OnChangeViewCommand()
        {
        }

        // Méthodes OnCanExecuteCommand
        private bool OnCanExecuteSaveCommand()
        {
            return CurrentDgSource != null;
        }
        private bool OnCanExecuteDeleteCommand()
        {
            return CurrentDgSource != null;
        }
        private bool OnCanExecuteChangeView()
        {
            return CurrentDgSource != null && CurrentDgSource.SaveColor != "Red";
        }

        //Filter 
        public virtual bool DgSourceFilter(object item)
        {
            return true;
        }

        #endregion

        #region Load
        public void Load_DgSource()
        {
            DgSource = new ObservableCollection<Dg_VM>(DgSource_Read());
        }
        private void Load_Etats()
        {
            Etats = new ObservableCollection<Etat_VM>(
               MakfiData.Etats.Where(x => x.Entite == EtatType).ToList());
        }
        private void Load_Employes(Guid id)
        {
            Employes = new ObservableCollection<Employe_VM>(
                  MakfiData.Employe_Read($"<employes><hotel>{id}</hotel></employes>")
                  .Select(x => new Employe_VM
                  {
                      Id = x.Id,
                      Nom = x.Nom,
                      Prenom = x.Prenom,
                      Etat = MakfiData.Etats.Where(e => e.Id == x.Etat.Id).Single(),
                      Commentaire = x.Commentaire,
                      SaveColor = "Navy"
                  }).ToList());
        }
        private void Load_Chambres(Guid id)
        {
            Chambres = new ObservableCollection<Chambre_VM>(
                  MakfiData.Chambre_Read($"<chambres><hotel>{id}</hotel></chambres>")
                  .Select(x => new Chambre_VM
                  {
                      Id = x.Id,
                      Nom = x.Nom,
                      Etat = MakfiData.Etats.Where(e => e.Id == x.Etat.Id).Single(),
                      Commentaire = x.Commentaire,
                      SaveColor = "Navy"
                  }).ToList());
        }
        private void Load_Etages(Guid id)
        {
            Etages = new ObservableCollection<Etage_VM>(
              MakfiData.GroupeChambre_Read($"<groupeChambres><hotel>{id}</hotel></groupeChambres>")
              .Select(x => new Etage_VM
              {
                  Id = x.Id,
                  Nom = x.Nom,
                  Commentaire = x.Commentaire,
                  SaveColor = "Navy"
              }).OrderBy(x => x.Nom).ToList());
        }
        public virtual IEnumerable<Dg_VM> DgSource_Read() { return null; }
        public virtual void DgSource_Save() { }
        #endregion
    }
}
