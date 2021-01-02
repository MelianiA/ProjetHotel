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
    public class EtageViewModel : ViewModel<Etage_VM>
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

            Init();

            // Icommand
            AjouterChambreCommand = new RelayCommand(p => OnAjouterChambreCommand(), p => OnCanExecuteAjouterChambreCommand());
            SupprimerChambreCommand = new RelayCommand(p => OnSupprimerChambreCommand(), p => OnCanExecuteSupprimerChambreCommand());
        }
        #endregion

        #region DgSource
        public override IEnumerable<Etage_VM> DgSource_Read()
        {
            var etages = MakfiData.Read<Etage>
                (
                "Etage_Read",
                $"<groupeChambres><hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel></groupeChambres>",
                e =>
                {
                    e.Id = (Guid)MakfiData.Reader["Id"];
                    e.Nom = MakfiData.Reader["Nom"] as string;
                    e.Commentaire = MakfiData.Reader["Commentaire"] as string;
                })
              .Select(x => new Etage_VM
              {
                  Id = x.Id,
                  Nom = x.Nom,
                  Commentaire = x.Commentaire,
                  SaveColor = "Navy",
                  Chambres = new ObservableCollection<Chambre_VM>(
                      MakfiData.Read<Chambre>(
                            "Chambre_Read",
                            $"<chambres><hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel><groupeChambre>{x.Id}</groupeChambre></chambres>",
                            e =>
                            {
                                e.Id = (Guid)MakfiData.Reader["Id"];
                                e.Nom = MakfiData.Reader["Nom"] as string;
                                e.Etat = (Guid)MakfiData.Reader["Etat"];
                                e.Etage = MakfiData.Reader["GroupeChambre"] as Guid?;
                                e.Commentaire = MakfiData.Reader["Commentaire"] as string;
                            })
                  .Select(c => new Chambre_VM
                  {
                      Id = c.Id,
                      Nom = c.Nom,
                  })),
                  AutresChambres = new ObservableCollection<Chambre_VM>(
                      MakfiData.Read<Chambre>(
                          "Chambre_Read",
                          $"<chambres><hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel><notGroupeChambre>{x.Id}</notGroupeChambre></chambres>",
                          e =>
                          {
                              e.Id = (Guid)MakfiData.Reader["Id"];
                              e.Nom = MakfiData.Reader["Nom"] as string;
                              e.Etat = (Guid)MakfiData.Reader["Etat"];
                              e.Etage = MakfiData.Reader["GroupeChambre"] as Guid?;
                              e.Commentaire = MakfiData.Reader["Commentaire"] as string;
                          })
                          .Select(c => new Chambre_VM
                          {
                              Id = c.Id,
                              Nom = c.Nom,
                          }))
              });
            //foreach(var etage in etages)
            //{
            //    etage.Chambres = new ObservableCollection<Chambre_VM>(MakfiData.Chambre_Read($"<chambres><hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel><groupeChambre>{etage.Id}</groupeChambre></chambres>")
            //              .Select(x => new Chambre_VM
            //              {
            //                  Id = x.Id,
            //                  Nom = x.Nom,
            //              }));
            //    etage.AutresChambres = new ObservableCollection<Chambre_VM>(MakfiData.Chambre_Read($"<chambres><hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel><notGroupeChambre>{etage.Id}</notGroupeChambre></chambres>")
            //              .Select(x => new Chambre_VM
            //              {
            //                  Id = x.Id,
            //                  Nom = x.Nom,
            //              }));
            //}
            return etages;
        }

        public override void DgSource_Save()
        {
            var param = $@"
                        <groupeChambres>
                            <id>{CurrentDgSource.Id}</id>
                            <nom>{CurrentDgSource.Nom}</nom>
                            <hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel>
                            <chambres>{string.Join("", CurrentDgSource.Chambres.Select(c => $"<chambre>{c.Id}</chambre>"))}</chambres>
                            <commentaire>{CurrentDgSource.Commentaire}</commentaire>    
                          </groupeChambres>";
            var ids = MakfiData.Save<Etage>("Etage_Save", param);

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
                Id = null,
                Nom = "( A définir !)",
            };
            DgSource.Add(CurrentDgSource);
        }
        public override void OnDeleteCommand(string spName, string spParam)
        {
            spName = "Etage_CanDelete";
            spParam = $"<groupechambres><id>{CurrentDgSource.Id}</id></groupechambres>";

            base.OnDeleteCommand(spName, spParam);
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
