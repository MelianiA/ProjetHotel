using Makrisoft.Makfi.Dal;
using Makrisoft.Makfi.Tools;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;
using System.Windows.Input;

namespace Makrisoft.Makfi.ViewModels
{
    public class HeaderViewModel : ViewModelBase
    {
        #region Propriété
        private readonly Timer HeaderTimer = new Timer(10000);

        #endregion

        #region Bindings
        // Utilisateur
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
                Message = "";
                Guid monID = default;
                if (currentUtilisateur == null) return;
                monID = currentUtilisateur.Id;

                if (currentUtilisateur.IsAdmin)
                {
                    Hotels = new ObservableCollection<Hotel_VM>(
                       MakfiData.Hotel_Read()
                      .Select(x => new Hotel_VM
                      {
                          Id = x.Id,
                          Nom = x.Nom,
                          Image = $"/Makrisoft.Makfi;component/Assets/hotels/{x.Nom.ToLower()}.png",
                          Gouvernante = Utilisateurs.Where(u => u.Id == x.Gouvernante).SingleOrDefault()
                      }));
                }
                else
                {
                    // Hotel
                    Hotels = new ObservableCollection<Hotel_VM>(
                        MakfiData.Hotel_Read($"<hotel><gouvernante>{monID}</gouvernante></hotel>")
                       .Select(x => new Hotel_VM
                       {
                           Id = x.Id,
                           Nom = x.Nom,
                           Image = $"/Makrisoft.Makfi;component/Assets/hotels/{x.Nom.ToLower()}.png",
                           Gouvernante = Utilisateurs.Where(u => u.Id == x.Gouvernante).SingleOrDefault()
                       }));
                }
                CurrentHotel = Hotels.FirstOrDefault();
                OnPropertyChanged("CurrentUtilisateur");
            }
        }
        private Utilisateur_VM currentUtilisateur;
        public bool CanChangeUtilisateur
        {
            get { return canChangeUtilisateur; }
            set
            {
                canChangeUtilisateur = value;
                OnPropertyChanged("CanChangeUtilisateur");
            }
        }
        private bool canChangeUtilisateur = true;

        // Hotel
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


        //PremiereConnexion
        public string Message
        {
            get { return premiereConnexion; }
            set
            {
                premiereConnexion = value;
                OnPropertyChanged("Message");
            }
        }
        private string premiereConnexion;


        // Horloge
        public string Horloge
        {
            get { return horloge; }
            set
            {
                horloge = value;
                OnPropertyChanged("Horloge");
            }
        }
        private string horloge = DateTime.Now.ToString("dddd d MMM - HH:mm");

        #endregion

        #region Command
        // ICommand
        public ICommand PersistMessageCommand { get; set; }
        public ICommand DeconnectCommand { get; set; }
        public ICommand BackCommand { get; set; }

        // Méthode
        private void OnBackCommand()
        {
            switch (Reference_ViewModel.Main.ViewSelected)
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

                case ViewEnum.Employe:
                    Reference_ViewModel.Main.ViewSelected = Dal.ViewEnum.Home;
                    break;
                case ViewEnum.Chambre:
                    Reference_ViewModel.Main.ViewSelected = Dal.ViewEnum.Home;
                    break;
                case ViewEnum.ChambreGroupe:
                    Reference_ViewModel.Main.ViewSelected = Dal.ViewEnum.Chambre;
                    break;
                case ViewEnum.Intervention:
                    Reference_ViewModel.Main.ViewSelected = Dal.ViewEnum.Home;
                    break;
                case ViewEnum.InterventionDetail:
                    if (Reference_ViewModel.Intervention.RevienIci == true)
                        Reference_ViewModel.Main.ViewSelected = Dal.ViewEnum.Intervention;
                    else
                        Reference_ViewModel.Main.ViewSelected = Dal.ViewEnum.Home;
                    break;
                case ViewEnum.InterventionAjouter:
                    Reference_ViewModel.Main.ViewSelected = Dal.ViewEnum.InterventionDetail;
                    break;
                case ViewEnum.InterventionSupprimer:
                    Reference_ViewModel.Main.ViewSelected = Dal.ViewEnum.InterventionDetail;
                    break;
            }
        }
        private void OnDeconnectCommand()
        {
            Reference_ViewModel.Main.ViewSelected = ViewEnum.Login;
            CurrentUtilisateur = Utilisateurs.FirstOrDefault(g => g.Nom == Properties.Settings.Default.Login);
            CanChangeUtilisateur = true;
        }
        #endregion

        #region Constructeur
        public HeaderViewModel()
        {
            // ICommand
            DeconnectCommand = new RelayCommand(p => OnDeconnectCommand());
            BackCommand = new RelayCommand(p => OnBackCommand(), p => OnCanExecuteBackCommand());

            // Utilisateur
            Utilisateur_Load();

            // Horloge
            HeaderTimer.Elapsed += (s, e) => HorlogeLoop();
            HeaderTimer.Start();
        }

        private bool OnCanExecuteBackCommand()
        {
            if (Reference_ViewModel.Main.ViewSelected == ViewEnum.Home ||
                Reference_ViewModel.Main.ViewSelected == ViewEnum.Login)
                return false;
            else return true;
        }
        #endregion

        #region Divers

        private void Utilisateur_Load()
        {
            Utilisateurs = new ObservableCollection<Utilisateur_VM>(
               MakfiData.Utilisateur_Read()
               .Where(x => x.Statut == RoleEnum.Admin || x.Statut == RoleEnum.Gouvernante)
               .Select(x => new Utilisateur_VM
               {
                   Id = x.Id,
                   Nom = x.Nom,
                   CodePin = x.CodePin,
                   Image = $"/Makrisoft.Makfi;component/Assets/Photos/{x.Nom.ToLower()}.png",
                   Statut = x.Statut,
                   SaveColor = "Navy"
               }));
            if (Utilisateurs.Count == 1) CurrentUtilisateur = Utilisateurs[0];
            if (CurrentUtilisateur == null)
                CurrentUtilisateur = Utilisateurs.FirstOrDefault(g => g.Nom.ToUpper() == Properties.Settings.Default.Login.ToUpper());

            if (string.IsNullOrEmpty(currentUtilisateur.CodePin))
                Message = "Tapez votre code pin";
        }

        private void HorlogeLoop()
        {
            // Horloge
            Horloge = DateTime.Now.ToString("dddd d MMM - HH:mm");
        }
        #endregion
    }
}
