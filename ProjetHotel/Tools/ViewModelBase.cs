using Makrisoft.Makfi.Dal;
using System;
using System.ComponentModel;
using System.Windows;

namespace Makrisoft.Makfi.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        #region OnPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        #endregion

        #region ViewModel
        public ViewEnum LastView;
        public string SaveColor
        {
            get
            { return saveColor; }
            set
            {
                saveColor = value;
                OnPropertyChanged("SaveColor");
            }
        }
        private string saveColor = "Navy";

        public Guid? Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }
        private Guid? id;


        public virtual void Load()
        {
            Reference_ViewModel.Header.MessageClear();
        }

        #endregion
    }
}
