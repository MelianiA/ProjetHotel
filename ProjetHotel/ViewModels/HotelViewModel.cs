using Makrisoft.Makfi.Dal;
using Makrisoft.Makfi.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Makrisoft.Makfi.ViewModels
{
    
    public class HotelViewModel : ViewModelBase   
    {
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

        // Méthodes OnCommand




        // Méthodes OnCanExecuteXXXXCommand



        //Autres Méthodes
        private void Hotel_Load()
        {
            Hotels = new ObservableCollection<Hotel_VM>(MakfiData.Hotel_Read()
             .Select(x => new Hotel_VM
             {
                 Id = x.Id,
                 Nom = x.Nom,
                 Image = $"/Makrisoft.Makfi;component/Assets/Photos/{x.Image}",

                 Gouvernante = Reference_ViewModel.utilisateur.Utilisateurs
                                .Where(u => u.Id == x.Gouvernante).SingleOrDefault(), 

                 Reception= Reference_ViewModel.utilisateur.Utilisateurs
                                .Where(u => u.Id == x.Reception).SingleOrDefault(),

                 Commentaire=x.Commentaire,
                 SaveColor = "Navy"
             }).ToList()) ;
            // ListeView
            HotelCollectionView = new ListCollectionView(Hotels);
            HotelCollectionView.Refresh();
        }
        private void GouvrnanteListLoad()
        {
            GouvernanteList = new ObservableCollection<Utilisateur_VM>( Reference_ViewModel.utilisateur.Utilisateurs
                                                            .Where(u=> u.Statut==RoleEnum.Gouvernante));;
        }
        private void ReceptionListLoad()
        {
            ReceptionList = new ObservableCollection<Utilisateur_VM>(Reference_ViewModel.utilisateur.Utilisateurs
                                                            .Where(u => u.Statut == RoleEnum.Reception));
        }
        #endregion

        #region Constructeur
        public HotelViewModel()
        {
            // Icommand


            // ListeView
            Hotel_Load();
            GouvrnanteListLoad();
            ReceptionListLoad();

            //Filter

        }



        #endregion


    }
}
