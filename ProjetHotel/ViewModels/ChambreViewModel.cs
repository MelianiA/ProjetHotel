using Makrisoft.Makfi.Dal;
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
    public class ChambreViewModel: ViewModelBase
    {
        #region Constructeur
        public ChambreViewModel()
        {
            // Icommand
            ChambreModifiedSaveCommand = new RelayCommand(p => OnSaveCommand(), p => OnCanExecuteSaveCommand());
            ChambreSelectedAddCommand = new RelayCommand(p => OnAddCommand(), p => true);
            ChambreSelectedDeleteCommand = new RelayCommand(p => OnDeleteCommand(), p => OnCanExecuteDeleteCommand());
            FilterClearCommand = new RelayCommand(p => OnFilterClearCommand(), p => OnCanExecuteFilterClearCommand());

            // ListeView
            Load_Etat();
            Load_Chambres();
            Load_EtatChambre();
        }
        #endregion

        #region Binding
        //Chambre
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
        public ListCollectionView ChambreCollectionView
        {
            get { return chambreCollectionView; }
            set { chambreCollectionView = value; OnPropertyChanged("ChambreCollectionView"); }
        }
        private ListCollectionView chambreCollectionView;
        public Chambre_VM CurrentChambre
        {
            get
            {

                return currentChambre;

            }
            set
            {
                currentChambre = value;
                OnPropertyChanged("CurrentChambre");
            }
        }
        private Chambre_VM currentChambre;

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

        //EtatChambre
        public ObservableCollection<Etat_VM> EtatChambre
        {
            get { return etatChambre; }
            set
            {
                etatChambre = value;
                OnPropertyChanged("EtatChambre");
            }
        }
        private ObservableCollection<Etat_VM> etatChambre;
        public ListCollectionView EtatChambreCollectionView
        {
            get { return etatChambreCollectionView; }
            set { etatChambreCollectionView = value; OnPropertyChanged("EtatChambreCollectionView"); }
        }
        private ListCollectionView etatChambreCollectionView;

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
                ChambreCollectionView.Filter = FilterChambres;
                OnPropertyChanged("CurrentFilter");
            }
        }
        private Etat_VM currentFilter;

        //
        public ListCollectionView HotelsCollectionView
        {
            get { return hotelsCollectionView; }
            set { hotelsCollectionView = value; OnPropertyChanged("HotelCollectionView"); }
        }
        private ListCollectionView hotelsCollectionView;

        #endregion

        #region Commands
        //ICommand
        public ICommand ChambreModifiedSaveCommand { get; set; }
        public ICommand ChambreSelectedAddCommand { get; set; }
        public ICommand ChambreSelectedDeleteCommand { get; set; }
        public ICommand FilterClearCommand { get; set; }

        // Méthodes OnCommand
        private void OnSaveCommand()
        {
            if (CurrentChambre.Nom == null || CurrentChambre.Etat == null || CurrentChambre.Hotel == null)
            {
                MessageBox.Show($"Impossible de sauvgarder cette chambre !", "Remarque !");
                return;
            }
            Guid? monID = null;
            if (CurrentChambre.Id != default) monID = CurrentChambre.Id;
            var param = $@"
                    <chambre>
                        <id>{monID}</id>
                        <nom>{CurrentChambre.Nom}</nom>
                         <etat>{CurrentChambre.Etat.Id}</etat>
                        <commentaire>{CurrentChambre.Commentaire}</commentaire>    
						<hotel>{CurrentChambre.Hotel.Id}</hotel>
                    </chambre>";
            var ids = MakfiData.Chambre_Save(param);
            if (ids.Count == 0) throw new Exception("Rien n'a été sauvgardé ! ");
            CurrentChambre.SaveColor = "Navy";
            Chambres.Clear();
            Load_Chambres();
            CurrentChambre = Chambres.Where(u => u.Id == ids[0].Id).SingleOrDefault();
        }
       
        private void OnAddCommand()
        {
            CurrentChambre = new Chambre_VM { Nom = "(A définir)" };
            Chambres.Add(CurrentChambre);
            ChambreCollectionView.Refresh();
        }
        private void OnDeleteCommand()
        {
            var canDeletes = MakfiData.Chambre_CanDelete($"<chambre><id>{CurrentChambre.Id}</id></chambre>");
            if (canDeletes.Count() == 0)
            {
                var param = MakfiData.Chambre_Delete($"<chambre><id>{CurrentChambre.Id}</id></chambre>");
                if (param)  Chambres.Remove(CurrentChambre);
            }
            else
            {
                MessageBox.Show($" Suppression impossible de la chambre : {CurrentChambre.Nom }", "Remarque !");
            }
        }
        private void OnFilterClearCommand()
        {
            CurrentFilter = null;
        }

        // Méthodes OnCanExecuteCommand
        private bool OnCanExecuteSaveCommand()
        {
            if (CurrentChambre != null) return true;
            else return false;
        }
        private bool OnCanExecuteDeleteCommand()
        {
            if (CurrentChambre != null) return true;
            else return false;
        }
        private bool OnCanExecuteFilterClearCommand()
        {
            if (CurrentFilter != null) return true;
            else return false;
        }

        // Divers
        public bool FilterChambres(object item)
        {
            if (item is Chambre_VM Chambre)
            {
                if (CurrentFilter == null) return true;
                if (CurrentFilter.Libelle == "Disponible")
                    return (Chambre.Etat.Libelle == "Disponible");
                if (CurrentFilter.Libelle == "Non disponible")
                    return (Chambre.Etat.Libelle == "Non disponible");
                if (CurrentFilter.Libelle == "Arrêt maladie")
                    return (Chambre.Etat.Libelle == "Arrêt maladie");
            }
            return true;
        }

        #endregion

        #region Load
        private void Load_Chambres()
        {
            Chambres = new ObservableCollection<Chambre_VM>(
              MakfiData.Chambre_Read()
              .Select(x => new Chambre_VM
              {
                  Id = x.Id,
                  Nom = x.Nom,
                  Etat = EtatList.Where(e => e.Id == x.Etat.Id).SingleOrDefault(),
                  Commentaire = x.Commentaire,
                  Hotel = Reference_ViewModel.Hotel.Hotels.Where(h => h.Id == x.Hotel.Id).SingleOrDefault(),
                  SaveColor = "Navy"
              }).OrderBy(x => x.Nom).ToList());
            ChambreCollectionView = new ListCollectionView(Chambres);
            ChambreCollectionView.Refresh();
            HotelsCollectionView = new ListCollectionView(Reference_ViewModel.Hotel.Hotels);
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
        private void Load_EtatChambre()
        {
            EtatChambre = new ObservableCollection<Etat_VM>(
               EtatList.Where(x => x.Entite == EntiteEnum.Chambre).ToList());
            EtatChambreCollectionView = new ListCollectionView(EtatChambre);
            EtatChambreCollectionView.Refresh();
            CurrentFilter = null;
        }

        #endregion

    }
}
