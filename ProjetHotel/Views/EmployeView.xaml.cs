using Makrisoft.Makfi.ViewModels;
using System.Windows.Controls;

namespace Makrisoft.Makfi.Views
{
    public partial class EmployeView : UserControl
    {
        public EmployeView()
        {
            Reference_ViewModel.Employe = new EmployeViewModel();
            DataContext = Reference_ViewModel.Employe;
            InitializeComponent();
        }
    }
}
