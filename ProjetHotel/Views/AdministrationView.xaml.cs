using Makrisoft.Makfi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Makrisoft.Makfi.Views
{
    /// <summary>
    /// Logique d'interaction pour AdministrationView.xaml
    /// </summary>
    public partial class AdministrationView : UserControl
    {
        public AdministrationView()
        {
            Reference_ViewModel.Administration = new AdministrationViewModel();
            DataContext = Reference_ViewModel.Administration;
            InitializeComponent();
        }
    }
}
