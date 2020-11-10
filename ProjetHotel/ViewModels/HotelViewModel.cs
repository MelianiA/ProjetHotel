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
    public class HotelViewModel : ViewModelBase
    {
        #region Constructeur
        public HotelViewModel()
        {
            // Icommand
            HotelModifiedSaveCommand = new RelayCommand(p => OnSaveCommand(), p => OnCanExecuteSaveCommand());
            HotelSelectedAddCommand = new RelayCommand(p => OnAddCommand(), p => OnCanExecuteAddCommand());
            HotelSelectedDeleteCommand = new RelayCommand(p => OnDeleteCommand(), p => OnCanExecuteDeleteCommand());

            // ListeView
            Hotel_Load();
            GouvernanteListLoad();
            ReceptionListLoad();

        }
        #endregion

        #region Binding
        //Hotel
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

                return currentHotel;

            }
            set
            {
                currentHotel = value;
                OnPropertyChanged("CurrentHotel");
            }
        }
        private Hotel_VM currentHotel;
        public ListCollectionView HotelCollectionView
        {
            get { return hotelCollectionView; }
            set { hotelCollectionView = value; OnPropertyChanged("HotelCollectionView"); }
        }
        private ListCollectionView hotelCollectionView;

        //Gouvernante
        public ObservableCollection<Utilisateur_VM> GouvernanteList
        {
            get { return gouvernanteList; }
            set
            {
                gouvernanteList = value;
                OnPropertyChanged("GouvernanteList");

            }
        }
        private ObservableCollection<Utilisateur_VM> gouvernanteList;


        //Reception
        public ObservableCollection<Utilisateur_VM> ReceptionList
        {
            get { return receptionList; }
            set
            {
                receptionList = value;
                OnPropertyChanged("ReceptionList");

            }
        }
        private ObservableCollection<Utilisateur_VM> receptionList;

        #endregion

        #region Commands
        //ICommand
        public ICommand HotelModifiedSaveCommand { get; set; }
        public ICommand HotelSelectedAddCommand { get; set; }
        public ICommand HotelSelectedDeleteCommand { get; set; }

        // Méthodes OnCommand
        private void OnSaveCommand()
        {
            if (CurrentHotel.Reception == null || CurrentHotel.Gouvernante == null)
            {
                MessageBox.Show($"Ajout impossible de l'hotel: {CurrentHotel.Nom}", "Important !");
                return;
            }
            Guid? monID = null;
            if (CurrentHotel.Id != default) monID = CurrentHotel.Id;
            var param = $@"
                    <hotel>
                        <id>{monID}</id>
                        <nom>{CurrentHotel.Nom}</nom>
                        <reception>{CurrentHotel.Reception.Id}</reception>
                        <gouvernante>{CurrentHotel.Gouvernante.Id}</gouvernante>
                        <commentaire>{CurrentHotel.Commentaire}</commentaire>       
                    </hotel>";
            var ids = MakfiData.Hotel_Save(param);
            if (ids.Count == 0) throw new Exception("Rien n'a été sauvgardé ! ");
            CurrentHotel.SaveColor = "Navy";
            Hotels.Clear();
            Hotel_Load();
            CurrentHotel = Hotels.Where(u => u.Id == ids[0].Id).SingleOrDefault();
        }
        private void OnAddCommand()
        {
            CurrentHotel = new Hotel_VM { Nom = "(A définir)" };
            Hotels.Add(CurrentHotel);
            HotelCollectionView.Refresh();
        }
        private void OnDeleteCommand()
        {
            var canDeletes = MakfiData.Hotel_CanDelete($"<hotel><id>{CurrentHotel.Id}</id></hotel>");
            if (canDeletes.Count() == 0)
            {
                var param = MakfiData.Hotel_Delete($"<hotel><id>{CurrentHotel.Id}</id></hotel>");
                if (param)
                {
                    Hotels.Remove(CurrentHotel);
                }
            }
            else
            {
                MessageBox.Show($" Suppression impossible de l'Hotel : {CurrentHotel.Nom }", "Utilisateur_CanDelete");
            }
            ///


        }

        // Méthodes OnCanExecuteCommand
        private bool OnCanExecuteSaveCommand()
        {
            if (CurrentHotel != null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        private bool OnCanExecuteAddCommand()
        {
            return true;
        }
        private bool OnCanExecuteDeleteCommand()
        {
            if (CurrentHotel != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Load
        private void Hotel_Load()
        {
            Hotels = new ObservableCollection<Hotel_VM>(
                MakfiData.Hotel_Read()
                .Select(x => new Hotel_VM
                {
                    Id = x.Id,
                    Nom = x.Nom,
                    Image = x.Image != null ? $"/Makrisoft.Makfi;component/Assets/hotels/{x.Image}" : $"/Makrisoft.Makfi;component/Assets/hotels/hotel.png",

                    Gouvernante = Reference_ViewModel.Utilisateur.Utilisateurs
                                .Where(u => u.Id == x.Gouvernante).SingleOrDefault(),

                    Reception = Reference_ViewModel.Utilisateur.Utilisateurs
                                .Where(u => u.Id == x.Reception).SingleOrDefault(),

                    Commentaire = x.Commentaire,
                    SaveColor = "Navy"
                }).OrderBy(x => x.Nom).ToList());
            HotelCollectionView = new ListCollectionView(Hotels);
            HotelCollectionView.Refresh();

        }
        public void GouvernanteListLoad()
        {
            GouvernanteList = new ObservableCollection<Utilisateur_VM>(
                Reference_ViewModel.Utilisateur.Utilisateurs
                .Where(u => u.Statut == RoleEnum.Gouvernante));
        }
        public void ReceptionListLoad()
        {
            ReceptionList = new ObservableCollection<Utilisateur_VM>(
                Reference_ViewModel.Utilisateur.Utilisateurs
                .Where(u => u.Statut == RoleEnum.Reception));
        }
        #endregion
    }
}
