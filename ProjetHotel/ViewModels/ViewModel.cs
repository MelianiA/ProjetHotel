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
    public class ViewModel<Dg_VM> : ViewModelBase where Dg_VM : ViewModelBase
    {
        #region Propriétés
        protected EntiteEnum EtatType;
        protected SortDescription[] SortDescriptions = null;
        public ComponentEnum Components = ComponentEnum.None;
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
                if (Components.HasFlag(ComponentEnum.Etats)) Load_Etats();
                if (Components.HasFlag(ComponentEnum.Employes)) Load_Employes(Reference_ViewModel.Header.CurrentHotel.Id);
                if (Components.HasFlag(ComponentEnum.Chambres)) Load_Chambres(Reference_ViewModel.Header.CurrentHotel.Id);
                if (Components.HasFlag(ComponentEnum.Etages)) Load_Etages(Reference_ViewModel.Header.CurrentHotel.Id);
                if (Components.HasFlag(ComponentEnum.Interventions)) Load_Interventions(
                    $@"
                    <interventions>
                        <hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel>
                    </interventions>
                    ");
                Load_DgSource();
            }

            // DgSourceCollectionView
            if (DgSource != null)
            {
                DgSourceCollectionView = new ListCollectionView(DgSource);
                DgSourceCollectionView.Filter = DgSource_Filter;
                if (SortDescriptions != null) foreach (var d in SortDescriptions) DgSourceCollectionView.SortDescriptions.Add(d);
                CurrentDgSource = DgSource.FirstOrDefault();
            }
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

        // Employes
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

        // Chambres
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

        // Etages
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

        // Interventions
        public ObservableCollection<Intervention_VM> Interventions
        {
            get { return interventions; }
            set
            {
                interventions = value;
                OnPropertyChanged("Interventions");
            }
        }
        private ObservableCollection<Intervention_VM> interventions;

        // Filter
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

        // Retour à cette page
        public bool RevientIci
        {
            get { return revientIci; }
            set { revientIci = value; }
        }
        public bool revientIci = false;

        public string Title { get { return title; } set { title = value; OnPropertyChanged("Title"); } }
        private string title = "Non défini";
        #endregion

        #region Commands
        // ICommand
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
        public virtual void OnAddCommand() { }
        public virtual void OnDeleteCommand() { }
    
        private void OnFilterClearCommand()
        {
            FilterEtat = null;
            FilterDateDebut = null;
            FilterDateFin = null;
            FilterEtage = null;
            FilterEmploye = null;
        }
        public virtual void OnChangeViewCommand() { }

        // Méthodes OnCanExecuteCommand
        private bool OnCanExecuteSaveCommand() { return CurrentDgSource != null; }
        private bool OnCanExecuteDeleteCommand() { return CurrentDgSource != null; }
        private bool OnCanExecuteChangeView() { return CurrentDgSource != null && CurrentDgSource.SaveColor != "Red"; }

        //Filter 
        public virtual bool DgSource_Filter(object item) { return true; }

        #endregion

        #region Load
        public void Load_DgSource()// AM : 20201228
        {
            var items = DgSource_Read();
            if (items == null) return;
            if (dgSource == null)
                DgSource = new ObservableCollection<Dg_VM>(items);
            else
            {
                dgSource.Clear();
                foreach (var item in items) dgSource.Add(item);
            }
        }
        private void Load_Etats()// AM : 20201228
        {
            var items = MakfiData.Etats.Where(x => x.Entite == EtatType);
            if (Etats == null)
                Etats = new ObservableCollection<Etat_VM>(items);
            else
            {
                Etats.Clear();
                foreach (var item in items) Etats.Add(item);
            }
        }
        private void Load_Employes(Guid id)// AM : 20201228
        {
            var items = MakfiData
                .Employe_Read($"<employes><hotel>{id}</hotel></employes>")
                .Select(x => new Employe_VM
                {
                    Id = x.Id,
                    Nom = x.Nom,
                    Prenom = x.Prenom,
                    Etat = MakfiData.Etats.Where(e => e.Id == x.Etat.Id).Single(),
                    Commentaire = x.Commentaire,
                    SaveColor = "Navy"
                });
            if (Employes == null)
                Employes = new ObservableCollection<Employe_VM>(items);
            else
            {
                Employes.Clear();
                foreach (var item in items) Employes.Add(item);
            }
        }
        private void Load_Chambres(Guid id)// AM : 20201228
        {
            var items = MakfiData
                .Chambre_Read($"<chambres><hotel>{id}</hotel></chambres>")
                .Select(x => new Chambre_VM
                {
                    Id = x.Id,
                    Nom = x.Nom,
                    Etat = MakfiData.Etats.Where(e => e.Id == x.Etat.Id).Single(),
                    Commentaire = x.Commentaire,
                    SaveColor = "Navy"
                });
            if (Chambres == null)
                Chambres = new ObservableCollection<Chambre_VM>(items);
            else
            {
                Chambres.Clear();
                foreach (var item in items) Chambres.Add(item);
            }
        }
        private void Load_Etages(Guid id)// AM : 20201228
        {
            var items = MakfiData
                .GroupeChambre_Read($"<groupeChambres><hotel>{id}</hotel></groupeChambres>")
                .Select(x => new Etage_VM
                {
                    Id = x.Id,
                    Nom = x.Nom,
                    Commentaire = x.Commentaire,
                    SaveColor = "Navy"
                }).OrderBy(x => x.Nom).ToList();

            if (Etages == null)
                Etages = new ObservableCollection<Etage_VM>(items);
            else
            {
                Etages.Clear();
                foreach (var item in items) Etages.Add(item);
            }
        }
        public void Load_Interventions(string xml)// AM : 20201228
        {
            var items = MakfiData.Intervention_Read(xml)
              .Select(x => new Intervention_VM
              {
                  Id = x.Id,
                  Libelle = x.Libelle,
                  Etat = MakfiData.Etats.Where(e => e.Id == x.Etat).Single(),
                  Date1 = x.Date1,
                  Commentaire = x.Commentaire,
                  Model = x.Model,
                  SaveColor = "Navy"
              }).OrderBy(x => x.Libelle).ToList();


            if (Interventions == null)
                Interventions = new ObservableCollection<Intervention_VM>(items);
            else
            {
                Interventions.Clear();
                foreach (var item in items) Interventions.Add(item);
            }
        }
        #endregion

        #region DgSource
        public virtual IEnumerable<Dg_VM> DgSource_Read() { return null; }
        public virtual void DgSource_Save() { }
        #endregion
    }

    [Flags]
    public enum ComponentEnum
    {
        None = 0,
        Etats = 1,
        Employes = 2,
        Chambres = 4,
        Etages = 8,
        DateDebut = 16,
        DateFin = 32,
        Interventions = 64
    }

}
