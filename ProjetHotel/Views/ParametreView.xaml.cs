using Makrisoft.Makfi.ViewModels;
using Microsoft.Win32;
using System.IO;
using System.Windows.Controls;

namespace Makrisoft.Makfi.Views
{
    /// <summary>
    /// Logique d'interaction pour EmployeView.xaml
    /// </summary>
    public partial class ParametreView : UserControl
    {
        public ParametreView()
        {
            Reference_ViewModel.Parametre = new ParametreViewModel();
            DataContext = Reference_ViewModel.Parametre;
            InitializeComponent();
        }

        
         
    }
}
