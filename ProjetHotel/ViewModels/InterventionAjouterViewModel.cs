using Makrisoft.Makfi.Dal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Makrisoft.Makfi.ViewModels
{
    public class InterventionAjouterViewModel : ViewModel<InterventionDetail_VM>
    {
        #region Constructeur
        public InterventionAjouterViewModel()
        {
            EtatType = EntiteEnum.InterventionAjouter;
            Components = ComponentEnum.Chambres | ComponentEnum.Employes | ComponentEnum.Interventions | ComponentEnum.Etages;
            Title = "Tout ajouter !";
            Init();
        }
        #endregion
        #region DgSource
        public override IEnumerable<InterventionDetail_VM> DgSource_Read()
        {
            if (CurrentIntervention != null)
            {
                return MakfiData
                    .InterventionDetail_Read($@"
                        <interventionDetails>
                            <intervention>{CurrentIntervention.Id}</intervention>
                        </interventionDetails>")
                    .Select(x => new InterventionDetail_VM
                    {
                        Id = null,
                        Employe = Reference_ViewModel.InterventionDetail.Employes.Where(c => c.Id == x.Employe.Id).Single(),
                        Chambre = Reference_ViewModel.InterventionDetail.Chambres.Where(c => c.Id == x.Chambre.Id).FirstOrDefault(),
                        Libelle = Reference_ViewModel.Intervention.CurrentDgSource.Libelle,
                        Commentaire = x.Commentaire,
                        Etat = MakfiData.Etats.Where(e => e.Id == x.Etat).Single(),
                        SaveColor = "Red"
                    })
                    .OrderBy(x => x.Libelle)
                    .ToList();
            }
            else if (CurrentEtage != null && CurrentEmploye != null)
            {
                var interDetails = new List<InterventionDetail_VM>();
                var chambres = MakfiData.Chambre_Read($"<chambres><groupeChambre>{CurrentEtage.Id}</groupeChambre></chambres>");
                foreach (var chambre in chambres)
                {
                    interDetails.Add(new InterventionDetail_VM
                    {
                        Id = null,
                        Chambre = Reference_ViewModel.InterventionDetail.Chambres.Where(c => c.Id == chambre.Id).FirstOrDefault(),
                        Employe = Reference_ViewModel.InterventionDetail.Employes.Where(c => c.Id == CurrentEmploye.Id).Single(),
                        Etat = MakfiData.Etats.Where(e => e.Entite == EntiteEnum.InterventionDetail && e.Libelle == "None").Single(),
                        SaveColor = "Red"
                    });
                }
                return interDetails;
            }
            return new List<InterventionDetail_VM>();
        }
        #endregion

        public bool CheckAnnuler
        {
            get { return checkAnnuler; }
            set
            {
                checkAnnuler = value;
                CurrentEmploye = null;
                CurrentEtage = null;
                CurrentIntervention = null;
                OnPropertyChanged("CheckAnnuler");
            }
        }
        private bool checkAnnuler = true;
        public Intervention_VM CurrentIntervention
        {
            get { return currentIntervention; }
            set
            {
                currentIntervention = value;
            
                Load_DgSource();
                OnPropertyChanged("CurrentIntervention");
            }
        }
        private Intervention_VM currentIntervention;

        public Etage_VM CurrentEtage
        {
            get { return currentEtage; }
            set
            {
                currentEtage = value;
                Load_DgSource();

                OnPropertyChanged("CurrentEtage");
            }
        }
        private Etage_VM currentEtage;
        public Employe_VM CurrentEmploye
        {
            get { return currentEmploye; }
            set
            {
                currentEmploye = value;
                Load_DgSource();
                OnPropertyChanged("CurrentEmploye");
            }
        }
        private Employe_VM currentEmploye;

        public bool CheckInterventionModel
        {
            get { return checkInterventionModel; }
            set
            {
                checkInterventionModel = value;
                CurrentEmploye = null;
                CurrentEtage = null;
                OnPropertyChanged("CheckInterventionModel");
            }
        }
        private bool checkInterventionModel = false;


        public bool CheckUnEtageUnEmploye
        {
            get { return checkUnEtageUnEmploye; }
            set
            {
                checkUnEtageUnEmploye = value;
                CurrentIntervention = null;
                OnPropertyChanged("CheckUnEtageUnEmploye");
            }
        }
        private bool checkUnEtageUnEmploye = false;


        //#region Constructeur
        //public InterventionAjouterViewModel()
        //{
        //    // Icommand

        //    // Load
        //    Load_InterventionDetails();
        //}
        //#endregion

        //#region Binding





        ////GroupeChambre
        //public ObservableCollection<Etage_VM> Etages
        //{
        //    get { return etages; }
        //    set
        //    {
        //        etages = value;
        //        OnPropertyChanged("GroupeChambres");

        //    }
        //}
        //private ObservableCollection<Etage_VM> etages;

        ////Interventions
        //public ObservableCollection<Intervention_VM> Interventions
        //{
        //    get { return interventions; }
        //    set { interventions = value; OnPropertyChanged("Interventions"); }
        //}
        //private ObservableCollection<Intervention_VM> interventions;


        ////ChambreByGroupe 
        //public ObservableCollection<ChambreByEtage_VM> AllChambres
        //{
        //    get { return allChambres; }
        //    set
        //    {
        //        allChambres = value;
        //        OnPropertyChanged("AllChambres");

        //    }
        //}
        //private ObservableCollection<ChambreByEtage_VM> allChambres;

        ////Employe 
        //public ObservableCollection<Employe_VM> EmployeIntervention
        //{
        //    get { return employeIntervention; }
        //    set
        //    {
        //        employeIntervention = value;
        //        OnPropertyChanged("EmployeIntervention");
        //    }
        //}
        //private ObservableCollection<Employe_VM> employeIntervention;

        //public ListCollectionView EmployeInterventionCollectionView
        //{
        //    get { return employeInterventionCollectionView; }
        //    set
        //    {
        //        employeInterventionCollectionView = value;
        //        OnPropertyChanged("EmployeInterventionCollectionView");
        //    }
        //}
        //private ListCollectionView employeInterventionCollectionView;

        ////Chambres
        //public ObservableCollection<Chambre_VM> Chambres
        //{
        //    get { return chambres; }
        //    set
        //    {
        //        chambres = value; OnPropertyChanged("Chambres");
        //    }
        //}
        //private ObservableCollection<Chambre_VM> chambres;

        //public ObservableCollection<Chambre_VM> ChambreIntervention
        //{
        //    get { return chambreIntervention; }
        //    set
        //    {
        //        chambreIntervention = value;
        //        OnPropertyChanged("ChambreIntervention");
        //    }
        //}
        //private ObservableCollection<Chambre_VM> chambreIntervention;

        //public Chambre_VM CurrentChambre
        //{
        //    get { return currentChambre; }
        //    set
        //    {
        //        currentChambre = value;
        //        OnPropertyChanged("CurrentChambre");

        //    }
        //}
        //private Chambre_VM currentChambre;

        //public ListCollectionView ChambreInterventionCollectionView
        //{
        //    get { return chambreInterventionCollectionView; }
        //    set
        //    {
        //        chambreInterventionCollectionView = value;
        //        OnPropertyChanged("ChambreInterventionCollectionView");
        //    }
        //}
        //private ListCollectionView chambreInterventionCollectionView;

        ////InterventionDetails
        //public ObservableCollection<InterventionDetail_VM> InterventionDetails
        //{
        //    get { return interventionDetails; }
        //    set { interventionDetails = value; OnPropertyChanged("InterventionDetails"); }
        //}
        //private ObservableCollection<InterventionDetail_VM> interventionDetails;

        //#endregion

        //#region Commands
        ////ICommand

        //// Méthodes OnCommand
        //public void OnAddInterventionDetails()
        //{
        //    if (CheckInterventionModel)
        //    {
        //        Guid? monId = default;
        //        var iDetails = new ObservableCollection<InterventionDetail_VM>(
        //            MakfiData.InterventionDetail_Read($"<interventionDetail><intervention>{Reference_ViewModel.Intervention.CurrentDgSource.Id}</intervention></interventionDetail>")
        //           .Select(x => new InterventionDetail_VM
        //           {
        //               Id = x.Id,
        //               Employe = new Employe_VM { Id = x.Employe.Id },
        //               Chambre = new Chambre_VM { Id = x.Chambre.Id },
        //               Etat = MakfiData.Etats.Where(e => e.Id == x.Etat).Single(),
        //               Libelle = Reference_ViewModel.Intervention.CurrentDgSource.Libelle,
        //               Commentaire = x.Commentaire,
        //               SaveColor = "Red"
        //           }).OrderBy(x => x.Libelle).ToList());

        //        foreach (var item in iDetails)
        //            Reference_ViewModel.InterventionDetail.DgSource.Add(item);
        //    }

        //    if (CheckUnEtageUnEmploye && CurrentEtage != null)
        //    {

        //        if (CurrentEtage.Chambres.Count == 0)
        //        {
        //            MessageBox.Show("Le groupe: " + CurrentEtage.Nom + " ne contient aucune chambre ");
        //            return;
        //        }
        //        foreach (var item in CurrentEtage.Chambres)
        //        {
        //            var inteventionChambreEmploye = new InterventionDetail_VM
        //            {
        //                Chambre = new Chambre_VM { Id = item.Id, Nom = item.Nom },
        //                Employe = CurentEmploye,
        //                Etat = MakfiData.Etats.Where(e => e.Libelle == "None" && e.Entite == EntiteEnum.InterventionDetail)
        //                .SingleOrDefault(),
        //                SaveColor = "Red"
        //            };
        //            Reference_ViewModel.InterventionDetail.DgSource.Add(inteventionChambreEmploye);
        //        }
        //    }
        //    Reference_ViewModel.Main.ViewSelected = ViewEnum.InterventionDetail;

        //}

        //// Méthodes OnCanExecuteCommand
        ////private bool OnCanExcuteAddCommand()
        ////{
        ////    return CheckInterventionModel ==
        ////        annuler == checkUnEtageUnEmploye == true;
        ////}

        ////Filter 
        //#endregion

        //#region Load

        //public void Load_InterventionDetails()
        //{

        //    Chambres = new ObservableCollection<Chambre_VM>(
        //        MakfiData.Chambre_Read()
        //        .Select(x => new Chambre_VM
        //        {
        //            Id = x.Id,
        //            Nom = x.Nom
        //        }));

        //    //Employe
        //    if (Reference_ViewModel.Employe.Employes != null)
        //    {
        //        EmployeIntervention = Reference_ViewModel.Employe.Employes;
        //        EmployeInterventionCollectionView = new ListCollectionView(EmployeIntervention);
        //        CurentEmploye = EmployeIntervention.FirstOrDefault();
        //    }
        //    //chambres
        //    if (Reference_ViewModel.Chambre.ChambreGroupeChambre != null)
        //    {
        //        ChambreIntervention = new ObservableCollection<Chambre_VM>(Reference_ViewModel.Chambre.ChambreGroupeChambre.Select(c => new Chambre_VM { Id = c.Id, Nom = c.Nom }).ToList());
        //        ChambreInterventionCollectionView = new ListCollectionView(ChambreIntervention);
        //        CurrentChambre = ChambreIntervention.FirstOrDefault();
        //    }
        //    //GroupeChambres
        //    Etages = Reference_ViewModel.ChambreGroupe.Etages;
        //    if (Etages != null)
        //        CurrentEtage = Etages.FirstOrDefault();

        //    //
        //    if (Reference_ViewModel.Header.CurrentHotel != null)
        //        AllChambres = new ObservableCollection<ChambreByEtage_VM>(
        //          MakfiData.ChambreByGroupe_Read($"<chambreByGroupe><hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel></chambreByGroupe>")
        //          .Select(x => new ChambreByEtage_VM
        //          {
        //              GroupeChambre = x.GroupeChambre,
        //              Nom = x.Nom,
        //              IdDelaChambre = x.IdDelaChambre,
        //              NomChambre = x.NomChambre
        //          }).ToList());

        //    //Intervention 
        //    if (Reference_ViewModel.Intervention.DgSource != null)
        //        Interventions = new ObservableCollection<Intervention_VM>(
        //            Reference_ViewModel.Intervention.DgSource.Where(i => i.Model == true));
        //    //CurrentIntervention = Interventions.FirstOrDefault();
        //}

        //public void Load_ChambreCurrentGroupe()
        //{
        //    //if (CurrentEtage != null && AllChambres != null)
        //    //{
        //    //    CurrentEtage.Chambres = new ObservableCollection<Chambre_VM>(
        //    //        AllChambres.Where(c => c.GroupeChambre == CurrentEtage.Id)
        //    //        );
        //    //}
        //}
        //#endregion
    }
}
