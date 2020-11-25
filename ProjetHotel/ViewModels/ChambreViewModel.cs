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
    public class ChambreViewModel : ViewModelBase
    {
        #region Constructeur
        public ChambreViewModel()
        {
            // Icommand
            ChambreModifiedSaveCommand = new RelayCommand(p => OnSaveCommand(), p => OnCanExecuteSaveCommand());
            ChambreSelectedAddCommand = new RelayCommand(p => OnAddCommand(), p => true);
            ChambreSelectedDeleteCommand = new RelayCommand(p => OnDeleteCommand(), p => OnCanExecuteDeleteCommand());
            FilterClearCommand = new RelayCommand(p => OnFilterEtatClearCommand(), p => OnCanExecuteFilterEtatClearCommand());
            ChambreGroupeViewCommand = new RelayCommand(p => OnChambreGroupeViewCommand());
            // ListeView
            if (Reference_ViewModel.Header.CurrentHotel != null)
            {
                Load_Etat();
                Load_EtatChambre();
                Load_Chambres();
            }


        }

        #endregion

        #region Binding
        //Chambre
        public ListCollectionView ChambreCollectionView
        {
            get { return chambreCollectionView; }
            set { chambreCollectionView = value; OnPropertyChanged("ChambreCollectionView"); }
        }
        private ListCollectionView chambreCollectionView;

        //GroupeChambre
        public ObservableCollection<GroupeChambre_VM> GroupeChambre
        {
            get { return groupeChambre; }
            set
            {
                groupeChambre = value;
                OnPropertyChanged("GroupeChambre");

            }
        }
        private ObservableCollection<GroupeChambre_VM> groupeChambre;
        public ListCollectionView GroupeChambreCollectionView
        {
            get { return groupeChambreCollectionView; }
            set { groupeChambreCollectionView = value; OnPropertyChanged("GroupeChambreCollectionView"); }
        }
        private ListCollectionView groupeChambreCollectionView;

        //ChambreGroupeChambre
        public ObservableCollection<ChambreGroupeChambre_VM> ChambreGroupeChambre
        {
            get { return chambreGroupeChambre; }
            set
            {
                chambreGroupeChambre = value;
                OnPropertyChanged("ChambreGroupeChambre");

            }
        }
        private ObservableCollection<ChambreGroupeChambre_VM> chambreGroupeChambre;
        public ChambreGroupeChambre_VM CurrentChambreGChambre
        {
            get
            {

                return currentChambreGChambre;

            }
            set
            {
                currentChambreGChambre = value;
                OnPropertyChanged("CurrentChambreGChambre");
            }
        }
        private ChambreGroupeChambre_VM currentChambreGChambre;
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
        public Etat_VM CurrentFilterEtat
        {
            get
            {
                return currentFilterEtat;
            }
            set
            {
                currentFilterEtat = value;
                if (ChambreCollectionView != null)
                    ChambreCollectionView.Filter = FilterChambres;
                OnPropertyChanged("CurrentFilterEtat");
            }
        }
        private Etat_VM currentFilterEtat;

        public GroupeChambre_VM CurrentFilterGroupe
        {
            get
            {
                return currentFilterGroupe;
            }
            set
            {
                currentFilterGroupe = value;
                if (ChambreCollectionView != null)
                    ChambreCollectionView.Filter = FilterChambres;
                OnPropertyChanged("CurrentFilterGroupe");
            }
        }
        private GroupeChambre_VM currentFilterGroupe;


        #endregion

        #region Commands
        //ICommand
        public ICommand ChambreModifiedSaveCommand { get; set; }
        public ICommand ChambreSelectedAddCommand { get; set; }
        public ICommand ChambreSelectedDeleteCommand { get; set; }
        public ICommand FilterClearCommand { get; set; }
        public ICommand ChambreGroupeViewCommand { get; set; }

        // Méthodes OnCommand
        private void OnSaveCommand()
        {
            if (Reference_ViewModel.Header.CurrentHotel == null)
            {
                MessageBox.Show($"Aucun hôtel ne vous a été assigné  ", "Impossible d'enregistrer  !");
                ChambreGroupeChambre.Remove(CurrentChambreGChambre);
                return;
            }
            if (CurrentChambreGChambre.Nom == null || CurrentChambreGChambre.Etat == null)
            {
                MessageBox.Show($"Impossible de sauvgarder cette chambre !", "Remarque !");
                return;
            }
            Guid? monID = null;
            if (CurrentChambreGChambre.Id != default) monID = CurrentChambreGChambre.Id;
            var param = $@"
                    <chambre>
                        <id>{monID}</id>
                        <nom>{CurrentChambreGChambre.Nom}</nom>
                        <etat>{CurrentChambreGChambre.Etat.Id}</etat>
                        <commentaire>{CurrentChambreGChambre.Commentaire}</commentaire>    
						<hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel>
                     </chambre>";
            var ids = MakfiData.Chambre_Save(param);
            if (ids.Count == 0) throw new Exception("Rien n'a été sauvgardé ! ");
            CurrentChambreGChambre.Id = ids[0].Id;
            CurrentChambreGChambre.SaveColor = "Navy";
            ChambreCollectionView.Refresh();
        }
        private void OnAddCommand()
        {
            CurrentChambreGChambre = new ChambreGroupeChambre_VM { Nom = "( A définir !)", Etat = EtatChambre.Where(e => e.Libelle == "Pas encore fait").SingleOrDefault() };
            ChambreGroupeChambre.Add(CurrentChambreGChambre);
            ChambreCollectionView.Refresh();
        }
        private void OnDeleteCommand()
        {
            var canDeletes = MakfiData.Chambre_CanDelete($"<chambre><id>{CurrentChambreGChambre.Id}</id></chambre>");
            if (canDeletes.Count() == 0)
            {
                var param = MakfiData.Chambre_Delete($"<chambre><id>{CurrentChambreGChambre.Id}</id></chambre>");
                if (param) ChambreGroupeChambre.Remove(CurrentChambreGChambre);
            }
            else
            {
                MessageBox.Show($" Suppression impossible de la chambre : {CurrentChambreGChambre.Nom }", "Remarque !");
            }
        }

        private void OnChambreGroupeViewCommand()
        {
            Reference_ViewModel.ChambreGroupe.Load_AllChambres();
            Reference_ViewModel.ChambreGroupe.Load_GroupeChambres();
            Reference_ViewModel.Main.ViewSelected = ViewEnum.ChambreGroupe;
        }
        private void OnFilterEtatClearCommand()
        {
            CurrentFilterEtat = null;
            CurrentFilterGroupe = null;
        }


        // Méthodes OnCanExecuteCommand
        private bool OnCanExecuteSaveCommand()
        {
            if (CurrentChambreGChambre != null) return true;
            else return false;
        }
        private bool OnCanExecuteDeleteCommand()
        {
            if (CurrentChambreGChambre != null) return true;
            else return false;
        }
        private bool OnCanExecuteFilterEtatClearCommand()
        {
            if (CurrentFilterEtat != null || CurrentFilterGroupe != null) return true;
            else return false;
        }

        // Divers
        public bool FilterChambres(object item)
        {
            if (CurrentFilterGroupe != null && CurrentFilterEtat != null)
            {
                if (item is ChambreGroupeChambre_VM chambre && chambre.GroupeChambre != null)
                {
                    var tab = chambre.GroupeChambre.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                    return (GroupeChambre.Any(gc => tab.Contains(CurrentFilterGroupe.Nom)) && EtatChambre.Any(e => chambre.Etat.Libelle == CurrentFilterEtat.Libelle));
                }
            }
            if (CurrentFilterGroupe != null)
            {
                if (item is ChambreGroupeChambre_VM chambre && chambre.GroupeChambre != null)
                {
                    var tab = chambre.GroupeChambre.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                    if (tab.Length > 0)
                        return GroupeChambre.Any(gc => tab.Contains(CurrentFilterGroupe.Nom));
                    return false;
                }
                return false;
            }
            if (CurrentFilterEtat != null)
            {
                if (item is ChambreGroupeChambre_VM chambre)
                {
                    return EtatChambre.Any(e => chambre.Etat.Libelle == CurrentFilterEtat.Libelle);
                }
                return false;
            }
            return true;

        }

        #endregion

        #region Load
        public void Load_Chambres()
        {
            //Load_Chambres
            Guid monId = default;
            if (Reference_ViewModel.Header.CurrentHotel != null)
                monId = Reference_ViewModel.Header.CurrentHotel.Id;
            //Load_ChambreGroupeChambre
            ChambreGroupeChambre = new ObservableCollection<ChambreGroupeChambre_VM>(
              MakfiData.ChambreGroupeChambre_Read($"<chambreGroupeChambre><hotel>{monId}</hotel></chambreGroupeChambre>")
              .Select(x => new ChambreGroupeChambre_VM
              {
                  Id = x.Id,
                  Nom = x.Nom,
                  Etat = EtatChambre.Where(e => e.Id == x.Etat).SingleOrDefault(),
                  Commentaire = x.Commentaire,
                  GroupeChambre = x.GroupeChambre,
                  SaveColor = "Navy"
              }).OrderBy(x => x.Nom).ToList());
            ChambreCollectionView = new ListCollectionView(ChambreGroupeChambre);
            ChambreCollectionView.Refresh();

            //Load_GroupeChambre
            Guid idHotel = default;
            if (Reference_ViewModel.Header.CurrentHotel != null) idHotel = Reference_ViewModel.Header.CurrentHotel.Id;
            GroupeChambre = new ObservableCollection<GroupeChambre_VM>(
             MakfiData.GroupeChambre_Read($"<groupeChambre><hotel>{idHotel}</hotel></groupeChambre>")
             .Select(x => new GroupeChambre_VM
             {
                 Id = x.Id,
                 Nom = x.Nom,
                 Commentaire = x.Commentaire
             }).ToList());
            GroupeChambreCollectionView = new ListCollectionView(GroupeChambre);
            GroupeChambreCollectionView.Refresh();
            CurrentFilterEtat = null;
            CurrentFilterGroupe = null;
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
        }

        #endregion

    }
}
