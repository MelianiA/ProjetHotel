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
    public class EtageViewModel : ViewModel<Etage_VM>
    {
        #region Constructeur
        public EtageViewModel()
        {
            EtatType = EntiteEnum.Intervention;
            SortDescriptions = new SortDescription[1] { new SortDescription("Nom", System.ComponentModel.ListSortDirection.Ascending) };
            Components = ComponentEnum.Etats;
            Title = "Les groupes de chambres";

            Init();
        }
        #endregion

        #region DgSource
        public override IEnumerable<Etage_VM> DgSource_Read()
        {
            return MakfiData.Chambre_Read($"<etages><hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel></etages>")
              .Select(x => new Etage_VM
              {
                  Nom = "(A définir)"
              });
        }

        public override void Load(ViewEnum exView)
        {
            RetourIntervention = false;
        }

        public override void DgSource_Save()
        {
            var param = $@"
                        <groupeChambre>
                            <id>{CurrentDgSource.Id}</id>
                            <nom>{CurrentDgSource.Nom}</nom>
                            <commentaire>{CurrentDgSource.Commentaire}</commentaire>    
                          </groupeChambre>";
            var ids = MakfiData.Chambre_Save(param);
            if (ids.Count == 0) throw new Exception("EtageViewModel.DgSource_Save");
            CurrentDgSource.Id = ids[0].Id;
            CurrentDgSource.SaveColor = "Navy";
        }
        #endregion

        #region Command
        public override void OnAddCommand()
        {
            CurrentDgSource = new Etage_VM
            {
                Nom = "( A définir !)",
            };
            DgSource.Add(CurrentDgSource);
        }
        public override void OnDeleteCommand()
        {
            var canDeletes = MakfiData.Chambre_CanDelete($"<groupechambres><id>{CurrentDgSource.Id}</id></groupechambres>");
            if (canDeletes.Count() == 0)
            {
                var param = MakfiData.Chambre_Delete($"<groupechambres><id>{CurrentDgSource.Id}</id></groupechambres>");
                if (param) DgSource.Remove(CurrentDgSource);
            }
            else
                MessageBox.Show($"Suppression impossible", "Erreur");
        }
        #endregion

        //#region Constructeur
        //public EtageViewModel()
        //{
        //    // Icommand
        //    EtageAjouterChambreCommand = new RelayCommand(p => OnEtageAjouterChambreCommand(), p => OnCanExecuteEtageAjouterChambreCommand());
        //    EtageSupprimerChambreCommand = new RelayCommand(p => OnEtageSupprimerChambreCommand(), p => OnCanExecuteEtageSupprimerChambreCommand());
        //    EtageSaveCommand = new RelayCommand(p => OnEtageSaveCommand(), p => OnCanExecuteOnEtageSaveCommand());
        //    EtageAddCommand = new RelayCommand(p => OnEtageAddCommand(), p => true);
        //    EtageDeleteCommand = new RelayCommand(p => OnEtageDeleteCommand(), p => OnCanExecuteEtageDeleteCommand());

        //    // ListeView
        //    if (Reference_ViewModel.Header.CurrentHotel != null)
        //    {
        //        Load_Etages();
        //        Load_AllChambres();
        //    }
        //}

        //public   void Load(ViewEnum exView) //override
        //{
        //    Load_AllChambres();
        //    Load_Etages();
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
        //        OnPropertyChanged("Etages");

        //    }
        //}
        //private ObservableCollection<Etage_VM> etages;

        //public Etage_VM CurrentEtage
        //{
        //    get
        //    {
        //        return currentEtage;
        //    }
        //    set
        //    {
        //        currentEtage = value;
        //        if (CurrentEtage != null && CurrentEtage.SaveColor == "Navy")
        //            Load_ChambresByEtage();
        //        OnPropertyChanged("CurrentEtage");
        //    }
        //}
        //private Etage_VM currentEtage;
        //public ListCollectionView EtageCollectionView
        //{
        //    get { return etageCollectionView; }
        //    set { etageCollectionView = value; OnPropertyChanged("EtageCollectionView"); }
        //}
        //private ListCollectionView etageCollectionView;

        ////ChambreByGroupe 
        //public ObservableCollection<Etage_VM> AllChambres
        //{
        //    get { return allChambres; }
        //    set
        //    {
        //        allChambres = value;
        //        OnPropertyChanged("AllChambres");

        //    }
        //}
        //private ObservableCollection<Etage_VM> allChambres;

        //#endregion

        //#region Commands
        ////ICommand
        //public ICommand EtageAjouterChambreCommand { get; set; }
        //public ICommand EtageSupprimerChambreCommand { get; set; }
        //public ICommand EtageSaveCommand { get; set; }
        //public ICommand EtageAddCommand { get; set; }
        //public ICommand EtageDeleteCommand { get; set; }

        //// Méthodes OnCommand
        //private void OnEtageAjouterChambreCommand()
        //{
        //    //CurrentEtage.Chambres.Add(CurrentEtage.CurrentNotChambreCG);
        //    //CurrentEtage.AutresChambres.Remove(CurrentEtage.CurrentNotChambreCG);
        //    //CurrentEtage.SaveColor = "Red";
        //}
        //private void OnEtageSupprimerChambreCommand()
        //{
        //    //CurrentEtage.AutresChambres.Add(CurrentEtage.CurrentChambreCG);
        //    //CurrentEtage.Chambres.Remove(CurrentEtage.CurrentChambreCG);
        //    //CurrentEtage.SaveColor = "Red";
        //}
        //private void OnEtageSaveCommand()
        //{
        //    if (Reference_ViewModel.Header.CurrentHotel == null)
        //    {
        //        MessageBox.Show($"Aucun hôtel ne vous a été assigné  ", "Impossible d'enregistrer  !");
        //        Etages.Remove(CurrentEtage);
        //        return;
        //    }
        //    if (CurrentEtage.Nom == null || CurrentEtage.Nom == "(A définir ! )")
        //    {
        //        MessageBox.Show($"Impossible de sauvgarder ce groupe !" +
        //            $"\nVous devez choisir un nom pour ce groupe de chambre.", "Remarque !");
        //        return;
        //    }

        //    //Etape01 : Insertion dans la table GroupeChambre
        //    Guid? monID = null;
        //    if (CurrentEtage.Id != default) monID = CurrentEtage.Id;
        //    var param = $@"
        //            <groupeChambre>
        //                <id>{monID}</id>
        //                <nom>{CurrentEtage.Nom}</nom>
        //                <commentaire>{CurrentEtage.Commentaire}</commentaire>    
        //              </groupeChambre>";
        //    var ids = MakfiData.GroupeChambre_Save(param);
        //    if (ids.Count == 0) throw new Exception("Rien n'a été sauvgardé ! ");
        //    if (monID == null) CurrentEtage.Id = ids[0].Id;
        //    //Etape02
        //    var chambreGroupeChambre_Delete = MakfiData.ChambreGroupeChambre_Delete(
        //        $"<chambreGroupeChambre><groupeChambre>{CurrentEtage.Id}</groupeChambre><hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel></chambreGroupeChambre>"
        //        );
        //    if (!chambreGroupeChambre_Delete) throw new Exception("Rien n'a été sauvgardé ! ");
        //    //Etape03
        //    foreach (var item in CurrentEtage.Chambres)
        //    {
        //        param = $@"
        //            <chambreGroupeChambre>
        //                <chambre>{item.Id}</chambre>
        //                <groupeChambre>{ids[0].Id}</groupeChambre>    
        //              </chambreGroupeChambre>";
        //        var ids2 = MakfiData.ChambreGroupeChambre_Save(param);
        //        if (ids2.Count == 0) throw new Exception("Rien n'a été sauvgardé ! ");
        //    }
        //    CurrentEtage.SaveColor = "Navy";
        //    Load_AllChambres();
        //    Load_ChambresByEtage();
        //    //Reference_ViewModel.Chambre.Load_Chambres();
        //}
        //private void OnEtageAddCommand()
        //{
        //    CurrentEtage = new Etage_VM { Nom = "(A définir ! )" };
        //    Load_ChambresByEtage();
        //    Etages.Add(CurrentEtage);
        //}
        //private void OnEtageDeleteCommand()
        //{
        //    if (CurrentEtage.Chambres.Count != 0)
        //    {
        //        MessageBox.Show($"Vérifiez s'il y a des chambres qui sont attachées à ce groupe ! ", "Impossible de supprimer  !");
        //        return;
        //    }
        //    var canDeletes = MakfiData.GroupeChambre_CanDelete($"<groupeChambre><id>{ CurrentEtage.Id}</id></groupeChambre>");
        //    if (canDeletes.Count() == 0)
        //    {
        //        var param = MakfiData.GroupeChambre_Delete($"<groupeChambre><id>{CurrentEtage.Id}</id></groupeChambre>");
        //        if (param) Etages.Remove(CurrentEtage);
        //    }
        //    else
        //    {
        //        MessageBox.Show($" Suppression impossible du groupe : {CurrentEtage.Nom }", "Remarque !");
        //    }
        //}

        //// Méthodes OnCanExecuteCommand
        //private bool OnCanExecuteEtageAjouterChambreCommand()
        //{
        //    if (CurrentEtage == null || CurrentEtage.AutresChambres.Count == 0) return false;
        //    else return true;
        //}
        //private bool OnCanExecuteEtageSupprimerChambreCommand()
        //{
        //    if (CurrentEtage == null || CurrentEtage.Chambres.Count == 0) return false;
        //    else return true;
        //}
        //private bool OnCanExecuteOnEtageSaveCommand()
        //{
        //    if (CurrentEtage != null) return true;
        //    else return false;
        //}
        //private bool OnCanExecuteEtageDeleteCommand()
        //{
        //    if (CurrentEtage == null) return false;
        //    else return true;
        //}

        //#endregion

        //#region Load
        //public void Load_Etages()
        //{

        //    if (Reference_ViewModel.Header.CurrentHotel == null)
        //    {
        //        if (Etages != null) Etages.Clear();
        //        MessageBox.Show($"Aucun hôtel ne vous a été assigné  ", "Impossible d'enregistrer  !");
        //        if (CurrentEtage != null) Etages.Remove(CurrentEtage);
        //        return;
        //    }

        //    Etages = new ObservableCollection<Etage_VM>(
        //      MakfiData.GroupeChambre_Read($"<groupeChambre><hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel></groupeChambre>")
        //      .Select(x => new Etage_VM
        //      {
        //          Id = x.Id,
        //          Nom = x.Nom,
        //          Commentaire = x.Commentaire,
        //          SaveColor = "Navy"
        //      }).OrderBy(x => x.Nom).ToList());
        //    EtageCollectionView = new ListCollectionView(Etages);
        //    EtageCollectionView.Refresh();
        //}
        //public void Load_AllChambres()
        //{
        //    if (Reference_ViewModel.Header.CurrentHotel == null)
        //    {
        //        MessageBox.Show($"Aucun hôtel ne vous a été assigné  ", "Impossible d'enregistrer  !");
        //        if (CurrentEtage != null) Etages.Remove(CurrentEtage);
        //        return;
        //    }
        //    AllChambres = new ObservableCollection<Etage_VM>(
        //       MakfiData.ChambreByGroupe_Read($"<chambreByGroupe><hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel></chambreByGroupe>")
        //       .Select(x => new Etage_VM
        //       {
        //           Etage = x.Etage,
        //           Nom = x.Nom,
        //           Id = x.Id,
        //       }).ToList());
        //}
        //public void Load_ChambresByEtage()
        //{
        //    //if (CurrentEtage != null)
        //    //{
        //    //    CurrentEtage.Chambres = new ObservableCollection<ChambreByEtage_VM>(
        //    //        AllChambres.Where(c => c.GroupeChambre == CurrentEtage.Id)
        //    //        );
        //    //    CurrentEtage.ChambresListview = new ListCollectionView(CurrentEtage.Chambres);
        //    //    CurrentEtage.ChambresListview.Refresh();

        //    //    if (CurrentEtage.AutresChambres != null) CurrentEtage.AutresChambres.Clear();
        //    //    CurrentEtage.AutresChambres = new ObservableCollection<ChambreByEtage_VM>(
        //    //      AllChambres.Where(c => c.GroupeChambre != CurrentEtage.Id && !CurrentEtage.Chambres.Any(a => a.Id == c.IdDelaChambre))
        //    //                .Select(x => new ChambreByEtage_VM { IdDelaChambre = x.IdDelaChambre, NomChambre = x.NomChambre })
        //    //                .GroupBy(g => g.IdDelaChambre, g => g.NomChambre, (Key, g) => new ChambreByEtage_VM { IdDelaChambre = Key, NomChambre = g.ToList().ElementAt(0) })
        //    //      );
        //    //    CurrentEtage.AutresChambresListview = new ListCollectionView(CurrentEtage.AutresChambres);
        //    //    CurrentEtage.AutresChambresListview.Refresh();
        //    //}
        //}

        //#endregion
    }
}
