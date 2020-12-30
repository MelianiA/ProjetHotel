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
    /// Logique d'interaction pour ChambreView.xaml
    /// </summary>
    public partial class EtageView : UserControl
    {
        public EtageView()
        {
            Reference_ViewModel.ChambreGroupe = new EtageViewModel();
            DataContext = Reference_ViewModel.ChambreGroupe;
            InitializeComponent();
        }

        private void TextBox_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }
    }
}
