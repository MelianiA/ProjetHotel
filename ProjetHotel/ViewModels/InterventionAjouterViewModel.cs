using Makrisoft.Makfi.Dal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Makrisoft.Makfi.ViewModels
{
    public class InterventionAjouterViewModel : ViewModel<InterventionDetail_VM>
    {
        #region Constructeur
        public InterventionAjouterViewModel()
        {
            EtatType = EntiteEnum.InterventionAjouter;
            Loads = LoadEnum.Chambres | LoadEnum.Employes | LoadEnum.Interventions | LoadEnum.Etages;
            Title = "Tout ajouter !";
            Init();
        }

        public override void Load(ViewEnum exView)
        {
            CheckAnnuler = true;
            base.Load(exView);
        }
        #endregion

        #region DgSource
        public override IEnumerable<InterventionDetail_VM> DgSource_Read()
        {
            if (CurrentIntervention != null)
            {
                return MakfiData
                    .InterventionDetail_Read($@"
                        <interventionDetails>
                            <intervention>{CurrentIntervention.Id}</intervention>
                        </interventionDetails>")
                    .Select(x => new InterventionDetail_VM
                    {
                        Id = null,
                        Employe = Reference_ViewModel.InterventionDetail.Employes.Where(c => c.Id == x.Employe.Id).Single(),
                        Chambre = Reference_ViewModel.InterventionDetail.Chambres.Where(c => c.Id == x.Chambre.Id).FirstOrDefault(),
                        Libelle = Reference_ViewModel.Intervention.CurrentDgSource.Libelle,
                        Commentaire = x.Commentaire,
                        Etat = MakfiData.Etats.Where(e => e.Id == x.Etat).Single(),
                        SaveColor = "Red"
                    });
            }
            else if (CurrentEtage != null && CurrentEmploye != null)
            {
                var interDetails = new List<InterventionDetail_VM>();
                var chambres = MakfiData.Chambre_Read($"<chambres><groupeChambre>{CurrentEtage.Id}</groupeChambre></chambres>");
                foreach (var chambre in chambres)
                {
                    interDetails.Add(new InterventionDetail_VM
                    {
                        Id = null,
                        Chambre = Reference_ViewModel.InterventionDetail.Chambres.Where(c => c.Id == chambre.Id).FirstOrDefault(),
                        Employe = Reference_ViewModel.InterventionDetail.Employes.Where(c => c.Id == CurrentEmploye.Id).Single(),
                        Etat = MakfiData.Etats.Where(e => e.Entite == EntiteEnum.InterventionDetail && e.Libelle == "None").Single(),
                        SaveColor = "Red"
                    });
                }
                return interDetails;
            }
            return new List<InterventionDetail_VM>();
        }
        #endregion

        public bool CheckAnnuler
        {
            get { return checkAnnuler; }
            set
            {
                checkAnnuler = value;
                CurrentEmploye = null;
                CurrentEtage = null;
                CurrentIntervention = null;
                OnPropertyChanged("CheckAnnuler");
            }
        }
        private bool checkAnnuler = true;
        public Intervention_VM CurrentIntervention
        {
            get { return currentIntervention; }
            set
            {
                currentIntervention = value;

                Load_DgSource();
                OnPropertyChanged("CurrentIntervention");
            }
        }
        private Intervention_VM currentIntervention;

        public Etage_VM CurrentEtage
        {
            get { return currentEtage; }
            set
            {
                currentEtage = value;
                Load_DgSource();

                OnPropertyChanged("CurrentEtage");
            }
        }
        private Etage_VM currentEtage;
        public Employe_VM CurrentEmploye
        {
            get { return currentEmploye; }
            set
            {
                currentEmploye = value;
                Load_DgSource();
                OnPropertyChanged("CurrentEmploye");
            }
        }
        private Employe_VM currentEmploye;

        public bool CheckInterventionModel
        {
            get { return checkInterventionModel; }
            set
            {
                checkInterventionModel = value;
                CurrentEmploye = null;
                CurrentEtage = null;
                OnPropertyChanged("CheckInterventionModel");
            }
        }
        private bool checkInterventionModel = false;


        public bool CheckUnEtageUnEmploye
        {
            get { return checkUnEtageUnEmploye; }
            set
            {
                checkUnEtageUnEmploye = value;
                CurrentIntervention = null;
                OnPropertyChanged("CheckUnEtageUnEmploye");
            }
        }
        private bool checkUnEtageUnEmploye = false;
    }
}
