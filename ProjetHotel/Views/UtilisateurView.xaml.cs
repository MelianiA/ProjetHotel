using Makrisoft.Makfi.ViewModels;
using System.Windows.Controls;

namespace Makrisoft.Makfi.Views
{
    /// <summary>
    /// Logique d'interaction pour EmployeView.xaml
    /// </summary>
    public partial class UtilisateurView : UserControl
    {
        public UtilisateurView()
        {
            Reference_ViewModel.Utilisateur = new UtilisateurViewModel();
            DataContext = Reference_ViewModel.Utilisateur;
            InitializeComponent();
        }
    }
}
