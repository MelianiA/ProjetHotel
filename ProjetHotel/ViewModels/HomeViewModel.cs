using Makrisoft.Makfi.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Makrisoft.Makfi.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {

        #region Binding

        private bool isAdmin = true;

        public bool IsAdmin
        {
            get { return isAdmin; }
            set
            {
                isAdmin = value;
                OnPropertyChanged("IsAdmin");
            }
        }
        #endregion


        #region ICommand
        public ICommand ChangeViewCommand { get; set; }


        #endregion


        #region Constructeur
        public HomeViewModel()
        {
            ChangeViewCommand = new RelayCommand(p => OnChangeViewCommand(p));
           


        }

        private void OnChangeViewCommand(object p)
        {
            Reference_ViewModel.Main.ViewSelected = Dal.ViewEnum.Administration;

        }
        #endregion


    }
}
