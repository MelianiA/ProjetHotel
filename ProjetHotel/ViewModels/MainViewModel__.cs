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
                LastView = viewSelected;
                viewSelected = value;

                switch (value)
                {
                    case ViewEnum.Chambre: Reference_ViewModel.Chambre.Load(); break;
                    case ViewEnum.Employe: Reference_ViewModel.Employe.Load(); break;
                    case ViewEnum.Etage: Reference_ViewModel.Etage.Load(); break;
                    case ViewEnum.Intervention: Reference_ViewModel.Intervention.Load(); break;
                    case ViewEnum.InterventionAjouter: Reference_ViewModel.InterventionAjouter.Load(); break;
                    case ViewEnum.InterventionDetail: Reference_ViewModel.InterventionDetail.Load(); break;
                    case ViewEnum.Home: Reference_ViewModel.Home.Load(); break;
                    case ViewEnum.Hotel: Reference_ViewModel.Hotel.Load(); break;
                    case ViewEnum.Login: Reference_ViewModel.Login.Load(); break;
                    case ViewEnum.Message: Reference_ViewModel.Message.Load(); break;
                    case ViewEnum.Utilisateur: Reference_ViewModel.Utilisateur.Load(); break;
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
