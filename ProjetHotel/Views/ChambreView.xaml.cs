using Makrisoft.Makfi.ViewModels;
using System.Windows.Controls;

namespace Makrisoft.Makfi.Views
{
    public partial class ChambreView : UserControl
    {
        public ChambreView()
        {
            Reference_ViewModel.Chambre = new ChambreViewModel();
            DataContext = Reference_ViewModel.Chambre;
            InitializeComponent();
        }
    }
}
