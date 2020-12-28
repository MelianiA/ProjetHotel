using System.ComponentModel;

namespace Makrisoft.Makfi.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
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

    }
}
