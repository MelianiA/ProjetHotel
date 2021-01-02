using Makrisoft.Makfi.Dal;
using System;
using System.ComponentModel;

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
        public Guid? Id { get; set; }
        public virtual void Load() { }

        #endregion
    }
}
