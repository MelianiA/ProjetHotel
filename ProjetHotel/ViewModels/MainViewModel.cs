using Makrisoft.Makfi.Dal;

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

                switch (value)
                {
                    case ViewEnum.InterventionAjouter:
                        Reference_ViewModel.InterventionAjouter.Load(exView);
                        break;
                    case ViewEnum.Intervention:
                        Reference_ViewModel.Intervention.Load(exView);
                        break;
                    case ViewEnum.InterventionDetail:
                        Reference_ViewModel.InterventionDetail.Load(exView);
                        break;
                    case ViewEnum.Etage:
                        Reference_ViewModel.ChambreGroupe.Load(exView);
                        break;
                    case ViewEnum.Login:
                        Reference_ViewModel.Login.Load(exView);
                        break;
                    case ViewEnum.Hotel:
                        Reference_ViewModel.Hotel.Load(exView);
                        break;
                    case ViewEnum.Home:
                        Reference_ViewModel.Home.Load(exView);
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
