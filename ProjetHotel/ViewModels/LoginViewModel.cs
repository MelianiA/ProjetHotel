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
        public HeaderViewModel Header_ViewModel;

        #region ICommand
        // Login
        public string Login { get { return login; } set { login = value; OnPropertyChanged("Login"); } }
        private string login = "";
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
            if (key.ToString() == "B") Login = Login.Length == 0 ? "" : Login.Substring(0, Login.Length - 1);
            else
            {
                Login += key.ToString();
                if (Login.Length == 4)
                {
                    

                    Login = "";
                }
            }
        }
    }
}
