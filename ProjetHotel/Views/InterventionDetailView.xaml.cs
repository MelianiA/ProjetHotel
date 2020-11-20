using Makrisoft.Makfi.ViewModels;
using System.Windows.Controls;

namespace Makrisoft.Makfi.Views
{
    /// <summary>
    /// Logique d'interaction pour InterventionDetailView.xaml
    /// </summary>
    public partial class InterventionDetailView : UserControl
    {
        public InterventionDetailView()
        {
            Reference_ViewModel.InterventionDetail = new InterventionDetailViewModel();
            DataContext = Reference_ViewModel.InterventionDetail;
            InitializeComponent();
        }
    }
}
