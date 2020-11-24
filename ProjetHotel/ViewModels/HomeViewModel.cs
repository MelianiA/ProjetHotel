using Makrisoft.Makfi.Dal;
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

 
        public bool IsAdmin
        {
            get { return isAdmin; }
            set
            {
                isAdmin = value;
                OnPropertyChanged("IsAdmin");
            }
        }
        private bool isAdmin = true;    
        
        public bool WithHotel
        {
            get { return Reference_ViewModel.Header.CurrentHotel!=null; }
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

        private void OnChangeViewCommand(object view)
        {
            if ((ViewEnum)view == ViewEnum.Intervention)
                Reference_ViewModel.Intervention.Load_Intervention();
            Reference_ViewModel.Main.ViewSelected = (ViewEnum)view;
            
        }
       
        #endregion


    }
}
