using Makrisoft.Makfi.ViewModels;
using System.Windows.Controls;

namespace Makrisoft.Makfi.Views
{
   
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
            Reference_ViewModel.Login = new LoginViewModel();
            DataContext = Reference_ViewModel.Login;
        }
    }
}
