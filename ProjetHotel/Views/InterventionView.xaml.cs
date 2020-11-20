using Makrisoft.Makfi.ViewModels;
using System.Windows.Controls;

namespace Makrisoft.Makfi.Views
{
    /// <summary>
    /// Logique d'interaction pour InterventionView.xaml
    /// </summary>
    public partial class InterventionView : UserControl
    {
        public InterventionView()
        {
            InitializeComponent();
            Reference_ViewModel.Intervention = new InterventionViewModel();
            DataContext = Reference_ViewModel.Intervention;
         }
    }
}
