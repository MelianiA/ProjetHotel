using Makrisoft.Makfi.Dal;
using Makrisoft.Makfi.Tools;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Makrisoft.Makfi.ViewModels
{
    public class ParametreViewModel : ViewModelBase
    {
        #region Constructeur
        public ParametreViewModel()
        {
            Load_Parametres();

        }

        #endregion

        #region Bindings
        public bool VoirMsgArchives
        {
            get { return voirMsgArchives; }
            set
            {
                voirMsgArchives = value;
                OnPropertyChanged("VoirMsgArchives");
            }
        }
        private bool voirMsgArchives;

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

        #endregion

        #region Commands
        //ICommand


        // RelayCommand


        // Méthodes OnCanExecuteCommand

        #endregion

        #region Load

        public void Load_Parametres()
        {
            var infoList = new ObservableCollection<Info_VM>(
             MakfiData.Info_Read()
             .Select(x => new Info_VM
             {
                 Id = x.Id,
                 Cle = x.Cle,
                 Valeur = x.Valeur
             }).ToList());
            VoirMsgArchives = bool.Parse(infoList.Where(i => i.Cle == "VoirMsgArchives").Select(i => i.Valeur).Single());
        }
        #endregion
    }
}
