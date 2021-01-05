using Makrisoft.Makfi.Dal;
using Makrisoft.Makfi.Models;
using Makrisoft.Makfi.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Makrisoft.Makfi.ViewModels
{
    public class EtageViewModel : ViewModel<Etage_VM, Etage>
    {

        public Chambre_VM CurrentChambreDispo
        {
            get { return currentChambreDispo; }
            set { currentChambreDispo = value; }
        }
        private Chambre_VM currentChambreDispo;
        public Chambre_VM CurrentChambre
        {
            get { return currentChambre; }
            set { currentChambre = value; }
        }
        private Chambre_VM currentChambre;

        #region Constructeur
        public EtageViewModel()
        {
            EtatType = EntiteEnum.Intervention;
            SortDescriptions = new SortDescription[1] { new SortDescription("Nom", System.ComponentModel.ListSortDirection.Ascending) };
            Loads = LoadEnum.Etats | LoadEnum.Chambres;
            Title = "Les groupes de chambres";

            Init<Etage>();

            // Icommand
            AjouterChambreCommand = new RelayCommand(p => OnAjouterChambreCommand(), p => OnCanExecuteAjouterChambreCommand());
            SupprimerChambreCommand = new RelayCommand(p => OnSupprimerChambreCommand(), p => OnCanExecuteSupprimerChambreCommand());
        }
        #endregion

        #region DgSource
        public override IEnumerable<Etage_VM> DgSource_Read()
        {
            var etages = MakfiData.Crud<Etage>
                (
                "GroupeChambre_Read",
                $"<groupeChambres><hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel></groupeChambres>",
                e =>
                {
                    e.Id = (Guid)MakfiData.Reader["Id"];
                    e.Nom = MakfiData.Reader["Nom"] as string;
                    e.Commentaire = MakfiData.Reader["Commentaire"] as string;
                })
              .Select(x => new Etage_VM(x));
            return etages;
        }

        public override void DgSource_Save(string spName, string spParam)
        {
            base.DgSource_Save(
                "GroupeChambre_Save",
                $@"
                <groupeChambres><groupeChambre>
                    <id>{CurrentDgSource.Id}</id>
                    <nom>{CurrentDgSource.Nom}</nom>
                    <hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel>
                    <chambres>{string.Join("", CurrentDgSource.Chambres.Select(c => $"<chambre>{c.Id}</chambre>"))}</chambres>
                    <commentaire>{CurrentDgSource.Commentaire}</commentaire>    
                </groupeChambre></groupeChambres>");
        }
        #endregion

        #region Command
        public override void OnAddCommand()
        {
            CurrentDgSource = new Etage_VM
            {
                Id = null,
                Nom = "( A définir !)",
                Chambres = new ObservableCollection<Chambre_VM>(),
                AutresChambres = new ObservableCollection<Chambre_VM>(Chambres)
            };
            DgSource.Add(CurrentDgSource);
        }
        public override void OnDeleteCommand(string spName, string spParam)
        {
            base.OnDeleteCommand(
                "GroupeChambre_Delete",
                $"<groupechambres><groupechambre><id>{CurrentDgSource.Id}</id></groupechambre></groupechambres>");
        }
        //ICommand
        public ICommand AjouterChambreCommand { get; set; }
        public ICommand SupprimerChambreCommand { get; set; }
        private void OnAjouterChambreCommand()
        {
            CurrentDgSource.Chambres.Add(CurrentChambreDispo);
            CurrentDgSource.AutresChambres.Remove(CurrentChambreDispo);
            CurrentDgSource.SaveColor = "Red";
        }
        private void OnSupprimerChambreCommand()
        {
            CurrentDgSource.AutresChambres.Add(CurrentChambre);
            CurrentDgSource.Chambres.Remove(CurrentChambre);
            CurrentDgSource.SaveColor = "Red";
        }

        private bool OnCanExecuteAjouterChambreCommand()
        {
            if (CurrentDgSource == null || CurrentDgSource.AutresChambres.Count == 0) return false;
            else return true;
        }
        private bool OnCanExecuteSupprimerChambreCommand()
        {
            if (CurrentDgSource == null || CurrentDgSource.Chambres.Count == 0) return false;
            else return true;
        }
        #endregion

    }
}
