using Makrisoft.Makfi.Dal;
using Makrisoft.Makfi.Tools;
using System.Windows.Input;

namespace Makrisoft.Makfi.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        #region Propriété
        #endregion

        #region Binding
        public string Password { get { return password; } set { password = value; OnPropertyChanged("Password"); } }
        private string password = "";
        #endregion

        #region ICommand
        // ICommand
        public ICommand LoginKeyCommand { get; set; }

        // Méthode
        private void OnLoginKeyCommand(object key)
        {
            Password += key.ToString();
            if (Password.Length == 4)
            {
#if PASSEDROIT
                Password = "1234";
#endif
                if (Password == Reference_ViewModel.Header.CurrentUtilisateur.CodePin)
                {
                    Reference_ViewModel.Main.ViewSelected = ViewEnum.Home;
                    Reference_ViewModel.Header.CanChangeUtilisateur = false;
                    Reference_ViewModel.Home.IsAdmin = Reference_ViewModel.Header.CurrentUtilisateur.IsAdmin;
                }
                Password = "";
            }
        }

        #endregion

        #region Constructeur
        public LoginViewModel()
        {
            // ICommand
            LoginKeyCommand = new RelayCommand(p => OnLoginKeyCommand(p));
        }
        #endregion
    }
}
