using Makrisoft.Makfi.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makrisoft.Makfi.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ViewEnum viewSelected;

        public ViewEnum ViewSelected
        {
            get { return viewSelected; }
            set
            {
                viewSelected = value;
                OnPropertyChanged("ViewSelected");
            }
        }

        public MainViewModel()
        {
            ViewSelected = ViewEnum.Login;
        }
    }
}
