using Makrisoft.Makfi.Dal;
using Makrisoft.Makfi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Makrisoft.Makfi.ViewModels
{
    public class InterventionViewModel : ViewModel<Intervention_VM, Intervention>
    {
        #region Constructeur
        public InterventionViewModel()
        {
            EtatType = EntiteEnum.Intervention;
            SortDescriptions = new SortDescription[1] { new SortDescription("Date1", System.ComponentModel.ListSortDirection.Descending) };
            Loads = LoadEnum.Etats | LoadEnum.DateDebut | LoadEnum.DateFin;
            Title = "Les interventions";

            Init<Intervention>();
        }
        #endregion

        #region DgSource
        public override IEnumerable<Intervention_VM> DgSource_Read()
        {
            return MakfiData.Crud<Intervention>(
                "Intervention_Read",
                $"<interventions><hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel></interventions>",
                e =>
                {
                    e.Id = (Guid)MakfiData.Reader["Id"];
                    e.Libelle = MakfiData.Reader["Libelle"] as string;
                    e.Etat = (Guid)MakfiData.Reader["Etat"];
                    e.Date1 = (DateTime)MakfiData.Reader["Date1"];
                    e.Commentaire = MakfiData.Reader["Commentaire"] as string;
                    e.IsModele = (bool)MakfiData.Reader["Model"];
                })
              .Select(x => new Intervention_VM
              {
                  Id = x.Id,
                  Libelle = x.Libelle,
                  Etat = MakfiData.Etats.Where(e => e.Id == x.Etat).Single(),
                  Date1 = x.Date1,
                  Commentaire = x.Commentaire,
                  Model = x.IsModele,
                  SaveColor = "Navy"
              }).OrderBy(x => x.Libelle).ToList();
        }
        public override void DgSource_Save(string spName, string spParam)
        {
            base.DgSource_Save(
                "Intervention_Save",
                $@"
                    <interventions><intervention>
                        <id>{CurrentDgSource.Id}</id>
                        <libelle>{CurrentDgSource.Libelle}</libelle>
                        <commentaire>{CurrentDgSource.Commentaire}</commentaire>    
						<hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel>
                        <date1>{CurrentDgSource.Date1}</date1>    
                        <model>{CurrentDgSource.Model}</model>   
                        <etat>{CurrentDgSource.Etat.Id}</etat> 
                     </intervention></interventions>");
        }
        public override bool DgSource_Filter(object item)
        {
            var intervention = (Intervention_VM)item;
            return (FilterDateDebut == null || intervention.Date1 >= FilterDateDebut) &&
                   (FilterDateFin == null || intervention.Date1 <= FilterDateFin) &&
                   (FilterEtat == null || Etats.Any(e => intervention.Etat.Id == FilterEtat.Id));
        }

        #endregion

        #region Command
        public override void OnAddCommand()
        {
            CurrentDgSource =
                new Intervention_VM
                {
                    Date1 = DateTime.Now,
                    Etat = MakfiData.Etats.Where(e => e.Entite == EntiteEnum.Intervention && e.Libelle == "None").Single(),
                    Libelle = $"Intervention du {DateTime.Now.ToShortDateString()}{Properties.Settings.Default.Autocar}",
                    Model = true
                };
            DgSource.Add(CurrentDgSource);
        }
        public override void OnDeleteCommand(string spName, string spParam)
        {
            base.OnDeleteCommand(
                 "Intervention_Delete",
                 $"<interventions><intervention><id>{CurrentDgSource.Id}</id></intervention></interventions>");
        }
        public override void OnChangeViewCommand()
        {
            Reference_ViewModel.Main.ViewSelected = ViewEnum.InterventionDetail;
            RetourIntervention = true;
        }
        #endregion
    }
}
