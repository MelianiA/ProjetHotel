using Makrisoft.Makfi.ViewModels;
using System.Windows.Controls;

namespace Makrisoft.Makfi.Views
{
    /// <summary>
    /// Logique d'interaction pour HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
            Reference_ViewModel.Home = new HomeViewModel();
            DataContext = Reference_ViewModel.Home;
        }
    }
}
