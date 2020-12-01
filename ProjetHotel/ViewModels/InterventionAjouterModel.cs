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
    public class InterventionAjouterModel : ViewModelBase
    {
        #region Constructeur
        public InterventionAjouterModel()
        {
            // Icommand
            InterventionDetailSelectedAddCommand = new RelayCommand(p => OnAddCommand(), p => true);
            InterventionDetailSelectedDeleteCommand = new RelayCommand(p => OnSupprimeCommand(), p => true);
            // ListeView
            Load_Etat();

            ////Autres
            EmployeIntervention = Reference_ViewModel.Employe.AllEmployes;
            EmployeInterventionCollectionView = new ListCollectionView(EmployeIntervention);

            ChambreIntervention = new ObservableCollection<Chambre_VM>(Reference_ViewModel.Chambre.ChambreGroupeChambre.Select(c => new Chambre_VM { Id = c.Id, Nom = c.Nom }).ToList());
            ChambreInterventionCollectionView = new ListCollectionView(ChambreIntervention);
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

        //InterventionDetails
        public ObservableCollection<InterventionDetail_VM> InterventionDetails
        {
            get { return interventionDetails; }
            set
            {
                interventionDetails = value;
                OnPropertyChanged("InterventionDetails");
            }
        }
        private ObservableCollection<InterventionDetail_VM> interventionDetails;
        public InterventionDetail_VM CurrentInterventionDetail
        {
            get { return currentInterventionDetail; }
            set
            {
                currentInterventionDetail = value;
                OnPropertyChanged("CurrentInterventionDetail");
            }
        }
        private InterventionDetail_VM currentInterventionDetail;
        public ListCollectionView InterventionDetailCollectionView
        {
            get { return interventionDetailCollectionView; }
            set
            {
                interventionDetailCollectionView = value;
                OnPropertyChanged("InterventionDetailCollectionView");
            }
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
            set
            {
                etatInterventionCollectionView = value;
                OnPropertyChanged("EtatInterventionCollectionView");
            }
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

        //GroupeChambres
        public ListCollectionView GroupeChambreCollectionView
        {
            get { return groupeChambreCollectionView; }
            set
            {
                groupeChambreCollectionView = value;
                OnPropertyChanged("GroupeChambreCollectionView");
            }
        }
        private ListCollectionView groupeChambreCollectionView;


        #endregion

        #region Commands
        //ICommand
        public ICommand InterventionDetailSelectedAddCommand { get; set; }
        public ICommand InterventionDetailSelectedDeleteCommand { get; set; }

        // Méthodes OnCommand
        private void OnAddCommand()
        {
            Reference_ViewModel.Main.ViewSelected = ViewEnum.InterventionAjouter;
        }

        private void OnSupprimeCommand()
        {
            Reference_ViewModel.Main.ViewSelected = ViewEnum.InterventionSupprimer;
        }
        // Méthodes OnCanExecuteCommand

        //Filter 


        #endregion

        #region Load
        public void Load_InterventionDetail()
        {
            if (Reference_ViewModel.Header.CurrentHotel == null)
            {
                if (InterventionDetails != null) InterventionDetails.Clear();
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
                   Employe = Reference_ViewModel.Employe.AllEmployes.Where(e => e.Id == x.Employe).SingleOrDefault(),
                   Chambre = Reference_ViewModel.Chambre.ChambreGroupeChambre.Where(c => c.Id == x.Chambre)
                   .Select(b => new Chambre_VM { Id = b.Id, Commentaire = b.Commentaire, Etat = b.Etat, Nom = b.Nom }).SingleOrDefault(),
                   Etat = EtatIntervention.Where(e => e.Id == x.Etat).SingleOrDefault(),
                   Libelle = Reference_ViewModel.Intervention.CurrentIntervention.Libelle,
                   Commentaire = x.Commentaire
               }).OrderBy(x => x.Libelle).ToList());
            InterventionDetailCollectionView = new ListCollectionView(InterventionDetails);
            InterventionDetailCollectionView.Refresh();
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
