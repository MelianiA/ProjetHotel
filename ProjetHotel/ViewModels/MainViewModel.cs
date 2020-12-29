using Makrisoft.Makfi.Dal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makrisoft.Makfi.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ViewEnum ViewSelected
        {
            get { return viewSelected; }
            set
            {
                var exView = viewSelected;
                viewSelected = value;

                // Partie dynamique
                switch (value)
                {
                    case ViewEnum.InterventionAjouter:
                        Reference_ViewModel.InterventionAjouter.CheckAnnuler = true;
                        Reference_ViewModel.InterventionAjouter.Load_Interventions($@"
                                <interventions>
                                    <hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel>
                                    <delete>{Reference_ViewModel.Intervention.CurrentDgSource.Id}</delete>
                                </interventions>");
                        break;
                    case ViewEnum.Intervention:
                        break;
                    case ViewEnum.InterventionDetail:
                        Reference_ViewModel.InterventionDetail.Title = Reference_ViewModel.Intervention.CurrentDgSource.Libelle;
                        if (exView == ViewEnum.InterventionAjouter)
                        {
                            foreach (var interD in Reference_ViewModel.InterventionAjouter.DgSource)
                                Reference_ViewModel.InterventionDetail.DgSource.Add(interD);
                        }
                        else
                            Reference_ViewModel.InterventionDetail.Load_DgSource();
                        break;
                    case ViewEnum.ChambreGroupe:
                        Reference_ViewModel.ChambreGroupe.Load_AllChambres();
                        Reference_ViewModel.ChambreGroupe.Load_Etages();
                        break;
                    case ViewEnum.Login:
                        Reference_ViewModel.Header.Utilisateur_Load();
                        break;
                    case ViewEnum.Hotel:
                        Reference_ViewModel.Hotel.Load_Receptions();
                        Reference_ViewModel.Hotel.Load_Gouvernantes();
                        break;
                }
                OnPropertyChanged("ViewSelected");
            }
        }
        private ViewEnum viewSelected;

        public MainViewModel()
        {
            viewSelected = ViewEnum.Login;


        }
    }
}
