using Makrisoft.Makfi.ViewModels;
using System.Windows.Controls;
 
namespace Makrisoft.Makfi.Views
{
    /// <summary>
    /// Logique d'interaction pour HotelView.xaml
    /// </summary>
    public partial class HotelView : UserControl
    {
        public HotelView()
        {
            Reference_ViewModel.Hotel = new HotelViewModel();
            DataContext = Reference_ViewModel.Hotel;
            InitializeComponent();

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
