using Makrisoft.Makfi.Dal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Makrisoft.Makfi.ViewModels
{
    public class ChambreViewModel : ViewModel<Chambre_VM>
    {
        #region Constructeur
        public ChambreViewModel()
        {
            EtatType = EntiteEnum.Chambre;
            SortDescriptions = new SortDescription[1] { new SortDescription("Nom", System.ComponentModel.ListSortDirection.Ascending) };
            Loads = LoadEnum.Etats | LoadEnum.Etages;
            Title = "Les chambres";

            Init();
        }
        #endregion

        #region DgSource
        public override IEnumerable<Chambre_VM> DgSource_Read()
        {
            return MakfiData.Chambre_Read($"<chambres><hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel></chambres>")
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

        public override void DgSource_Save()
        {
            var param = $@"
                <chambres>
                    <id>{CurrentDgSource.Id}</id>
                    <nom>{CurrentDgSource.Nom}</nom>
                    <etat>{CurrentDgSource.Etat.Id}</etat>
                    <commentaire>{CurrentDgSource.Commentaire}</commentaire>    
                    <hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel>
                </chambres>";
            var ids = MakfiData.Chambre_Save(param);
            if (ids.Count == 0) throw new Exception("ChambreViewModel.DgSource_Save");
            CurrentDgSource.Id = ids[0].Id;
            CurrentDgSource.SaveColor = "Navy";
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
        public override void OnDeleteCommand()
        {
            var canDeletes = MakfiData.Chambre_CanDelete($"<chambres><id>{CurrentDgSource.Id}</id></chambres>");
            if (canDeletes.Count() == 0)
            {
                var param = MakfiData.Chambre_Delete($"<chambres><id>{CurrentDgSource.Id}</id></chambres>");
                if (param) DgSource.Remove(CurrentDgSource);
            }
            else
                MessageBox.Show($"Suppression impossible", "Erreur");
        }
        public override void OnChangeViewCommand()
        {
            Reference_ViewModel.Main.ViewSelected = ViewEnum.Etage;
        }
        public override bool OnCanExecuteChangeView() { return true; }

        #endregion
    }
}
