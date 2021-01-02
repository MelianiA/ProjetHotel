using Makrisoft.Makfi.Dal;
using Makrisoft.Makfi.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Makrisoft.Makfi.ViewModels
{
    public class InterventionSupprimerViewModel_ : ViewModelBase
    {
        #region Constructeur

        #endregion

        #region Binding
        //GroupeChambre
        public ObservableCollection<Etage_VM> GroupeChambres
        {
            get { return groupeChambres; }
            set
            {
                groupeChambres = value;
                OnPropertyChanged("GroupeChambres");

            }
        }
        private ObservableCollection<Etage_VM> groupeChambres;

        public Etage_VM CurrentEtage
        {
            get { return currentEtage; }
            set
            {
                currentEtage = value;
                if (currentEtage != null)
                    Load_ChambreCurrentGroupe();
                OnPropertyChanged("CurrentEtage");
            }
        }
        private Etage_VM currentEtage;
        //ChambreByGroupe 
        public ObservableCollection<Chambre_VM> AllChambres
        {
            get { return allChambres; }
            set
            {
                allChambres = value;
                OnPropertyChanged("AllChambres");

            }
        }
        private ObservableCollection<Chambre_VM> allChambres;

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

        //Les choix
        public bool ParEmploye
        {
            get { return parEmploye; }
            set
            {
                parEmploye = value;
                OnPropertyChanged("ParEmploye");
            }
        }
        private bool parEmploye = false;

        public bool Annuler
        {
            get { return annuler; }
            set
            {
                annuler = value; OnPropertyChanged("Annuler");
            }
        }
        private bool annuler = true;

        public bool ParGroupeChambre
        {
            get { return parGroupeChambre; }
            set
            {
                parGroupeChambre = value; OnPropertyChanged("ParGroupeChambre");
            }
        }
        private bool parGroupeChambre = false;

        public bool ParChambre
        {
            get { return parChambre; }
            set
            {
                parChambre = value; OnPropertyChanged("ParChambre");
            }
        }
        private bool parChambre = false;

        #endregion

        #region Commands
        //ICommand

        // Méthodes OnCommand
        public void OnSupprimerParBloc()
        {
            if (ParEmploye)
            {
                var employe = Reference_ViewModel.InterventionDetail.DgSource.Where(i => i.Employe.Id == CurentEmploye.Id).ToList();
                foreach (var item in employe)
                {
                    var param = MakfiData.Delete("InterventionDetails_Delete", $" < interventionDetails><id>{item.Id}</id></interventionDetails>");
                    if (param) Reference_ViewModel.InterventionDetail.DgSource.Remove(item);
                }
            }
            if (ParGroupeChambre)
            {
                foreach (var item in Reference_ViewModel.InterventionDetail.DgSource)
                {
                    if (CurrentEtage.Chambres.Any(c => c.Id == item.Chambre.Id))
                    {
                        if (!MakfiData.Delete("InterventionDetails_Delete", $" < interventionDetails><id>{item.Id}</id></interventionDetails>"))
                            throw new Exception();
                    }
                }
                //Reference_ViewModel.InterventionDetail.Load_DgSource(); 

            }
            if (ParChambre)
            {
                var employe = Reference_ViewModel.InterventionDetail.DgSource.Where(i => i.Chambre.Id == currentChambre.Id).ToList();
                foreach (var item in employe)
                {
                    var param = MakfiData.Delete("InterventionDetails_Delete", $" < interventionDetails><id>{item.Id}</id></interventionDetails>");
                    if (param) Reference_ViewModel.InterventionDetail.DgSource.Remove(item);
                }
            }
        }

        // Méthodes OnCanExecuteCommand

        //Filter 
        #endregion

        #region Load
        public void Load_InterventionDetailsAjouter()
        {

            Chambres = new ObservableCollection<Chambre_VM>(
                MakfiData.Read<Chambre>
                (
                    "Chambre_Read",
                    null,
                    e =>
                    {
                        e.Id = (Guid)MakfiData.Reader["Id"];
                        e.Nom = MakfiData.Reader["Nom"] as string;
                        e.Etat = (Guid)MakfiData.Reader["Etat"];
                        e.Etage = MakfiData.Reader["GroupeChambre"] as Guid?;
                        e.Commentaire = MakfiData.Reader["Commentaire"] as string;
                    })
                .Select(x => new Chambre_VM
                {
                    Id = x.Id,
                    Nom = x.Nom
                }));

            //Employe
            if (Reference_ViewModel.Employe.Employes != null) // Employes interdit !!!!!
            {
                EmployeIntervention = Reference_ViewModel.Employe.Employes;// Employes interdit !!!!!
                EmployeInterventionCollectionView = new ListCollectionView(EmployeIntervention);
                CurentEmploye = EmployeIntervention.FirstOrDefault();
            }
            //chambres
            if (Reference_ViewModel.Chambre.Chambres != null)
            {
                ChambreIntervention = new ObservableCollection<Chambre_VM>(Reference_ViewModel.Chambre.Chambres.Select(c => new Chambre_VM { Id = c.Id, Nom = c.Nom }).ToList());
                ChambreInterventionCollectionView = new ListCollectionView(ChambreIntervention);
                CurrentChambre = ChambreIntervention.FirstOrDefault();
            }
            //GroupeChambres
            GroupeChambres = Reference_ViewModel.Etage.Etages;
            CurrentEtage = GroupeChambres.FirstOrDefault();
            //
            AllChambres = new ObservableCollection<Chambre_VM>
                (
                MakfiData.Read<Chambre>(
                    "ChambreByGroupe_Read",
                    $"<chambreByGroupe><hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel></chambreByGroupe>",
                    e =>
                    {
                        e.Id = (Guid)MakfiData.Reader["IdDelaChambre"];
                        e.Etage = MakfiData.Reader["GroupeChambre"] as Guid?;
                        e.Nom = MakfiData.Reader["Nom"] as string;
                        e.Nom = MakfiData.Reader["NomChambre"] as string;
                    })
              .Select(x => new Chambre_VM
              {
                  Etage = x.Etage,
                  Nom = x.Nom,
                  Id = x.Id,
              }).ToList());


        }
        public void Load_ChambreCurrentGroupe()
        {
            //if (CurrentEtage != null && AllChambres!=null)
            //{
            //    CurrentEtage.Chambres = new ObservableCollection<ChambreByEtage_VM>(
            //        AllChambres.Where(c => c.GroupeChambre == CurrentEtage.Id)
            //        );
            //}
        }

        #endregion
    }
}
