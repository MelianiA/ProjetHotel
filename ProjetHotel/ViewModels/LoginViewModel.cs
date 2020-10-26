//using Makrisoft.Makfi.Data;
//using Makrisoft.Makfi.Services;
//using Makrisoft.Makfi.Tools;
using Makrisoft.Makfi.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Makrisoft.Makfi.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        #region ICommand
        // Login
        public string Password { get { return password; } set { password = value; OnPropertyChanged("Password"); } }
        private string password = "";
        public ICommand LoginKeyCommand { get; set; }

        #endregion
     
        #region Constructor
        public LoginViewModel( )
        {
             LoginKeyCommand = new RelayCommand(p => OnLoginKeyCommand(p));
        }
        #endregion
        private void OnLoginKeyCommand(object key)
        {
            if (key.ToString() == "B") Password = Password.Length == 0 ? "" : Password.Substring(0, Password.Length - 1);
            else
            {
                Password += key.ToString();
                if (Password.Length == 4)
                {
#if PASSEDROIT
                    Reference_ViewModel.Main.ViewSelected = Dal.ViewEnum.Home;
#endif
                    Reference_ViewModel.Header.CurrentUtilisateur.CanChangeUtilisateur = false;
                    Password = "";
                 }
            }
        }
    }
}
