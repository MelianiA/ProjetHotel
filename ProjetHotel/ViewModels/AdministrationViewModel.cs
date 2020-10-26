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
    public class AdministrationViewModel : ViewModelBase
    {
        public ICommand ChangeViewCommand { get; set; }
        private void OnChangeViewCommand(object view)
        {
            Reference_ViewModel.Main.ViewSelected = (ViewEnum)view;
        }

        public AdministrationViewModel()
        {
            ChangeViewCommand = new RelayCommand(p => OnChangeViewCommand(p));
        }

    }
}
