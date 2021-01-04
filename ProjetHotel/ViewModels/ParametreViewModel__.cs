using Makrisoft.Makfi.Dal;
using Makrisoft.Makfi.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;

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
        #endregion

        #region Commands
        //ICommand


        // RelayCommand


        // Méthodes OnCanExecuteCommand

        #endregion

        #region Load
        public override void Load()
        {
            base.Load();
        }
        public void Load_Parametres()
        {
            var infoList = new ObservableCollection<Info_VM>(
             MakfiData.Crud<Info>(
                 "Info_Read",
                 null,
                e =>
                {
                    e.Id = (Guid)MakfiData.Reader["Id"];
                    e.Cle = MakfiData.Reader["Cle"] as string;
                    e.Valeur = MakfiData.Reader["Valeur"] as string;
                })
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
