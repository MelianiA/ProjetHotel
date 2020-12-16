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
    public class InterventionDetailViewModel : ViewModelBase
    {
        #region Constructeur
        public InterventionDetailViewModel()
        {
            // Icommand
            InterventionDetailSelectedAddCommand = new RelayCommand(p => OnAddCommand(), p => true);
            InterventionDetailSelectedDeleteCommand = new RelayCommand(p => OnSupprimeCommand(), p => OnCanExecuteSupprimeCommand());
            InterventionDetailModifiedSaveCommand = new RelayCommand(p => OnSaveCommand(), p => OnCanExecuteSaveCommand());
            AddCommand = new RelayCommand(p => OnAddCommandSimple(), p => true);
            DeleteCommand = new RelayCommand(p => OnDeleteCommandSimple(), p => OnCanExecuteDeleteCommandSimple());
            EnregistrerTout = new RelayCommand(p => OnEnregistrerTout(), p => OnCanExecuteEnregistrerTout());
            FilterClearCommand = new RelayCommand(p => OnFilterClearCommand(), p => OnCanExecuteFilterClearCommand());

        }

        #endregion

        #region Binding

        //Intervention
        public Intervention_VM CurrentIntervention
        {
            get { return currentIntervention; }
            set
            {
                currentIntervention = value;
                OnPropertyChanged("CurrentIntervention");
            }
        }
        private Intervention_VM currentIntervention;

        //IsEnabled
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }
        private bool isEnabled;

        //InterventionDetails
        public ObservableCollection<InterventionDetail_VM> InterventionDetails
        {
            get { return interventionDetails; }
            set { interventionDetails = value; OnPropertyChanged("InterventionDetails"); }
        }
        private ObservableCollection<InterventionDetail_VM> interventionDetails;
        public InterventionDetail_VM CurrentInterventionDetail
        {
            get { return currentInterventionDetail; }
            set
            {
                currentInterventionDetail = value;
                if (currentInterventionDetail == null) IsEnabled = false;
                else IsEnabled = true;
                OnPropertyChanged("CurrentInterventionDetail");
            }
        }
        private InterventionDetail_VM currentInterventionDetail;
        public ListCollectionView InterventionDetailsCollectionView
        {
            get { return interventionDetailCollectionView; }
            set { interventionDetailCollectionView = value; OnPropertyChanged("InterventionDetailsCollectionView"); }
        }
        private ListCollectionView interventionDetailCollectionView;

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

        //EtatIntervention
        public ObservableCollection<Etat_VM> EtatIntervention
        {
            get { return etatIntervention; }
            set
            {
                etatIntervention = value;
                OnPropertyChanged("EtatIntervention");
            }
        }
        private ObservableCollection<Etat_VM> etatIntervention;
        public ListCollectionView EtatInterventionCollectionView
        {
            get { return etatInterventionCollectionView; }
            set { etatInterventionCollectionView = value; OnPropertyChanged("EtatInterventionCollectionView"); }
        }
        private ListCollectionView etatInterventionCollectionView;

        //Employe 
        public ObservableCollection<Employe_VM> EmployeIntervention
        {
            get { return employeIntervention; }
            set
            {
                employeIntervention = value;
                OnPropertyChanged("EmployeIntervention");
            }
        }
        private ObservableCollection<Employe_VM> employeIntervention;
        public ListCollectionView EmployeInterventionCollectionView
        {
            get { return employeInterventionCollectionView; }
            set
            {
                employeInterventionCollectionView = value;
                OnPropertyChanged("EmployeInterventionCollectionView");
            }
        }
        private ListCollectionView employeInterventionCollectionView;

        //Chambre
        public ObservableCollection<Chambre_VM> ChambreIntervention
        {
            get { return chambreIntervention; }
            set
            {
                chambreIntervention = value;
                OnPropertyChanged("ChambreIntervention");
            }
        }
        private ObservableCollection<Chambre_VM> chambreIntervention;
        public ListCollectionView ChambreInterventionCollectionView
        {
            get { return chambreInterventionCollectionView; }
            set
            {
                chambreInterventionCollectionView = value;
                OnPropertyChanged("ChambreInterventionCollectionView");
            }
        }
        private ListCollectionView chambreInterventionCollectionView;

        //Filter
        public GroupeChambre_VM CurrentGroupeChambres
        {
            get { return currentGroupeChambres; }
            set
            {
                currentGroupeChambres = value;
                if (InterventionDetailsCollectionView != null)
                    InterventionDetailsCollectionView.Filter = FilterInterventionDetails;
                OnPropertyChanged("CurrentGroupeChambres");
            }
        }
        private GroupeChambre_VM currentGroupeChambres;

        public Etat_VM CurrentEtatIntervention
        {
            get { return currentEtatIntervention; }
            set
            {
                currentEtatIntervention = value;
                if (InterventionDetailsCollectionView != null)
                    InterventionDetailsCollectionView.Filter = FilterInterventionDetails;
                OnPropertyChanged("CurrentEtatIntervention");
            }
        }
        private Etat_VM currentEtatIntervention;

        public Employe_VM CurentEmployeIntervention
        {
            get { return curentEmployeIntervention; }
            set
            {
                curentEmployeIntervention = value;
                if (InterventionDetailsCollectionView != null)
                    InterventionDetailsCollectionView.Filter = FilterInterventionDetails;
                OnPropertyChanged("CurentEmployeIntervention");
            }
        }
        private Employe_VM curentEmployeIntervention;


        //GroupeChambre
        public ObservableCollection<GroupeChambre_VM> GroupeChambres
        {
            get { return groupeChambres; }
            set
            {
                groupeChambres = value;
                OnPropertyChanged("GroupeChambres");
            }
        }
        private ObservableCollection<GroupeChambre_VM> groupeChambres;


        #endregion

        #region Commands
        //ICommand
        public ICommand InterventionDetailSelectedAddCommand { get; set; }
        public ICommand InterventionDetailSelectedDeleteCommand { get; set; }
        public ICommand InterventionDetailModifiedSaveCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand EnregistrerTout { get; set; }
        public ICommand FilterClearCommand { get; set; }

        // Méthodes OnCommand
        private void OnAddCommand()
        {
            Reference_ViewModel.Main.ViewSelected = ViewEnum.InterventionAjouter;
            Reference_ViewModel.InterventionAjouter.Load_InterventionDetailsAjouter();
            Reference_ViewModel.InterventionAjouter.Annuler = true;
            Reference_ViewModel.InterventionAjouter.Interventions.Remove(CurrentIntervention);

        }
        private void OnSupprimeCommand()
        {
            Reference_ViewModel.InterventionSupprimer.Load_InterventionDetailsAjouter();
            Reference_ViewModel.InterventionSupprimer.Annuler = true;
            Reference_ViewModel.Main.ViewSelected = ViewEnum.InterventionSupprimer;
        }
        private void OnSaveCommand()
        {
            Guid? monID = null;
            if (currentInterventionDetail.Id != default) monID = currentInterventionDetail.Id;
            var param = $@"
                    <interventionDetail>
                        <id>{monID}</id>
                        <employeAffecte>{currentInterventionDetail.Employe.Id}</employeAffecte>
                        <commentaire>{currentInterventionDetail.Commentaire}</commentaire>    
						<chambreAffectee>{currentInterventionDetail.Chambre.Id}</chambreAffectee>
                        <intervention>{CurrentIntervention.Id}</intervention>    
                        <etat>{currentInterventionDetail.Etat.Id}</etat> 
                     </interventionDetail>";
            var ids = MakfiData.InterventionDetail_Save(param);
            if (ids.Count == 0) throw new Exception("Rien n'a été sauvgardé ! ");
            currentInterventionDetail.Id = ids[0].Id;
            currentInterventionDetail.SaveColor = "Navy";
            Reference_ViewModel.Intervention.CurrentIntervention.Etat = GetSommeEtats();
            Reference_ViewModel.Intervention.CurrentIntervention.SaveColor = "Navy";

        }
        private void OnAddCommandSimple()
        {
            CurrentInterventionDetail = new InterventionDetail_VM
            {
                Etat = EtatIntervention.Where(e => e.Libelle == "None").SingleOrDefault()
            };
            InterventionDetails.Add(CurrentInterventionDetail);
        }
        private void OnDeleteCommandSimple()
        {
            var param = MakfiData.InterventionDetails_Delete($"<interventionDetails><id>{CurrentInterventionDetail.Id}</id></interventionDetails>");
            if (param) InterventionDetails.Remove(CurrentInterventionDetail);
        }
        private void OnEnregistrerTout()
        {
            List<InterventionDetail_VM> elementAsupp = new List<InterventionDetail_VM>();
            var IntDetails = InterventionDetails.Where(i => i.SaveColor == "Red");
            foreach (var item in IntDetails)
            {
                Guid? monID = null; var param = "";
                if (item.Id != default) monID = item.Id;
                if (item.Employe != null && item.Chambre != null)
                {
                    param = $@"
                    <interventionDetail>
                        <id>{monID}</id>
                        <employeAffecte>{item.Employe.Id}</employeAffecte>
                        <commentaire>{item.Commentaire}</commentaire>    
						<chambreAffectee>{item.Chambre.Id}</chambreAffectee>
                        <intervention>{CurrentIntervention.Id}</intervention>    
                        <etat>{item.Etat.Id}</etat> 
                     </interventionDetail>";
                    var ids = MakfiData.InterventionDetail_Save(param);
                    if (ids.Count == 0) throw new Exception("Rien n'a été sauvgardé ! ");
                    item.Id = ids[0].Id;
                    item.SaveColor = "Navy";
                    Reference_ViewModel.Intervention.CurrentIntervention.Etat = GetSommeEtats();
                    Reference_ViewModel.Intervention.CurrentIntervention.SaveColor = "Navy";
                }
                else
                    elementAsupp.Add(item);
            }
            foreach (var item in elementAsupp)
                InterventionDetails.Remove(item);
        }
        private void OnFilterClearCommand()
        {
            CurrentGroupeChambres = null;
            CurrentEtatIntervention = null;
            CurentEmployeIntervention = null;
        }


        // Méthodes OnCanExecuteCommand
        private bool OnCanExecuteSaveCommand()
        {
            return CurrentInterventionDetail != null;
        }
        private bool OnCanExecuteDeleteCommandSimple()
        {
            return CurrentInterventionDetail != null;
        }
        private bool OnCanExecuteEnregistrerTout()
        {
            return InterventionDetails.Any(i => i.SaveColor == "Red");
        }
        private bool OnCanExecuteSupprimeCommand()
        {
            return InterventionDetails.Count > 0;
        }
        private bool OnCanExecuteFilterClearCommand()
        {
            return (CurrentGroupeChambres != null || CurrentEtatIntervention != null || CurentEmployeIntervention != null);
        }

        //Filter 
        public bool FilterInterventionDetails(object item)
        {
            if (CurrentEtatIntervention != null && CurentEmployeIntervention != null && CurrentGroupeChambres != null)
            {
                GetChambresGroupChambre();
                if (item is InterventionDetail_VM interventionDetail)
                    return EmployeIntervention.Any(e => interventionDetail.Employe.Id == CurentEmployeIntervention.Id)
                        && EtatIntervention.Any(e => interventionDetail.Etat.Id == CurrentEtatIntervention.Id)
                        && CurrentGroupeChambres.ChambreCurrentGroupe.Any(e => interventionDetail.Chambre.Id == e.IdDelaChambre);
                return false;
            }
            if (CurrentGroupeChambres != null && CurrentEtatIntervention != null)
            {
                GetChambresGroupChambre();
                if (item is InterventionDetail_VM interventionDetail)
                    return EtatIntervention.Any(e => interventionDetail.Etat.Id == CurrentEtatIntervention.Id)
                        && CurrentGroupeChambres.ChambreCurrentGroupe.Any(e => interventionDetail.Chambre.Id == e.IdDelaChambre);
                return false;
            }
            if (CurrentGroupeChambres != null && CurentEmployeIntervention != null)
            {
                GetChambresGroupChambre();
                if (item is InterventionDetail_VM interventionDetail)
                    return EmployeIntervention.Any(e => interventionDetail.Employe.Id == CurentEmployeIntervention.Id)
                        && CurrentGroupeChambres.ChambreCurrentGroupe.Any(e => interventionDetail.Chambre.Id == e.IdDelaChambre);
                return false;
            }

            if (CurrentEtatIntervention != null && CurentEmployeIntervention != null)
            {
                if (item is InterventionDetail_VM interventionDetail)
                    return EmployeIntervention.Any(e => interventionDetail.Employe.Id == CurentEmployeIntervention.Id)
                        && EtatIntervention.Any(e => interventionDetail.Etat.Id == CurrentEtatIntervention.Id);
                return false;
            }

          
            if (CurrentEtatIntervention != null)
            {
                if (item is InterventionDetail_VM interventionDetail)
                    return EtatIntervention.Any(e => interventionDetail.Etat.Id == CurrentEtatIntervention.Id);
                return false;
            }
            if (CurentEmployeIntervention != null)
            {
                if (item is InterventionDetail_VM interventionDetail)
                    return EmployeIntervention.Any(e => interventionDetail.Employe.Id == CurentEmployeIntervention.Id);
                return false;
            }
            if (CurrentGroupeChambres != null)
            {
                GetChambresGroupChambre();
                if (item is InterventionDetail_VM interventionDetail)
                    return CurrentGroupeChambres.ChambreCurrentGroupe.Any(e => interventionDetail.Chambre.Id == e.IdDelaChambre);
                return false;
            }
            return true;
        }

        //Autres
        public Etat_VM GetSommeEtats()
        {
            if (InterventionDetails.Count == 0)
                return EtatIntervention.Where(e => e.Libelle == "None").SingleOrDefault();

            if (InterventionDetails.All(i => i.Etat.Libelle == "Fait"))
                return EtatIntervention.Where(e => e.Libelle == "Fait").SingleOrDefault();

            if (InterventionDetails.Any(i => i.Etat.Libelle == "Incident"))
                return EtatIntervention.Where(e => e.Libelle == "Incident").SingleOrDefault();

            if (InterventionDetails.Any(i => i.Etat.Libelle == "En cours")
                || InterventionDetails.Any(i => i.Etat.Libelle == "Fait"))
                return EtatIntervention.Where(e => e.Libelle == "En cours").SingleOrDefault();

            else
                return EtatIntervention.Where(e => e.Libelle == "None").SingleOrDefault();
        }
        public void GetChambresGroupChambre()
        {
            if (CurrentGroupeChambres != null)
            {
                CurrentGroupeChambres.ChambreCurrentGroupe = new ObservableCollection<ChambreByGroupe_VM>(
                    Reference_ViewModel.InterventionAjouter.AllChambres.Where(c => c.GroupeChambre == CurrentGroupeChambres.Id)
                    );
            }
        }
        #endregion

        #region Load
        public void Load_InterventionDetail()
        {
            //Chargement des etats 
            Load_Etat();
            //Employe
            EmployeIntervention = Reference_ViewModel.Employe.AllEmployes;
            EmployeInterventionCollectionView = new ListCollectionView(EmployeIntervention);
            //chambre
            ChambreIntervention = new ObservableCollection<Chambre_VM>(
                Reference_ViewModel.Chambre.ChambreGroupeChambre.Select(c => new Chambre_VM { Id = c.Id, Nom = c.Nom }).ToList());
            ChambreInterventionCollectionView = new ListCollectionView(ChambreIntervention);
            //GroupeChambre
            GroupeChambres = Reference_ViewModel.ChambreGroupe.GroupeChambres;
            // InterventionDetails
            if (InterventionDetails != null) InterventionDetails.Clear();
            if (Reference_ViewModel.Header.CurrentHotel == null)
            {
                MessageBox.Show($"Aucun hôtel ne vous a été assigné  ", "Impossible d'enregistrer  !");
                return;
            }
            Guid monId = default;
            if (Reference_ViewModel.Intervention.CurrentIntervention != null)
                monId = Reference_ViewModel.Intervention.CurrentIntervention.Id;

            InterventionDetails = new ObservableCollection<InterventionDetail_VM>(
               MakfiData.InterventionDetail_Read($"<interventionDetail><intervention>{monId}</intervention></interventionDetail>")
               .Select(x => new InterventionDetail_VM
               {
                   Id = x.Id,
                   Employe = EmployeIntervention.Where(e => e.Id == x.Employe).SingleOrDefault(),
                   Chambre = ChambreIntervention.Where(c => c.Id == x.Chambre).SingleOrDefault(),
                   Libelle = Reference_ViewModel.Intervention.CurrentIntervention.Libelle,
                   Commentaire = x.Commentaire,
                   Etat = EtatIntervention.Where(e => e.Id == x.Etat).SingleOrDefault(),
                   SaveColor = "Navy"
               }).OrderBy(x => x.Libelle).ToList());
            InterventionDetailsCollectionView = new ListCollectionView(InterventionDetails);
            InterventionDetailsCollectionView.Refresh();
            CurrentInterventionDetail = InterventionDetails.Count > 0 ? InterventionDetails[0] : null;
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
            EtatIntervention = new ObservableCollection<Etat_VM>(
               EtatList.Where(x => x.Entite == EntiteEnum.Intervention).ToList());
            EtatInterventionCollectionView = new ListCollectionView(EtatIntervention);
            EtatInterventionCollectionView.Refresh();
        }

        #endregion

    }
}
