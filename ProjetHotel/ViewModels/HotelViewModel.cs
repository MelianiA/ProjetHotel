using Makrisoft.Makfi.Dal;
using Makrisoft.Makfi.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Makrisoft.Makfi.ViewModels
{
    public class HotelViewModel : ViewModel<Hotel_VM>
    {

        #region Constructeur
        public HotelViewModel()
        {
            EtatType = EntiteEnum.None;
            SortDescriptions = new SortDescription[1] { new SortDescription("Nom", ListSortDirection.Ascending) };
            Components = ComponentEnum.None;
            Title = "Les hôtels";

             Load_Receptions();
             Load_Gouvernantes();
            Init();
        }
        #endregion

        #region DgSource
        public override IEnumerable<Hotel_VM> DgSource_Read()
        {
            return new ObservableCollection<Hotel_VM>(
                MakfiData.Hotel_Read()
                .Select(x => new Hotel_VM
                {
                    Id = x.Id,
                    Nom = x.Nom,
                    Image = $"/Makrisoft.Makfi;component/Assets/hotels/{x.Nom.ToLower()}.png",
                    Gouvernante = Gouvernantes.Where(u => u.Id == x.Gouvernante).SingleOrDefault(),
                    Reception = Receptions.Where(u => u.Id == x.Reception).SingleOrDefault(),
                    Commentaire = x.Commentaire,
                    SaveColor = "Navy"
                }));
        }
        public override void DgSource_Save()
        {
            var reception = CurrentDgSource.Reception == null ? null : CurrentDgSource.Reception.Id;
            var gouv = CurrentDgSource.Gouvernante == null ? null : CurrentDgSource.Gouvernante.Id;
            var param = $@"<hotels>
                                <id>{CurrentDgSource.Id}</id>
                                <nom>{CurrentDgSource.Nom}</nom> 
                                <reception>{reception}</reception>
                                <gouvernante>{gouv}</gouvernante>
                                <commentaire>{CurrentDgSource.Commentaire}</commentaire>       
                            </hotels>";
            var ids = MakfiData.Hotel_Save(param);
            if (ids.Count == 0) throw new Exception("Rien n'a été sauvegardé ! ");
            CurrentDgSource.Id = ids[0].Id;
            CurrentDgSource.SaveColor = "Navy";
        }

        #endregion

        #region Command
        public override void OnAddCommand()
        {
            CurrentDgSource = new Hotel_VM { Id = null, Nom = "(A définir)" };
            DgSource.Add(CurrentDgSource);
        }
        public override void OnDeleteCommand()
        {
            var canDeletes = MakfiData.Hotel_CanDelete($"<hotels><id>{CurrentDgSource.Id}</id></hotels>");
            if (canDeletes.Count() == 0)
            {
                var param = MakfiData.Hotel_Delete($"<hotels><id>{CurrentDgSource.Id}</id></hotels>");
                if (param)
                {
                    DgSource.Remove(CurrentDgSource);
                }
            }
            else
            {
                MessageBox.Show($" Suppression impossible de l'hôtel : {CurrentDgSource.Nom }", "Hotel_CanDelete");
            }
        }
        #endregion

        // Gouvernantes
        public ObservableCollection<Utilisateur_VM> Gouvernantes
        {
            get { return gouvernantes; }
            set
            {
                gouvernantes = value;
                OnPropertyChanged("Gouvernantes");
            }
        }
        private ObservableCollection<Utilisateur_VM> gouvernantes;

        // Reception
        public ObservableCollection<Utilisateur_VM> Receptions
        {
            get { return receptions; }
            set
            {
                receptions = value;
                OnPropertyChanged("Receptions");
            }
        }
        private ObservableCollection<Utilisateur_VM> receptions;

        public void Load_Gouvernantes()
        {
            var items = new ObservableCollection<Utilisateur_VM>(MakfiData.Utilisateur_Read()
                                    .Where(u => u.Statut == RoleEnum.Gouvernante)
                                    .Select(x => new Utilisateur_VM
                                    {
                                        Id = x.Id,
                                        Nom = x.Nom,
                                        Image = $"/Makrisoft.Makfi;component/Assets/Photos/{x.Nom.ToLower()}.png",
                                        CodePin = x.CodePin,
                                        Statut = x.Statut,
                                        DateModified = default,
                                        SaveColor = "Navy"
                                    }) );
            if (Gouvernantes == null)
                Gouvernantes = new ObservableCollection<Utilisateur_VM>(items);
            else
            {
                Gouvernantes.Clear();
                foreach (var item in items) Gouvernantes.Add(item);
            }
        }
        public void Load_Receptions()
        {
            var items = new ObservableCollection<Utilisateur_VM>(MakfiData.Utilisateur_Read()
                                         .Where(u => u.Statut == RoleEnum.Reception)
                                         .Select(x => new Utilisateur_VM
                                         {
                                             Id = x.Id,
                                             Nom = x.Nom,
                                             Image = $"/Makrisoft.Makfi;component/Assets/Photos/{x.Nom.ToLower()}.png",
                                             CodePin = x.CodePin,
                                             Statut = x.Statut,
                                             DateModified = default,
                                             SaveColor = "Navy"
                                         }));
            if (Receptions == null)
                Receptions = new ObservableCollection<Utilisateur_VM>(items);
            else
            {
                receptions.Clear();
                foreach (var item in items) receptions.Add(item);
            }
        }

    }
    //public class HotelViewModel : ViewModelBase
    //{
    //    #region Constructeur
    //    public HotelViewModel()
    //    {
    //        // Icommand
    //        HotelSaveCommand = new RelayCommand(p => OnHotelSaveCommand(), p => OnCanExecuteHotelSaveCommand());
    //        HotelAddCommand = new RelayCommand(p => OnHotelAddCommand(), p => OnCanExecuteHotelAddCommand());
    //        HotelDeleteCommand = new RelayCommand(p => OnHotelDeleteCommand(), p => OnCanExecuteHotelDeleteCommand());

    //        // ListeView
    //        Hotel_Load();
    //        Gouvernante_Load();
    //        Reception_Load();
    //    }
    //    #endregion

    //    #region Binding
    //    //Hotel
    //    public ObservableCollection<Hotel_VM> Hotels
    //    {
    //        get { return hotels; }
    //        set
    //        {
    //            hotels = value;
    //            OnPropertyChanged("Hotels");

    //        }
    //    }
    //    private ObservableCollection<Hotel_VM> hotels;
    //    public Hotel_VM CurrentHotel
    //    {
    //        get
    //        {

    //            return currentHotel;

    //        }
    //        set
    //        {
    //            currentHotel = value;
    //            if (currentHotel == null) IsModifierEnabled = false;
    //            else IsModifierEnabled = true;
    //            OnPropertyChanged("CurrentHotel");
    //        }
    //    }
    //    private Hotel_VM currentHotel;
    //    public ListCollectionView HotelCollectionView
    //    {
    //        get { return hotelCollectionView; }
    //        set { hotelCollectionView = value; OnPropertyChanged("HotelCollectionView"); }
    //    }
    //    private ListCollectionView hotelCollectionView;

    //    //IsModifierEnabled
    //    public bool IsModifierEnabled
    //    {
    //        get { return isModifierEnabled; }
    //        set
    //        {
    //            isModifierEnabled = value;
    //            OnPropertyChanged("IsModifierEnabled");
    //        }
    //    }
    //    private bool isModifierEnabled;

    //    //Gouvernante
    //    public ObservableCollection<Utilisateur_VM> Gouvernantes
    //    {
    //        get { return gouvernantes; }
    //        set
    //        {
    //            gouvernantes = value;
    //            OnPropertyChanged("Gouvernantes");

    //        }
    //    }
    //    private ObservableCollection<Utilisateur_VM> gouvernantes;


    //    //Reception
    //    public ObservableCollection<Utilisateur_VM> Receptions
    //    {
    //        get { return receptions; }
    //        set
    //        {
    //            receptions = value;
    //            OnPropertyChanged("Receptions");

    //        }
    //    }
    //    private ObservableCollection<Utilisateur_VM> receptions;

    //    #endregion

    //    #region Commands
    //    //ICommand
    //    public ICommand HotelSaveCommand { get; set; }
    //    public ICommand HotelAddCommand { get; set; }
    //    public ICommand HotelDeleteCommand { get; set; }

    //    // Méthodes OnCommand
    //    private void OnHotelSaveCommand()
    //    {
    //        if (CurrentHotel.Nom == null )
    //        {
    //            MessageBox.Show($"Il faut préciser le nom de l'hôtel", "Important !");
    //            return;
    //        }
    //        Guid? monID = null;
    //        if (CurrentHotel.Id != default) monID = CurrentHotel.Id;
    //        Guid? rec = CurrentHotel.Reception != null? (Guid?)CurrentHotel.Reception.Id: null;
    //        Guid? gouv = CurrentHotel.Gouvernante != null ? (Guid?)CurrentHotel.Gouvernante.Id :null;
    //        var param = $@"
    //                <hotel>
    //                    <id>{monID}</id>
    //                    <nom>{CurrentHotel.Nom}</nom>
    //                    <reception>{rec}</reception>
    //                    <gouvernante>{gouv}</gouvernante>
    //                    <commentaire>{CurrentHotel.Commentaire}</commentaire>       
    //                </hotel>";
    //        var ids = MakfiData.Hotel_Save(param);
    //        if (ids.Count == 0) throw new Exception("Rien n'a été sauvgardé ! ");
    //        CurrentHotel.SaveColor = "Navy";
    //        Hotels.Clear();
    //        Hotel_Load();
    //        CurrentHotel = Hotels.Where(u => u.Id == ids[0].Id).SingleOrDefault();
    //    }
    //    private void OnHotelAddCommand()
    //    {
    //        CurrentHotel = new Hotel_VM { Nom = "(A définir)" };
    //        Hotels.Add(CurrentHotel);
    //        HotelCollectionView.Refresh();
    //    }
    //    private void OnHotelDeleteCommand()
    //    {
    //        var canDeletes = MakfiData.Hotel_CanDelete($"<hotel><id>{CurrentHotel.Id}</id></hotel>");
    //        if (canDeletes.Count() == 0)
    //        {
    //            var param = MakfiData.Hotel_Delete($"<hotel><id>{CurrentHotel.Id}</id></hotel>");
    //            if (param)
    //            {
    //                Hotels.Remove(CurrentHotel);
    //            }
    //        }
    //        else
    //        {
    //            MessageBox.Show($" Suppression impossible de l'Hotel : {CurrentHotel.Nom }", "Utilisateur_CanDelete");
    //        }
    //        ///


    //    }

    //    // Méthodes OnCanExecuteCommand
    //    private bool OnCanExecuteHotelSaveCommand()
    //    {
    //        if (CurrentHotel != null)
    //        {
    //            return true;
    //        }
    //        else
    //        {
    //            return false;
    //        }

    //    }
    //    private bool OnCanExecuteHotelAddCommand()
    //    {
    //        return true;
    //    }
    //    private bool OnCanExecuteHotelDeleteCommand()
    //    {
    //        if (CurrentHotel != null)
    //        {
    //            return true;
    //        }
    //        else
    //        {
    //            return false;
    //        }
    //    }
    //    #endregion

    //    #region Load
    //    private void Hotel_Load()
    //    {
    //        Hotels = new ObservableCollection<Hotel_VM>(
    //            MakfiData.Hotel_Read()
    //            .Select(x => new Hotel_VM
    //            {
    //                Id = x.Id,
    //                Nom = x.Nom,
    //                Image = $"/Makrisoft.Makfi;component/Assets/hotels/{x.Nom.ToLower()}.png",
    //                Gouvernante = Reference_ViewModel.Utilisateur.DgSource
    //                            .Where(u => u.Id == x.Gouvernante).SingleOrDefault(),
    //                Reception = Reference_ViewModel.Utilisateur.DgSource
    //                            .Where(u => u.Id == x.Reception).SingleOrDefault(),
    //                Commentaire = x.Commentaire,
    //                SaveColor = "Navy"
    //            }).OrderBy(x => x.Nom).ToList()); 
    //        HotelCollectionView = new ListCollectionView(Hotels);
    //        HotelCollectionView.Refresh();

    //    }
    //    public void Gouvernante_Load()
    //    {
    //        Gouvernantes = new ObservableCollection<Utilisateur_VM>(
    //            Reference_ViewModel.Utilisateur.DgSource
    //            .Where(u => u.Statut == RoleEnum.Gouvernante));
    //    }
    //    public void Reception_Load()
    //    {
    //        Receptions = new ObservableCollection<Utilisateur_VM>(
    //            Reference_ViewModel.Utilisateur.DgSource
    //            .Where(u => u.Statut == RoleEnum.Reception));
    //    }
    //    #endregion
    //}
}
