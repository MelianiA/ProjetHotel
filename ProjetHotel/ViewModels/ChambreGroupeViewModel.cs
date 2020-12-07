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
    public class ChambreGroupeViewModel : ViewModelBase
    {
        #region Constructeur
        public ChambreGroupeViewModel()
        {
            // Icommand
            AddChambreAuGroupe = new RelayCommand(p => OnAddChambreAuGroupe(), p => OnCanExecuteAddChambreAuGroupe());
            RemoveChambreAuGroupe = new RelayCommand(p => OnRemoveChambreAuGroupe(), p => OnCanExecuteRemoveChambreAuGroupe());
            ChambreGroupeModifiedSaveCommand = new RelayCommand(p => OnChambreGroupeModifiedSaveCommand(), p => OnCanExecuteChambreGroupeModifiedSaveCommand());
            ChambreGroupeSelectedAddCommand = new RelayCommand(p => OnChambreGroupeSelectedAddCommand(), p => true);
            ChambreGroupeSelectedDeleteCommand = new RelayCommand(p => OnChambreGroupeSelectedDeleteCommand(), p => OnCanExecuteChambreGroupeSelectedDeleteCommand());

            // ListeView
            if (Reference_ViewModel.Header.CurrentHotel != null)
            {
                Load_GroupeChambres();
                Load_AllChambres();
            }
              
            //Load_ChambreCurrentGroupe();

        }

        #endregion

        #region Binding
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
            get
            {
                return currentGroupeChambre;
            }
            set
            {
                currentGroupeChambre = value;
                if (CurrentGroupeChambre != null && CurrentGroupeChambre.SaveColor == "Navy")
                    Load_ChambreCurrentGroupe();
                OnPropertyChanged("CurrentGroupeChambre");
            }
        }
        private GroupeChambre_VM currentGroupeChambre;
        public ListCollectionView GroupeChambreCollectionView
        {
            get { return groupeChambreCollectionView; }
            set { groupeChambreCollectionView = value; OnPropertyChanged("GroupeChambreCollectionView"); }
        }
        private ListCollectionView groupeChambreCollectionView;

        //ChambreByGroupe 
        public ObservableCollection<ChambreByGroupe_VM> AllChambres
        {
            get { return allChambres; }
            set
            {
                allChambres = value;
                OnPropertyChanged("AllChambres");

            }
        }
        private ObservableCollection<ChambreByGroupe_VM> allChambres;

        #endregion

        #region Commands
        //ICommand
        public ICommand AddChambreAuGroupe { get; set; }
        public ICommand RemoveChambreAuGroupe { get; set; }
        public ICommand ChambreGroupeModifiedSaveCommand { get; set; }
        public ICommand ChambreGroupeSelectedAddCommand { get; set; }
        public ICommand ChambreGroupeSelectedDeleteCommand { get; set; }

        // Méthodes OnCommand
        private void OnAddChambreAuGroupe()
        {
            CurrentGroupeChambre.ChambreCurrentGroupe.Add(CurrentGroupeChambre.CurrentNotChambreCG);
            CurrentGroupeChambre.ChambreNotCurrentGroupe.Remove(CurrentGroupeChambre.CurrentNotChambreCG);
            CurrentGroupeChambre.SaveColor = "Red";
        }
        private void OnRemoveChambreAuGroupe()
        {
            CurrentGroupeChambre.ChambreNotCurrentGroupe.Add(CurrentGroupeChambre.CurrentChambreCG);
            CurrentGroupeChambre.ChambreCurrentGroupe.Remove(CurrentGroupeChambre.CurrentChambreCG);
            CurrentGroupeChambre.SaveColor = "Red";
        }
        private void OnChambreGroupeModifiedSaveCommand()
        {
            if (Reference_ViewModel.Header.CurrentHotel == null)
            {
                MessageBox.Show($"Aucun hôtel ne vous a été assigné  ", "Impossible d'enregistrer  !");
                GroupeChambres.Remove(CurrentGroupeChambre);
                return;
            }
            if (CurrentGroupeChambre.Nom == null || CurrentGroupeChambre.Nom == "(A définir ! )")
            {
                MessageBox.Show($"Impossible de sauvgarder ce groupe !" +
                    $"\nVous devez choisir un nom pour ce groupe de chambre.", "Remarque !");
                return;
            }
       
            //Etape01 : Insertion dans la table GroupeChambre
            Guid? monID = null;
            if (CurrentGroupeChambre.Id != default) monID = CurrentGroupeChambre.Id;
            var param = $@"
                    <groupeChambre>
                        <id>{monID}</id>
                        <nom>{CurrentGroupeChambre.Nom}</nom>
                        <commentaire>{CurrentGroupeChambre.Commentaire}</commentaire>    
                      </groupeChambre>";
            var ids = MakfiData.GroupeChambre_Save(param);
            if (ids.Count == 0) throw new Exception("Rien n'a été sauvgardé ! ");
            if (monID == null) CurrentGroupeChambre.Id = ids[0].Id;
            //Etape02
            var chambreGroupeChambre_Delete = MakfiData.ChambreGroupeChambre_Delete(
                $"<chambreGroupeChambre><groupeChambre>{CurrentGroupeChambre.Id}</groupeChambre><hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel></chambreGroupeChambre>"
                );
            if (!chambreGroupeChambre_Delete) throw new Exception("Rien n'a été sauvgardé ! ");
            //Etape03
            foreach (var item in CurrentGroupeChambre.ChambreCurrentGroupe)
            {
                param = $@"
                    <chambreGroupeChambre>
                        <chambre>{item.IdDelaChambre}</chambre>
                        <groupeChambre>{ids[0].Id}</groupeChambre>    
                      </chambreGroupeChambre>";
                var ids2 = MakfiData.ChambreGroupeChambre_Save(param);
                if (ids2.Count == 0) throw new Exception("Rien n'a été sauvgardé ! ");
            }
            CurrentGroupeChambre.SaveColor = "Navy";
            Load_AllChambres();
            Load_ChambreCurrentGroupe();
            Reference_ViewModel.Chambre.Load_Chambres();
        }
        private void OnChambreGroupeSelectedAddCommand()
        {
            CurrentGroupeChambre = new GroupeChambre_VM { Nom = "(A définir ! )" };
            Load_ChambreCurrentGroupe();
            GroupeChambres.Add(CurrentGroupeChambre);
        }
        private void OnChambreGroupeSelectedDeleteCommand()
        {
            if (CurrentGroupeChambre.ChambreCurrentGroupe.Count != 0)
            {
                MessageBox.Show($"Vérifiez s'il y a des chambres qui sont attachées à ce groupe ! ", "Impossible de supprimer  !");
                return;
            }
            var canDeletes = MakfiData.GroupeChambre_CanDelete($"<groupeChambre><id>{ CurrentGroupeChambre.Id}</id></groupeChambre>");
            if (canDeletes.Count() == 0)
            {
                var param = MakfiData.GroupeChambre_Delete($"<groupeChambre><id>{CurrentGroupeChambre.Id}</id></groupeChambre>");
                if (param) GroupeChambres.Remove(CurrentGroupeChambre);
            }
            else
            {
                MessageBox.Show($" Suppression impossible du groupe : {CurrentGroupeChambre.Nom }", "Remarque !");
            }
        }

        // Méthodes OnCanExecuteCommand
        private bool OnCanExecuteAddChambreAuGroupe()
        {
            if (CurrentGroupeChambre == null || CurrentGroupeChambre.ChambreNotCurrentGroupe.Count == 0) return false;
            else return true;
        }
        private bool OnCanExecuteRemoveChambreAuGroupe()
        {
            if (CurrentGroupeChambre == null || CurrentGroupeChambre.ChambreCurrentGroupe.Count == 0) return false;
            else return true;
        }
        private bool OnCanExecuteChambreGroupeModifiedSaveCommand()
        {
            if (CurrentGroupeChambre != null) return true;
            else return false;
        }
        private bool OnCanExecuteChambreGroupeSelectedDeleteCommand()
        {
            if (CurrentGroupeChambre == null) return false;
            else return true;
        }
  
        #endregion

        #region Load
        public void Load_GroupeChambres()
        {

            if (Reference_ViewModel.Header.CurrentHotel == null)
            {
                if(GroupeChambres!=null) GroupeChambres.Clear();
                MessageBox.Show($"Aucun hôtel ne vous a été assigné  ", "Impossible d'enregistrer  !");
                if (CurrentGroupeChambre != null)  GroupeChambres.Remove(CurrentGroupeChambre);
                return;
            }
             
            GroupeChambres = new ObservableCollection<GroupeChambre_VM>(
              MakfiData.GroupeChambre_Read($"<groupeChambre><hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel></groupeChambre>")
              .Select(x => new GroupeChambre_VM
              {
                  Id = x.Id,
                  Nom = x.Nom,
                  Commentaire = x.Commentaire,
                  SaveColor = "Navy"
              }).OrderBy(x => x.Nom).ToList());
            GroupeChambreCollectionView = new ListCollectionView(GroupeChambres);
            GroupeChambreCollectionView.Refresh();
        }
        public void Load_AllChambres()
        {
            if (Reference_ViewModel.Header.CurrentHotel == null)
            {
                MessageBox.Show($"Aucun hôtel ne vous a été assigné  ", "Impossible d'enregistrer  !");
                if (CurrentGroupeChambre != null) GroupeChambres.Remove(CurrentGroupeChambre);
                return;
            }
            AllChambres = new ObservableCollection<ChambreByGroupe_VM>(
               MakfiData.ChambreByGroupe_Read($"<chambreByGroupe><hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel></chambreByGroupe>")
               .Select(x => new ChambreByGroupe_VM
               {
                   GroupeChambre = x.GroupeChambre,
                   Nom = x.Nom,
                   IdDelaChambre = x.IdDelaChambre,
                   NomChambre = x.NomChambre
               }).ToList());

        }
        public void Load_ChambreCurrentGroupe()
        {
            if (CurrentGroupeChambre != null)
            {
                CurrentGroupeChambre.ChambreCurrentGroupe = new ObservableCollection<ChambreByGroupe_VM>(
                    AllChambres.Where(c => c.GroupeChambre == CurrentGroupeChambre.Id)
                    );
                CurrentGroupeChambre.ChambreCurrentGroupeListview = new ListCollectionView(CurrentGroupeChambre.ChambreCurrentGroupe);
                CurrentGroupeChambre.ChambreCurrentGroupeListview.Refresh();

                if (CurrentGroupeChambre.ChambreNotCurrentGroupe != null) CurrentGroupeChambre.ChambreNotCurrentGroupe.Clear();
                CurrentGroupeChambre.ChambreNotCurrentGroupe = new ObservableCollection<ChambreByGroupe_VM>(
                  AllChambres.Where(c => c.GroupeChambre != CurrentGroupeChambre.Id && !CurrentGroupeChambre.ChambreCurrentGroupe.Any(a => a.IdDelaChambre == c.IdDelaChambre))
                            .Select(x => new ChambreByGroupe_VM { IdDelaChambre = x.IdDelaChambre, NomChambre = x.NomChambre })
                            .GroupBy(g => g.IdDelaChambre, g => g.NomChambre, (Key, g) => new ChambreByGroupe_VM { IdDelaChambre = Key, NomChambre = g.ToList().ElementAt(0) })
                  );
                CurrentGroupeChambre.ChambreNotCurrentGroupeListview = new ListCollectionView(CurrentGroupeChambre.ChambreNotCurrentGroupe);
                CurrentGroupeChambre.ChambreNotCurrentGroupeListview.Refresh();
            }
        }

        #endregion
    }
}
