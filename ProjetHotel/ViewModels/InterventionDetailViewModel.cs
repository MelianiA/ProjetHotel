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
    public class InterventionDetailViewModel : ViewModel<InterventionDetail_VM>
    {

        public InterventionDetailViewModel()
        {
            EtatType = EntiteEnum.InterventionDetail;
            SortDescriptions = new SortDescription[1] { new SortDescription("Chambre.Nom", ListSortDirection.Ascending) };
            MustLoad = LoadEnum.Etats | LoadEnum.Employes | LoadEnum.Chambres | LoadEnum.Etages;
            Init();
        }
        public override IEnumerable<InterventionDetail_VM> DgSource_Read()
        {
            return MakfiData.InterventionDetail_Read($"<interventionDetail><intervention>{Reference_ViewModel.Intervention.CurrentDgSource.Id}</intervention></interventionDetail>")
             .Select(x => new InterventionDetail_VM
             {
                 Id = x.Id,
                 Employe = Employes.Where(c => c.Id == x.Employe.Id).Single(),
                 Chambre = Chambres.Where(c => c.Id == x.Chambre.Id).FirstOrDefault(),
                 Libelle = Reference_ViewModel.Intervention.CurrentDgSource.Libelle,
                 Commentaire = x.Commentaire,
                 Etat = MakfiData.Etats.Where(e => e.Id == x.Etat).Single(),
                 SaveColor = "Navy"
             }).OrderBy(x => x.Libelle).ToList();
        }
        public override void DgSource_Save()
        {
            var param = $@"
                          <interventionDetail>
                            <id>{CurrentDgSource.Id}</id>
                            <employeAffecte>{CurrentDgSource.Employe.Id}</employeAffecte>
                            <commentaire>{CurrentDgSource.Commentaire}</commentaire>    
                            <chambreAffectee>{CurrentDgSource.Chambre.Id}</chambreAffectee>
                            <intervention>{Reference_ViewModel.Intervention.CurrentDgSource.Id}</intervention>    
                            <etat>{CurrentDgSource.Etat.Id}</etat> 
                           </interventionDetail>";
            var ids = MakfiData.InterventionDetail_Save(param);
            if (ids.Count == 0) throw new Exception("Rien n'a été sauvgardé ! ");
                CurrentDgSource.Id = ids[0].Id;

            CurrentDgSource.SaveColor = "Navy";
        }

        public override void OnAddCommand()
        {
            CurrentDgSource =
                new InterventionDetail_VM
                {
                    Id=null,
                    Etat = MakfiData.Etats.Where(e => e.Libelle == "None" && e.Entite == EtatType).SingleOrDefault(),
                    Employe = Employes[0],
                    Chambre = Chambres[0]
                };
            DgSource.Add(CurrentDgSource);
        }

        public override void OnDeleteCommand()
        {
            var param = MakfiData.InterventionDetails_Delete($"<interventionDetails><id>{CurrentDgSource.Id}</id></interventionDetails>");
            if (param) DgSource.Remove(CurrentDgSource);
        }

        public Etat_VM GetSommeEtats()
        {
            if (DgSource.Count == 0 || DgSource.All(i => i.Etat.Libelle == "None"))
                return MakfiData.Etats.Where(e => e.Libelle == "None" && e.Entite == EtatType).Single();
            if (DgSource.All(i => i.Etat.EtatEtat))
                return MakfiData.Etats.Where(e => e.Libelle == "Terminée" && e.Entite == EtatType).Single();
            else
                return MakfiData.Etats.Where(e => e.Libelle == "Non terminée" && e.Entite == EtatType).Single();
        }
        //public void GetChambresGroupChambre()
        //{
        //    if (CurrentGroupeChambres != null)
        //    {
        //        CurrentGroupeChambres.Chambres = new ObservableCollection<ChambreByEtage_VM>(
        //            Reference_ViewModel.InterventionAjouter.AllChambres.Where(c => c.GroupeChambre == CurrentGroupeChambres.Id)
        //            );
        //    }
        //}
        public bool OnCanExecuteMenuSaveAllCommand()
        {
            return DgSource.Any(i => i.SaveColor == "Red");
        }
        public override bool DgSourceFilter(object item)
        {
            var interventionDetail = (InterventionDetail_VM)item;
            return (FilterEtage == null || FilterEtage.Chambres.Any(e=> e.Id == interventionDetail.Chambre.Id)) &&
                   (FilterEmploye == null || Employes.Any(e => interventionDetail.Employe.Id == FilterEmploye.Id)) &&
                   (FilterEtat == null || Etats.Any(e => interventionDetail.Etat.Id == FilterEtat.Id));
        }

        //  #region Constructeur
        //  public InterventionDetailViewModel()
        //  {
        //      // Icommand
        //      MenuAddAllCommand = new RelayCommand(p => OnMenuAddAllCommand(), p => true);
        //      MenuDeleteAllCommand = new RelayCommand(p => OnMenuDeleteAllCommand(), p => OnCanExecuteMenuDeleteAllCommand());
        //      MenuAddCommand = new RelayCommand(p => OnMenuAddCommand(), p => true);
        //      MenuDeleteCommand = new RelayCommand(p => OnMenuDeleteCommand(), p => OnCanExecuteMenuDeleteCommand());
        //      MenuSaveAllCommand = new RelayCommand(p => OnMenuSaveAllCommand(), p => OnCanExecuteMenuSaveAllCommand());

        //      InterventionDetailSaveCommand = new RelayCommand(p => OnInterventionDetailSaveCommand(), p => OnCanExecuteInterventionDetailSaveCommand());
        //      FilterClearCommand = new RelayCommand(p => OnFilterClearCommand(), p => OnCanExecuteFilterClearCommand());

        //      InterventionDetails = new ObservableCollection<InterventionDetail_VM>();
        //      InterventionDetailsCollectionView = new ListCollectionView(InterventionDetails);
        //      InterventionDetailsCollectionView.SortDescriptions.Add(new SortDescription("Chambre.Nom", System.ComponentModel.ListSortDirection.Ascending));
        //  }

        //  #endregion

        //  #region Binding

        //  //Intervention
        //  public Intervention_VM CurrentIntervention
        //  {
        //      get { return currentIntervention; }
        //      set
        //      {
        //          currentIntervention = value;
        //          OnPropertyChanged("CurrentIntervention");
        //      }
        //  }
        //  private Intervention_VM currentIntervention;

        //  //IsModifierEnabled
        //  public bool IsModifierEnabled
        //  {
        //      get { return isModifierEnabled; }
        //      set
        //      {
        //          isModifierEnabled = value;
        //          OnPropertyChanged("IsModifierEnabled");
        //      }
        //  }
        //  private bool isModifierEnabled;

        //  //InterventionDetails
        //  public ObservableCollection<InterventionDetail_VM> InterventionDetails
        //  {
        //      get { return interventionDetails; }
        //      set { interventionDetails = value; OnPropertyChanged("InterventionDetails"); }
        //  }
        //  private ObservableCollection<InterventionDetail_VM> interventionDetails;
        //  public InterventionDetail_VM CurrentInterventionDetail
        //  {
        //      get { return currentInterventionDetail; }
        //      set
        //      {
        //          currentInterventionDetail = value;
        //          if (currentInterventionDetail == null) IsModifierEnabled = false;
        //          else IsModifierEnabled = true;
        //          OnPropertyChanged("CurrentInterventionDetail");
        //      }
        //  }
        //  private InterventionDetail_VM currentInterventionDetail;
        //  public ListCollectionView InterventionDetailsCollectionView
        //  {
        //      get { return interventionDetailCollectionView; }
        //      set { interventionDetailCollectionView = value; OnPropertyChanged("InterventionDetailsCollectionView"); }
        //  }
        //  private ListCollectionView interventionDetailCollectionView;

        //  //Etat
        //  public ObservableCollection<Etat_VM> EtatList
        //  {
        //      get { return etatList; }
        //      set
        //      {
        //          etatList = value;
        //          OnPropertyChanged("EtatList");
        //      }
        //  }
        //  private ObservableCollection<Etat_VM> etatList;
        //  public ListCollectionView EtatListCollectionView
        //  {
        //      get { return etatListCollectionView; }
        //      set { etatListCollectionView = value; OnPropertyChanged("EtatListCollectionView"); }
        //  }
        //  private ListCollectionView etatListCollectionView;

        //  //EtatIntervention
        //  public ObservableCollection<Etat_VM> EtatIntervention
        //  {
        //      get { return etatIntervention; }
        //      set
        //      {
        //          etatIntervention = value;
        //          OnPropertyChanged("EtatIntervention");
        //      }
        //  }
        //  private ObservableCollection<Etat_VM> etatIntervention;
        //  public ListCollectionView EtatInterventionCollectionView
        //  {
        //      get { return etatInterventionCollectionView; }
        //      set { etatInterventionCollectionView = value; OnPropertyChanged("EtatInterventionCollectionView"); }
        //  }
        //  private ListCollectionView etatInterventionCollectionView;

        //  public ListCollectionView EtatInterventionFilterCollectionView
        //  {
        //      get { return etatInterventionFilterCollectionView; }
        //      set { etatInterventionFilterCollectionView = value; OnPropertyChanged("EtatInterventionFilterCollectionView"); }
        //  }
        //  private ListCollectionView etatInterventionFilterCollectionView;
        //  //Employe 
        //  public ObservableCollection<Employe_VM> EmployeIntervention
        //  {
        //      get { return employeIntervention; }
        //      set
        //      {
        //          employeIntervention = value;
        //          OnPropertyChanged("EmployeIntervention");
        //      }
        //  }
        //  private ObservableCollection<Employe_VM> employeIntervention;
        //  public ListCollectionView EmployeInterventionCollectionView
        //  {
        //      get { return employeInterventionCollectionView; }
        //      set
        //      {
        //          employeInterventionCollectionView = value;
        //          OnPropertyChanged("EmployeInterventionCollectionView");
        //      }
        //  }
        //  private ListCollectionView employeInterventionCollectionView;

        //  //Chambre
        //  public ObservableCollection<Chambre_VM> ChambreIntervention
        //  {
        //      get { return chambreIntervention; }
        //      set
        //      {
        //          chambreIntervention = value;
        //          OnPropertyChanged("ChambreIntervention");
        //      }
        //  }
        //  private ObservableCollection<Chambre_VM> chambreIntervention;
        //  public ListCollectionView ChambreInterventionCollectionView
        //  {
        //      get { return chambreInterventionCollectionView; }
        //      set
        //      {
        //          chambreInterventionCollectionView = value;
        //          OnPropertyChanged("ChambreInterventionCollectionView");
        //      }
        //  }
        //  private ListCollectionView chambreInterventionCollectionView;

        //  //Filter
        //  public Etage_VM CurrentGroupeChambres
        //  {
        //      get { return currentGroupeChambres; }
        //      set
        //      {
        //          currentGroupeChambres = value;
        //          if (InterventionDetailsCollectionView != null)
        //              InterventionDetailsCollectionView.Filter = FilterInterventionDetails;
        //          OnPropertyChanged("CurrentGroupeChambres");
        //      }
        //  }
        //  private Etage_VM currentGroupeChambres;

        //  public Etat_VM CurrentEtatIntervention
        //  {
        //      get { return currentEtatIntervention; }
        //      set
        //      {
        //          currentEtatIntervention = value;
        //          if (InterventionDetailsCollectionView != null)
        //              InterventionDetailsCollectionView.Filter = FilterInterventionDetails;
        //          OnPropertyChanged("CurrentEtatIntervention");
        //      }
        //  }
        //  private Etat_VM currentEtatIntervention;

        //  public Employe_VM CurentEmployeIntervention
        //  {
        //      get { return curentEmployeIntervention; }
        //      set
        //      {
        //          curentEmployeIntervention = value;
        //          if (InterventionDetailsCollectionView != null)
        //              InterventionDetailsCollectionView.Filter = FilterInterventionDetails;
        //          OnPropertyChanged("CurentEmployeIntervention");
        //      }
        //  }
        //  private Employe_VM curentEmployeIntervention;


        //  //GroupeChambre
        //  public ObservableCollection<Etage_VM> GroupeChambres
        //  {
        //      get { return groupeChambres; }
        //      set
        //      {
        //          groupeChambres = value;
        //          OnPropertyChanged("GroupeChambres");
        //      }
        //  }
        //  private ObservableCollection<Etage_VM> groupeChambres;


        //  #endregion

        //  #region Commands
        //  //ICommand
        //  public ICommand MenuAddAllCommand { get; set; }
        //  public ICommand MenuDeleteAllCommand { get; set; }
        //  public ICommand InterventionDetailSaveCommand { get; set; }
        //  public ICommand MenuAddCommand { get; set; }
        //  public ICommand MenuDeleteCommand { get; set; }
        //  public ICommand MenuSaveAllCommand { get; set; }
        //  public ICommand FilterClearCommand { get; set; }

        //  // Méthodes OnCommand
        //  private void OnMenuAddAllCommand()
        //  {
        //      Reference_ViewModel.Main.ViewSelected = ViewEnum.InterventionAjouter;
        //      Reference_ViewModel.InterventionAjouter.Load_InterventionDetails();
        //      Reference_ViewModel.InterventionAjouter.CheckAnnuler = true;
        //      Reference_ViewModel.InterventionAjouter.Interventions.Remove(CurrentIntervention);

        //  }
        //  private void OnMenuDeleteAllCommand()
        //  {
        //      Reference_ViewModel.InterventionSupprimer.Load_InterventionDetailsAjouter();
        //      Reference_ViewModel.InterventionSupprimer.Annuler = true;
        //      Reference_ViewModel.Main.ViewSelected = ViewEnum.InterventionSupprimer;
        //  }
        //  private void OnInterventionDetailSaveCommand()
        //  {

        //      Guid? monID = null;
        //      if (currentInterventionDetail.Id != default) monID = currentInterventionDetail.Id;
        //      var param = $@"
        //              <interventionDetail>
        //                  <id>{monID}</id>
        //                  <employeAffecte>{currentInterventionDetail.Employe.Id}</employeAffecte>
        //                  <commentaire>{currentInterventionDetail.Commentaire}</commentaire>    
        //<chambreAffectee>{currentInterventionDetail.Chambre.Id}</chambreAffectee>
        //                  <intervention>{CurrentIntervention.Id}</intervention>    
        //                  <etat>{currentInterventionDetail.Etat.Id}</etat> 
        //               </interventionDetail>";
        //      var ids = MakfiData.InterventionDetail_Save(param);
        //      if (ids.Count == 0) throw new Exception("Rien n'a été sauvgardé ! ");
        //      currentInterventionDetail.Id = ids[0].Id;
        //      currentInterventionDetail.SaveColor = "Navy";
        //      Reference_ViewModel.Intervention.CurrentDgSource.Etat = GetSommeEtats();
        //      Reference_ViewModel.Intervention.CurrentDgSource.SaveColor = "Navy";

        //  }
        //  private void OnMenuAddCommand()
        //  {
        //      CurrentInterventionDetail = new InterventionDetail_VM
        //      {
        //          Etat = EtatIntervention.Where(e => e.Libelle == "None" && e.Entite==EntiteEnum.InterventionDetail).SingleOrDefault()
        //      };
        //      InterventionDetails.Add(CurrentInterventionDetail);
        //  }
        //  private void OnMenuDeleteCommand()
        //  {
        //      var param = MakfiData.InterventionDetails_Delete($"<interventionDetails><id>{CurrentInterventionDetail.Id}</id></interventionDetails>");
        //      if (param) InterventionDetails.Remove(CurrentInterventionDetail);
        //  }
        //  private void OnMenuSaveAllCommand()
        //  {
        //      List<InterventionDetail_VM> elementAsupp = new List<InterventionDetail_VM>();
        //      var IntDetails = InterventionDetails.Where(i => i.SaveColor == "Red");
        //      foreach (var item in IntDetails)
        //      {
        //          Guid? monID = null; var param = "";
        //          if (item.Id != default) monID = item.Id;
        //          if (item.Employe != null && item.Chambre != null)
        //          {
        //              param = $@"
        //              <interventionDetail>
        //                  <id>{monID}</id>
        //                  <employeAffecte>{item.Employe.Id}</employeAffecte>
        //                  <commentaire>{item.Commentaire}</commentaire>    
        //<chambreAffectee>{item.Chambre.Id}</chambreAffectee>
        //                  <intervention>{CurrentIntervention.Id}</intervention>    
        //                  <etat>{item.Etat.Id}</etat> 
        //               </interventionDetail>";
        //              var ids = MakfiData.InterventionDetail_Save(param);
        //              if (ids.Count == 0) throw new Exception("Rien n'a été sauvgardé ! ");
        //              item.Id = ids[0].Id;
        //              item.SaveColor = "Navy";
        //              Reference_ViewModel.Intervention.CurrentDgSource.Etat = GetSommeEtats();
        //              Reference_ViewModel.Intervention.CurrentDgSource.SaveColor = "Navy";
        //          }
        //          else
        //              elementAsupp.Add(item);
        //      }
        //      foreach (var item in elementAsupp)
        //          InterventionDetails.Remove(item);
        //  }
        //  private void OnFilterClearCommand()
        //  {
        //      CurrentGroupeChambres = null;
        //      CurrentEtatIntervention = null;
        //      CurentEmployeIntervention = null;
        //  }


        //  // Méthodes OnCanExecuteCommand
        //  private bool OnCanExecuteInterventionDetailSaveCommand()
        //  {
        //      return CurrentInterventionDetail != null&& currentInterventionDetail.Employe != null&& currentInterventionDetail.Chambre != null ;
        //  }
        //  private bool OnCanExecuteMenuDeleteCommand()
        //  {
        //      return CurrentInterventionDetail != null;
        //  }
        //  private bool OnCanExecuteMenuDeleteAllCommand()
        //  {
        //      return InterventionDetails.Count > 0;
        //  }
        //  private bool OnCanExecuteFilterClearCommand()
        //  {
        //      return (CurrentGroupeChambres != null || CurrentEtatIntervention != null || CurentEmployeIntervention != null);
        //  }

        //  //Filter 
        //  public bool FilterInterventionDetails(object item)
        //  {
        //      if (CurrentEtatIntervention != null && CurentEmployeIntervention != null && CurrentGroupeChambres != null)
        //      {
        //          GetChambresGroupChambre();
        //          if (item is InterventionDetail_VM interventionDetail)
        //              return EmployeIntervention.Any(e => interventionDetail.Employe.Id == CurentEmployeIntervention.Id)
        //                  && EtatIntervention.Any(e => interventionDetail.Etat.Id == CurrentEtatIntervention.Id)
        //                  && CurrentGroupeChambres.Chambres.Any(e => interventionDetail.Chambre.Id == e.IdDelaChambre);
        //          return false;
        //      }
        //      if (CurrentGroupeChambres != null && CurrentEtatIntervention != null)
        //      {
        //          GetChambresGroupChambre();
        //          if (item is InterventionDetail_VM interventionDetail)
        //              return EtatIntervention.Any(e => interventionDetail.Etat.Id == CurrentEtatIntervention.Id)
        //                  && CurrentGroupeChambres.Chambres.Any(e => interventionDetail.Chambre.Id == e.IdDelaChambre);
        //          return false;
        //      }
        //      if (CurrentGroupeChambres != null && CurentEmployeIntervention != null)
        //      {
        //          GetChambresGroupChambre();
        //          if (item is InterventionDetail_VM interventionDetail)
        //              return EmployeIntervention.Any(e => interventionDetail.Employe.Id == CurentEmployeIntervention.Id)
        //                  && CurrentGroupeChambres.Chambres.Any(e => interventionDetail.Chambre.Id == e.IdDelaChambre);
        //          return false;
        //      }

        //      if (CurrentEtatIntervention != null && CurentEmployeIntervention != null)
        //      {
        //          if (item is InterventionDetail_VM interventionDetail)
        //              return EmployeIntervention.Any(e => interventionDetail.Employe.Id == CurentEmployeIntervention.Id)
        //                  && EtatIntervention.Any(e => interventionDetail.Etat.Id == CurrentEtatIntervention.Id);
        //          return false;
        //      }


        //      if (CurrentEtatIntervention != null)
        //      {
        //          if (item is InterventionDetail_VM interventionDetail)
        //              return EtatIntervention.Any(e => interventionDetail.Etat != null && interventionDetail.Etat.Id == CurrentEtatIntervention.Id);
        //          return false;
        //      }
        //      if (CurentEmployeIntervention != null)
        //      {
        //          if (item is InterventionDetail_VM interventionDetail)
        //              return EmployeIntervention.Any(e => interventionDetail.Employe.Id == CurentEmployeIntervention.Id);
        //          return false;
        //      }
        //      if (CurrentGroupeChambres != null)
        //      {
        //          GetChambresGroupChambre();
        //          if (item is InterventionDetail_VM interventionDetail)
        //              return CurrentGroupeChambres.Chambres.Any(e => interventionDetail.Chambre.Id == e.IdDelaChambre);
        //          return false;
        //      }
        //      return true;
        //  }

        //  //Autres
        //  #endregion

        //  #region Load
        //  public void Load_InterventionDetail()
        //  {
        //      //Chargement des etats 
        //      Load_Etat();
        //      //Employe
        //      EmployeIntervention = Reference_ViewModel.Employe.Employes;
        //      EmployeInterventionCollectionView = new ListCollectionView(EmployeIntervention.Where(e => e.Etat.Libelle == "Disponible").ToList());
        //      //chambre
        //      ChambreIntervention = new ObservableCollection<Chambre_VM>(
        //          Reference_ViewModel.Chambre.ChambreGroupeChambre.Select(c => new Chambre_VM { Id = c.Id, Nom = c.Nom, Etat=c.Etat }).ToList());
        //      ChambreInterventionCollectionView = new ListCollectionView(ChambreIntervention.Where(c=>c.Etat.Libelle== "Disponible").ToList());
        //      //GroupeChambre
        //      GroupeChambres = Reference_ViewModel.ChambreGroupe.Etages;
        //      // InterventionDetails
        //      if (InterventionDetails != null) InterventionDetails.Clear();
        //      if (Reference_ViewModel.Header.CurrentHotel == null)
        //      {
        //          MessageBox.Show($"Aucun hôtel ne vous a été assigné  ", "Impossible d'enregistrer  !");
        //          return;
        //      }
        //      Guid? monId = default;
        //      if (Reference_ViewModel.Intervention.CurrentDgSource != null)
        //              monId = Reference_ViewModel.Intervention.CurrentDgSource.Id;

        //      var tmp =(
        //         MakfiData.InterventionDetail_Read($"<interventionDetail><intervention>{monId}</intervention></interventionDetail>")
        //         .Select(x => new InterventionDetail_VM
        //         {
        //             Id = x.Id,
        //             Employe = EmployeIntervention.Where(e => e.Id == x.Employe).SingleOrDefault(),
        //             Chambre = ChambreIntervention.Where(c => c.Id == x.Chambre).SingleOrDefault(),
        //             Libelle = Reference_ViewModel.Intervention.CurrentDgSource.Libelle,
        //             Commentaire = x.Commentaire,
        //             Etat = EtatIntervention.Where(e => e.Id == x.Etat).SingleOrDefault(),
        //             SaveColor = "Navy"
        //         }).OrderBy(x => x.Libelle).ToList());

        //      foreach (var item in tmp)
        //          InterventionDetails.Add(item);
        //      CurrentInterventionDetail = InterventionDetailsCollectionView.Count > 0 ? (InterventionDetail_VM)InterventionDetailsCollectionView.GetItemAt(0) : null;
        //      OnFilterClearCommand();
        //  }
        //  private void Load_Etat()
        //  {
        //      EtatList = new ObservableCollection<Etat_VM>(
        //        MakfiData.Etat_Read()
        //        .Select(x => new Etat_VM
        //        {
        //            Id = x.Id,
        //            Libelle = x.Libelle,
        //            Icone = x.Icone,
        //            Couleur = x.Couleur,
        //            Entite = x.Entite,
        //            EtatEtat = x.EtatEtat
        //        }).ToList()); ;
        //      EtatListCollectionView = new ListCollectionView(EtatList);
        //      EtatListCollectionView.Refresh();
        //      EtatIntervention = new ObservableCollection<Etat_VM>(
        //         EtatList.Where(x => x.Entite == EntiteEnum.InterventionDetail || x.Entite == EntiteEnum.Intervention).ToList());
        //      EtatInterventionCollectionView = new ListCollectionView(EtatIntervention.Where(w=>w.Entite==EntiteEnum.InterventionDetail).ToList());
        //      EtatInterventionFilterCollectionView = new ListCollectionView(EtatIntervention.Where(w => w.Entite == EntiteEnum.InterventionDetail).ToList());
        //      EtatInterventionCollectionView.Refresh();
        //  }

        //  #endregion

    }
}
