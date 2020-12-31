using Makrisoft.Makfi.Dal;
using Makrisoft.Makfi.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace Makrisoft.Makfi.ViewModels
{
    public class InterventionDetailViewModel : ViewModel<InterventionDetail_VM>
    {
        #region Constructeur
        public InterventionDetailViewModel()
        {
            EtatType = EntiteEnum.InterventionDetail;
            SortDescriptions = new SortDescription[1] { new SortDescription("Chambre.Nom", ListSortDirection.Ascending) };
            Loads = LoadEnum.Etats | LoadEnum.Employes | LoadEnum.Chambres | LoadEnum.Etages;

            Init();

            AddAllCommand = new RelayCommand(p => OnAddAllCommand(), p => true);
            SaveAllCommand = new RelayCommand(p => OnSaveAllCommand(), p => OnCanExecuteSaveAllCommand());
            DeleteAllCommand = new RelayCommand(p => OnDeleteAllCommand(), p => OnCanExecuteDeleteAllCommand());

        }

        public override void Load(ViewEnum exView)
        {
            Title = Reference_ViewModel.Intervention.CurrentDgSource.Libelle;


            if (exView == ViewEnum.InterventionAjouter)
            {
                foreach (var interD in Reference_ViewModel.InterventionAjouter.DgSource)
                    Reference_ViewModel.InterventionDetail.DgSource.Add(interD);
            }
            else
                base.Load(exView);
        }

        #endregion

        #region DgSource
        public override IEnumerable<InterventionDetail_VM> DgSource_Read()
        {
            if (Reference_ViewModel.Intervention.CurrentDgSource == null) return null;
            return MakfiData
                .InterventionDetail_Read($"<interventionDetails><intervention>{Reference_ViewModel.Intervention.CurrentDgSource.Id}</intervention></interventionDetails>")
                .Select(x => new InterventionDetail_VM
                {
                    Id = x.Id,
                    Employe = Employes.Where(c => c.Id == x.Employe.Id).Single(),
                    Chambre = Chambres.Where(c => c.Id == x.Chambre.Id).FirstOrDefault(),
                    Libelle = Reference_ViewModel.Intervention.CurrentDgSource.Libelle,
                    Commentaire = x.Commentaire,
                    Etat = MakfiData.Etats.Where(e => e.Id == x.Etat).Single(),
                    SaveColor = "Navy"
                })
                .OrderBy(x => x.Libelle)
                .ToList();
        }
        public override void DgSource_Save()
        {
            var param = $@"<interventionDetails><intervention>{Reference_ViewModel.Intervention.CurrentDgSource.Id}</intervention> <interventionDetail>
                            <id>{CurrentDgSource.Id}</id>
                            <employeAffecte>{CurrentDgSource.Employe.Id}</employeAffecte>
                            <commentaire>{CurrentDgSource.Commentaire}</commentaire>    
                            <chambreAffectee>{CurrentDgSource.Chambre.Id}</chambreAffectee>
                            <etat>{CurrentDgSource.Etat.Id}</etat> 
                           </interventionDetail></interventionDetails>";
            var ids = MakfiData.InterventionDetail_Save(param);
            if (ids.Count == 0) throw new Exception("InterventionDetailViewModel.DgSource_Save:1");
            CurrentDgSource.Id = ids[0].Id;
            CurrentDgSource.SaveColor = "Navy";

            param = $@"
                    <intervention>
                        <id>{Reference_ViewModel.Intervention.CurrentDgSource.Id}</id>
                        <libelle>{Reference_ViewModel.Intervention.CurrentDgSource.Libelle}</libelle>
                        <commentaire>{Reference_ViewModel.Intervention.CurrentDgSource.Commentaire}</commentaire>    
						<hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel>
                        <date1>{Reference_ViewModel.Intervention.CurrentDgSource.Date1}</date1>    
                        <model>{Reference_ViewModel.Intervention.CurrentDgSource.Model}</model>   
                        <etat>{SommeEtat().Id}</etat> 
                     </intervention>";

            var ids2 = MakfiData.Intervention_Save(param);
            if (ids2.Count == 0) throw new Exception("InterventionDetailViewModel.DgSource_Save:2");
        }
        public override bool DgSource_Filter(object item)
        {
            var interventionDetail = (InterventionDetail_VM)item;
            return (FilterEtage == null || FilterEtage.Chambres.Any(e => e.Id == interventionDetail.Chambre.Id)) &&
                   (FilterEmploye == null || Employes.Any(e => interventionDetail.Employe.Id == FilterEmploye.Id)) &&
                   (FilterEtat == null || Etats.Any(e => interventionDetail.Etat.Id == FilterEtat.Id));
        }
        #endregion

        #region Command
        public ICommand AddAllCommand { get; set; }
        public ICommand SaveAllCommand { get; set; }


        public ICommand DeleteAllCommand { get; set; }

        public bool OnCanExecuteSaveAllCommand()
        {
            return DgSource.Any(i => i.SaveColor == "Red");
        }

        private bool OnCanExecuteDeleteAllCommand()
        {
            return DgSource.Count > 0;
        }

        public override void OnAddCommand()
        {
            CurrentDgSource = new InterventionDetail_VM
            {
                Id = null,
                Etat = MakfiData.Etats.Where(e => e.Libelle == "None" && e.Entite == EtatType).SingleOrDefault(),
            };
            DgSource.Add(CurrentDgSource);
        }

        public override void OnDeleteCommand()
        {
            if (CurrentDgSource.Id != null)
            {
                MakfiData.InterventionDetails_Delete($"<interventionDetails><id>{CurrentDgSource.Id}</id></interventionDetails>");
            }
            DgSource.Remove(CurrentDgSource);

            // Maj intervention Etat 
            var param = $@"
                    <intervention>
                        <id>{Reference_ViewModel.Intervention.CurrentDgSource.Id}</id>
                        <libelle>{Reference_ViewModel.Intervention.CurrentDgSource.Libelle}</libelle>
                        <commentaire>{Reference_ViewModel.Intervention.CurrentDgSource.Commentaire}</commentaire>    
						<hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel>
                        <date1>{Reference_ViewModel.Intervention.CurrentDgSource.Date1}</date1>    
                        <model>{Reference_ViewModel.Intervention.CurrentDgSource.Model}</model>   
                        <etat>{SommeEtat().Id}</etat> 
                     </intervention>";

            var ids2 = MakfiData.Intervention_Save(param);
            if (ids2.Count == 0) throw new Exception("InterventionDetailViewModel.DgSource_Save:2");
        }

        private void OnAddAllCommand()
        {
            Reference_ViewModel.Main.ViewSelected = ViewEnum.InterventionAjouter;
        }
        private void OnSaveAllCommand()
        {
            var param =
                $"<interventionDetails><intervention>{Reference_ViewModel.Intervention.CurrentDgSource.Id}</intervention>" +
                string.Join("",
                    DgSource.Where(d => d.SaveColor == "Red")
                    .Select(x => $@"
                    <interventionDetail>
                            <id>{x.Id}</id>
                            <employeAffecte>{x.Employe.Id}</employeAffecte>
                            <commentaire>{x.Commentaire}</commentaire>    
                            <chambreAffectee>{x.Chambre.Id}</chambreAffectee>
                            <intervention>{Reference_ViewModel.Intervention.CurrentDgSource.Id}</intervention>    
                            <etat>{x.Etat.Id}</etat> 
                    </interventionDetail>
                    "))
                + "</interventionDetails>";
            var ids = MakfiData.InterventionDetail_Save(param); // insere = insert | update
            if (ids.Count == 0) throw new Exception("InterventionDetailViewModel.OnSaveAllCommand");
            ids = ids.Where(x => x.Insere).ToList();
            int i = 0;
            foreach (var d in DgSource)
            {
                if (d.Id == null)
                {
                    d.Id = ids[i].Id; i++;
                }
                d.SaveColor = "Navy";
            }

            // Maj intervention Etat 
            param = $@"
                    <intervention>
                        <id>{Reference_ViewModel.Intervention.CurrentDgSource.Id}</id>
                        <libelle>{Reference_ViewModel.Intervention.CurrentDgSource.Libelle}</libelle>
                        <commentaire>{Reference_ViewModel.Intervention.CurrentDgSource.Commentaire}</commentaire>    
						<hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel>
                        <date1>{Reference_ViewModel.Intervention.CurrentDgSource.Date1}</date1>    
                        <model>{Reference_ViewModel.Intervention.CurrentDgSource.Model}</model>   
                        <etat>{SommeEtat().Id}</etat> 
                     </intervention>";

            var ids2 = MakfiData.Intervention_Save(param);
            if (ids2.Count == 0) throw new Exception("InterventionDetailViewModel.DgSource_Save:2");
        }

        private void OnDeleteAllCommand()
        {
            DgSource.Clear();
            MakfiData.InterventionDetails_Delete($"<interventionDetails><intervention>{Reference_ViewModel.Intervention.CurrentDgSource.Id}</intervention></interventionDetails>");

            // Maj intervention Etat 
            var param = $@"
                    <intervention>
                        <id>{Reference_ViewModel.Intervention.CurrentDgSource.Id}</id>
                        <libelle>{Reference_ViewModel.Intervention.CurrentDgSource.Libelle}</libelle>
                        <commentaire>{Reference_ViewModel.Intervention.CurrentDgSource.Commentaire}</commentaire>    
						<hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel>
                        <date1>{Reference_ViewModel.Intervention.CurrentDgSource.Date1}</date1>    
                        <model>{Reference_ViewModel.Intervention.CurrentDgSource.Model}</model>   
                        <etat>{SommeEtat().Id}</etat> 
                     </intervention>";

            var ids2 = MakfiData.Intervention_Save(param);
            if (ids2.Count == 0) throw new Exception("InterventionDetailViewModel.OnDeleteAllCommand");


        }
        #endregion

        #region Divers
        public Etat_VM SommeEtat()
        {
            if (DgSource.Count == 0 || DgSource.All(i => i.Etat.Libelle == "None"))
                return MakfiData.Etats.Where(e => e.Libelle == "None" && e.Entite == EntiteEnum.Intervention).Single();
            else if (DgSource.All(d => d.Etat.EtatEtat))
                return MakfiData.Etats.Where(e => e.Libelle == "Terminée" && e.Entite == EntiteEnum.Intervention).Single();
            else
                return MakfiData.Etats.Where(e => e.Libelle == "Non terminée" && e.Entite == EntiteEnum.Intervention).Single();
        }
        #endregion

    }
}
