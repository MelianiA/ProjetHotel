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
            Components = ComponentEnum.Etats | ComponentEnum.Employes | ComponentEnum.Chambres | ComponentEnum.Etages;

            Init();

            AddAllCommand = new RelayCommand(p => OnAddAllCommand(), p => true);
            SaveAllCommand = new RelayCommand(p => OnSaveAllCommand(), p => OnCanExecuteSaveAllCommand());
            DeleteAllCommand = new RelayCommand(p => OnDeleteAllCommand(), p => OnCanExecuteDeleteAllCommand());

        }

        private void OnSaveAllCommand()
        {
            var param =
                $"<interventionDetails><intervention>{Reference_ViewModel.Intervention.CurrentDgSource.Id}</intervention>" +
                string.Join("",
                    DgSource.Where(x => x.Id == null)
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
            var ids = MakfiData.InterventionDetail_Save(param);
            if (ids.Count == 0) throw new Exception("Rien n'a été sauvgardé ! ");

            int i = 0;
            foreach (var dg in DgSource)
            {
                if (dg.Id == null)
                {
                    dg.Id = ids[i].Id;
                    i++;
                }
                else if (dg.SaveColor == "Red")
                {
                    MakfiData.InterventionDetail_Save($@"
                        <interventionDetail>
                            <id>{CurrentDgSource.Id}</id>
                            <employeAffecte>{CurrentDgSource.Employe.Id}</employeAffecte>
                            <commentaire>{CurrentDgSource.Commentaire}</commentaire>    
                            <chambreAffectee>{CurrentDgSource.Chambre.Id}</chambreAffectee>
                            <intervention>{Reference_ViewModel.Intervention.CurrentDgSource.Id}</intervention>    
                            <etat>{CurrentDgSource.Etat.Id}</etat> 
                           </interventionDetail>");
                }
                dg.SaveColor = "Navy";
            }
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
            var param = $@"<interventionDetail>
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

        private void OnAddAllCommand()
        {
            Reference_ViewModel.Main.ViewSelected = ViewEnum.InterventionAjouter;
        }
        public override void OnAddCommand()
        {
            CurrentDgSource = new InterventionDetail_VM
            {
                Id = null,
                Etat = MakfiData.Etats.Where(e => e.Libelle == "None" && e.Entite == EtatType).SingleOrDefault(),
                Employe = Employes[0],
                Chambre = Chambres[0]
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

        }

        public bool OnCanExecuteSaveAllCommand()
        {
            return DgSource.Any(i => i.SaveColor == "Red");
        }

        private bool OnCanExecuteDeleteAllCommand()
        {
            return DgSource.Count > 0;
        }

        private void OnDeleteAllCommand()
        {
            DgSource.Clear();
            MakfiData.InterventionDetails_Delete($"<interventionDetails><intervention>{Reference_ViewModel.Intervention.CurrentDgSource.Id}</intervention></interventionDetails>");
        }
        #endregion

        #region Divers
        public Etat_VM GetSommeEtats()
        {
            if (DgSource.Count == 0 || DgSource.All(i => i.Etat.Libelle == "None"))
                return MakfiData.Etats.Where(e => e.Libelle == "None" && e.Entite == EtatType).Single();
            if (DgSource.All(i => i.Etat.EtatEtat))
                return MakfiData.Etats.Where(e => e.Libelle == "Terminée" && e.Entite == EtatType).Single();
            else
                return MakfiData.Etats.Where(e => e.Libelle == "Non terminée" && e.Entite == EtatType).Single();
        }
        #endregion
    }
}
