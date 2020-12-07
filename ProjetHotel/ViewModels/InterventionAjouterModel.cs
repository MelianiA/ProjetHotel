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
            Add = new RelayCommand(p => OnAddCommand(), p => OnCanExcuteAddCommand());

            // Load
            Load_Chambres();

            ////Autres
            if (Reference_ViewModel.Employe.AllEmployes != null)
            {
                EmployeIntervention = Reference_ViewModel.Employe.AllEmployes;
                EmployeInterventionCollectionView = new ListCollectionView(EmployeIntervention);
            }
            if (Reference_ViewModel.Chambre.ChambreGroupeChambre != null)
            {
                ChambreIntervention = new ObservableCollection<Chambre_VM>(Reference_ViewModel.Chambre.ChambreGroupeChambre.Select(c => new Chambre_VM { Id = c.Id, Nom = c.Nom }).ToList());
                ChambreInterventionCollectionView = new ListCollectionView(ChambreIntervention);
            }
            GroupeChambres = Reference_ViewModel.ChambreGroupe.GroupeChambres;
        }



        #endregion

        #region Binding

        public bool PrendreInterventionCommeModele
        {
            get { return prendreInterventionCommeModele; }
            set
            {
                prendreInterventionCommeModele = value; OnPropertyChanged("PrendreInterventionCommeModele");
            }
        }
        private bool prendreInterventionCommeModele;

        public bool UneChambreUnEmploye
        {
            get { return uneChambreUnEmploye; }
            set
            {
                uneChambreUnEmploye = value; OnPropertyChanged("UneChambreUnEmploye");
            }
        }
        private bool uneChambreUnEmploye;

        public bool UnGroupeChambreUnEmplye
        {
            get { return unGroupeChambreUnEmplye; }
            set
            {
                unGroupeChambreUnEmplye = value; OnPropertyChanged("UnGroupeChambreUnEmplye");
            }
        }
        private bool unGroupeChambreUnEmplye;

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


        public GroupeChambre_VM CurrentGroupeChambre
        {
            get { return currentGroupeChambre; }
            set
            {
                currentGroupeChambre = value; OnPropertyChanged("CurrentGroupeChambre");
            }
        }
        private GroupeChambre_VM currentGroupeChambre;



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

        public Employe_VM CurentEmploye
        {
            get { return curentEmploye; }
            set
            {
                curentEmploye = value; OnPropertyChanged("CurentEmploye");
            }
        }
        private Employe_VM curentEmploye;


        //Chambres
        public ObservableCollection<Chambre_VM> Chambres
        {
            get { return chambres; }
            set
            {
                chambres = value; OnPropertyChanged("Chambres");
            }
        }
        private ObservableCollection<Chambre_VM> chambres;


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

        public Chambre_VM CurrentChambre
        {
            get { return currentChambre; }
            set
            {
                currentChambre = value;
                OnPropertyChanged("CurrentChambre");

            }
        }
        private Chambre_VM currentChambre;

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

        //InterventionDetails
        public ObservableCollection<InterventionDetail_VM> InterventionDetails
        {
            get { return interventionDetails; }
            set { interventionDetails = value; OnPropertyChanged("InterventionDetails"); }
        }
        private ObservableCollection<InterventionDetail_VM> interventionDetails;

        #endregion

        #region Commands
        //ICommand
        public ICommand Add { get; set; }

        // Méthodes OnCommand
        private void OnAddCommand()
        {
            if (UneChambreUnEmploye)
            {
                var inteventionChambreEmploye = new InterventionDetail_VM
                {
                    Chambre = CurrentChambre,
                    Employe = CurentEmploye,
                    Etat = Reference_ViewModel.InterventionDetail.EtatIntervention.Where(e => e.Libelle == "None")
                    .SingleOrDefault(),
                    SaveColor = "Red"
                };
                Reference_ViewModel.InterventionDetail.InterventionDetails.Add(inteventionChambreEmploye);
            }

            if (UnGroupeChambreUnEmplye)
            {
                if(CurrentGroupeChambre.ChambreCurrentGroupe == null)
                {
                    MessageBox.Show("Le groupe: " + CurrentGroupeChambre.Nom + " ne contient aucune chambre ");
                    return;
                }
                foreach (var item in CurrentGroupeChambre.ChambreCurrentGroupe)
                {
                    var inteventionChambreEmploye = new InterventionDetail_VM
                    {
                        Chambre = new Chambre_VM { Id= item.IdDelaChambre, Nom=item.NomChambre},
                        Employe = CurentEmploye,
                        Etat = Reference_ViewModel.InterventionDetail.EtatIntervention.Where(e => e.Libelle == "None")
                        .SingleOrDefault(),
                        SaveColor = "Red"
                    };
                    Reference_ViewModel.InterventionDetail.InterventionDetails.Add(inteventionChambreEmploye);
                }
            }
            Reference_ViewModel.Main.ViewSelected = ViewEnum.InterventionDetail;

        }
        // Méthodes OnCanExecuteCommand
        private bool OnCanExcuteAddCommand()
        {
            return PrendreInterventionCommeModele ==
                UneChambreUnEmploye == unGroupeChambreUnEmplye == true;
        }

        //Filter 


        #endregion

        #region Load

        private void Load_Chambres()
        {
            Chambres = new ObservableCollection<Chambre_VM>(
                MakfiData.Chambre_Read()
                .Select(x => new Chambre_VM
                {
                    Id = x.Id,
                    Nom = x.Nom
                }));
        }

        #endregion
    }
}
