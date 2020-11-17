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
            // ListeView
            Load_GroupeChambres();
            Load_AllChambres();
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
                OnPropertyChanged("AllGroupeChambre");

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
                if (CurrentGroupeChambre.SaveColor == "Navy")
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
            if (CurrentGroupeChambre.Nom == null)
            {
                MessageBox.Show($"Impossible de sauvgarder cette chambre !", "Remarque !");
                return;
            }
            //Etape01
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
            //Etape02
            var chambreGroupeChambre_Delete = MakfiData.ChambreGroupeChambre_Delete($"<chambreGroupeChambre><groupeChambre>{CurrentGroupeChambre.Id}</groupeChambre></chambreGroupeChambre>");
            if (!chambreGroupeChambre_Delete) throw new Exception("Rien n'a été sauvgardé ! ");

            //Etape03
            //Sauvgarder (CurrentGroupeChambre.ChambreCurrentGroupeListview) dans la table ChambreGroupeChambre
            foreach (var item in CurrentGroupeChambre.ChambreCurrentGroupe)
            {
                param = $@"
                    <chambreGroupeChambre>
                        <chambre>{item.IdDelaChambre}</chambre>
                        <groupeChambre>{CurrentGroupeChambre.Id}</groupeChambre>    
                      </chambreGroupeChambre>";
                ids = MakfiData.ChambreGroupeChambre_Save(param);
                if (ids.Count == 0) throw new Exception("Rien n'a été sauvgardé ! ");
            }
            CurrentGroupeChambre.SaveColor = "Navy";
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
        // Divers


        #endregion

        #region Load
        public void Load_GroupeChambres()
        {
            GroupeChambres = new ObservableCollection<GroupeChambre_VM>(
              MakfiData.GroupeChambre_Read()
              .Select(x => new GroupeChambre_VM
              {
                  Id = x.Id,
                  Nom = x.Nom,
                  Commentaire = x.Commentaire,
                  SaveColor = "Navy"
              }).ToList());
            GroupeChambreCollectionView = new ListCollectionView(GroupeChambres);
            GroupeChambreCollectionView.Refresh();
        }
        public void Load_AllChambres()
        {
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
