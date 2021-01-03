using Makrisoft.Makfi.Dal;
using Makrisoft.Makfi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Makrisoft.Makfi.ViewModels
{
    public class ChambreViewModel : ViewModel<Chambre_VM, Chambre>
    {
        #region Constructeur
        public ChambreViewModel()
        {
            EtatType = EntiteEnum.Chambre;
            SortDescriptions = new SortDescription[1] { new SortDescription("Nom", System.ComponentModel.ListSortDirection.Ascending) };
            Loads = LoadEnum.Etats | LoadEnum.Etages;
            Title = "Les chambres";

            Init<Chambre>();
        }
        #endregion

        #region DgSource
        public override IEnumerable<Chambre_VM> DgSource_Read()
        {
            return MakfiData.Read<Chambre>
                (
                "Chambre_Read",
                $"<chambres><hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel></chambres>",
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
                    Nom = x.Nom,
                    Etat = MakfiData.Etats.Where(e => e.Id == x.Etat).Single(),
                    Commentaire = x.Commentaire,
                    Etage = x.Etage,
                    SaveColor = "Navy"
                });
        }

        public override void DgSource_Save(string spName, string spParam)
        {
            base.DgSource_Save(
                "Chambre_Save",
                $@"<chambres><chambre>
                    <id>{CurrentDgSource.Id}</id>
                    <nom>{CurrentDgSource.Nom}</nom>
                    <etat>{CurrentDgSource.Etat.Id}</etat>
                    <commentaire>{CurrentDgSource.Commentaire}</commentaire>    
                    <hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel>
                </chambre></chambres>");
        }
        public override bool DgSource_Filter(object item)
        {
            var chambre = (Chambre_VM)item;
            return (FilterEtage == null || (chambre.Etage != null && Etages.Any(e => chambre.Etage.Value == FilterEtage.Id))) &&
                   (FilterEtat == null || Etats.Any(e => chambre.Etat.Id == FilterEtat.Id));
        }

        #endregion

        #region Command
        public override void OnAddCommand()
        {
            CurrentDgSource = new Chambre_VM
            {
                Id = null,
                Nom = "( A définir !)",
                Etat = MakfiData.Etats.Where(e => e.Entite == EntiteEnum.Chambre && e.Libelle == "Disponible").Single()
            };
            DgSource.Add(CurrentDgSource);
        }
        public override void OnDeleteCommand(string spName, string spParam)
        {
            base.OnDeleteCommand(
                 "Chambre_Delete",
                 $"<chambres><chambre><id>{CurrentDgSource.Id}</id></chambre></chambres>");
        }
        public override void OnChangeViewCommand()
        {
            Reference_ViewModel.Main.ViewSelected = ViewEnum.Etage;
        }
        public override bool OnCanExecuteChangeView() { return true; }

        #endregion
    }
}
