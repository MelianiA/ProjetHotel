using Makrisoft.Makfi.ViewModels;
using System.Windows.Controls;

namespace Makrisoft.Makfi.Views
{
    public partial class EmployeView : UserControl
    {
        public EmployeView()
        {
            InitializeComponent();
            Reference_ViewModel.Employe = new EmployeViewModel();
            DataContext = Reference_ViewModel.Employe;
             
        }
    }
}
