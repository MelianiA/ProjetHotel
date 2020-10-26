using Makrisoft.Makfi.ViewModels;
using System.Windows.Controls;

namespace Makrisoft.Makfi.Views
{
    /// <summary>
    /// Logique d'interaction pour HeaderView.xaml
    /// </summary>
    public partial class HeaderView : UserControl
    {
        public HeaderView()
        {
            Reference_ViewModel.Header = new HeaderViewModel();
            DataContext = Reference_ViewModel.Header;
            InitializeComponent();
        }
    }
}
