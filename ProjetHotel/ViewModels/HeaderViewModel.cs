using Makrisoft.Makfi.Dal;
using Makrisoft.Makfi.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using System.Windows.Markup;

namespace Makrisoft.Makfi.ViewModels
{
    public class HeaderViewModel : ViewModelBase
    {
        #region Bindings

 
        public MainViewModel Main_ViewModel
        {
            get;
            set;
        }



        public ObservableCollection<Utilisateur_VM> Utilisateurs { get; set; }
        public Utilisateur_VM CurrentUtilisateur
        {
            get
            {
                return
                  currentUtilisateur;
            }
            set
            {
                currentUtilisateur = value;
                Hotels = new ObservableCollection<Hotel_VM>(
                    MakfiData.Hotel_Read($"<hotel><gouvernante>{currentUtilisateur.Id}</gouvernante></hotel>")
                   .Select(x => new Hotel_VM
                   {
                       Id = x.Id,
                       Nom = x.Nom,
                       Image = $"/Makrisoft.Makfi;component/Assets/hotels/{x.Image}",
                       Gouvernante = x.Gouvernante,
                   }));
                CurrentHotel = Hotels.FirstOrDefault();
                OnPropertyChanged("CurrentUtilisateur");
            }
        }
        private Utilisateur_VM currentUtilisateur;


        public ObservableCollection<Hotel_VM> Hotels
        {
            get { return hotels; }
            set
            {
                hotels = value;
                OnPropertyChanged("Hotels");
            }
        }
        private ObservableCollection<Hotel_VM> hotels;


        public Hotel_VM CurrentHotel
        {
            get
            {
                return
                  currentHotel;
            }
            set
            {
                currentHotel = value;
                OnPropertyChanged("CurrentHotel");
            }
        }
        private Hotel_VM currentHotel;
        #endregion

        #region Command
        // ICommand
        public ICommand PersistMessage { get; set; }
        public ICommand DeconnectCommand { get; set; }
        public ICommand BackCommand { get; set; }
        // RelayCommand
        //private void RelayCommand()
        //{
        //    PersistMessage = new RelayCommand(p => OnPersistMessage());
        //    DeconnectCommand = new RelayCommand(p => OnDeconnectCommand(), p => OnCanExecuteDeconnectCommand());
        //    BackCommand = new RelayCommand(p => OnBackCommand());
        //}
        private void OnBackCommand()
        {
            switch (Main_ViewModel.ViewSelected)
            {
                case ViewEnum.Administration:
                    Reference_ViewModel.Main.ViewSelected = Dal.ViewEnum.Home;
                    break;

                case ViewEnum.Utilisateur:
                    Reference_ViewModel.Main.ViewSelected = Dal.ViewEnum.Administration;
                    break;

                case ViewEnum.Hotel:
                    Reference_ViewModel.Main.ViewSelected = Dal.ViewEnum.Administration;
                    break;


            }
 
        }
        private void OnDeconnectCommand()
        {
            Main_ViewModel.ViewSelected = ViewEnum.Login;
            CurrentUtilisateur = Utilisateurs.FirstOrDefault(g => g.Nom == "danielle.lopez");
            CurrentUtilisateur.CanChangeUtilisateur = true;
        }
        #endregion

        #region Constructeur
        public HeaderViewModel()
        {
            Utilisateurs = new ObservableCollection<Utilisateur_VM>(
                MakfiData.Utilisateur_Read()
                .Where(x => x.Statut == 1 || x.Statut == 2)
                .Select(x => new Utilisateur_VM
                {
                    Id = x.Id,
                    Nom = x.Nom,
                    Image = $"/Makrisoft.Makfi;component/Assets/Photos/{x.Image}",
                    Statut=x.Statut
                }));
            CurrentUtilisateur = Utilisateurs.FirstOrDefault(g => g.Nom == "danielle.lopez");

            Main_ViewModel = Reference_ViewModel.Main;
            DeconnectCommand = new RelayCommand(p => OnDeconnectCommand());
            BackCommand = new RelayCommand(p => OnBackCommand());

        }

        #endregion
       

        #region Horloge
        private readonly Timer HeaderTimer = new Timer(10000);
        public string Horloge { get { return horloge; } set { HorlogeLoop(); OnPropertyChanged("Horloge"); } }
        private string horloge = DateTime.Now.ToString("dddd d MMM - HH:mm");

        private void HorlogeLoop()
        {
            // Horloge
            horloge = DateTime.Now.ToString("dddd d MMM - HH:mm");

        }
        #endregion
    }
}
