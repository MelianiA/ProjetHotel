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
    public class EmployeViewModel : ViewModelBase
    {

        #region Constructeur
        public EmployeViewModel()
        {
            // Icommand
            EmployeSaveCommand = new RelayCommand(p => OnSaveCommand(), p => OnCanExecuteSaveCommand());
            EmployeAddCommand = new RelayCommand(p => OnEmployeAddCommand(), p => true);
            EmployeDeleteCommand = new RelayCommand(p => OnEmployeDeleteCommand(), p => OnCanExecuteEmployeDeleteCommand());
            FilterClearCommand = new RelayCommand(p => OnFilterClearCommand(), p => OnCanExecuteFilterClearCommand());

            // ListeView
            if (Reference_ViewModel.Header.CurrentHotel != null)
            {
                Load_Etat();
                Load_Employes();
                Load_EtatEmploye();
            }
      
        }
        #endregion

        #region Binding
        public ListCollectionView EmployesCollectionView
        {
            get { return employesCollectionView; }
            set { employesCollectionView = value; OnPropertyChanged("EmployesCollectionView"); }
        }
        private ListCollectionView employesCollectionView;


        //IsModifierEnabled
        public bool IsModifierEnabled
        {
            get { return isModifierEnabled; }
            set
            {
                isModifierEnabled = value;
                OnPropertyChanged("IsModifierEnabled");
            }
        }
        private bool isModifierEnabled;

        //Employe
        public ObservableCollection<HotelEmploye_VM> HotelEmployesCurrentHotel;
        public Employe_VM CurrentEmploye
        {
            get
            {

                return currentEmploye;

            }
            set
            {
                currentEmploye = value;
                if (currentEmploye == null) IsModifierEnabled = false;
                else IsModifierEnabled = true;
                OnPropertyChanged("CurrentEmploye");
            }
        }
        private Employe_VM currentEmploye;

        public ObservableCollection<Employe_VM> Employes
        {
            get { return employes; }
            set
            {
                employes = value;
                OnPropertyChanged("Employes");
            }
        }
        private ObservableCollection<Employe_VM> employes;

        //Etat
        public ObservableCollection<Etat_VM> EtatList
        {
            get { return etatList; }
            set
            {
                etatList = value;
                OnPropertyChanged("EtatList");
            }
        }
        private ObservableCollection<Etat_VM> etatList;
        public ListCollectionView EtatListCollectionView
        {
            get { return etatListCollectionView; }
            set { etatListCollectionView = value; OnPropertyChanged("EtatListCollectionView"); }
        }
        private ListCollectionView etatListCollectionView;

        //EtatEmploye
        public ObservableCollection<Etat_VM> EtatEmploye
        {
            get { return etatEmploye; }
            set
            {
                etatEmploye = value;
                OnPropertyChanged("EtatEmploye");
            }
        }
        private ObservableCollection<Etat_VM> etatEmploye;
        public ListCollectionView EtatEmployeCollectionView
        {
            get { return etatEmployeCollectionView; }
            set { etatEmployeCollectionView = value; OnPropertyChanged("EtatEmployeCollectionView"); }
        }
        private ListCollectionView etatEmployeCollectionView;

        //Filter
        public Etat_VM CurrentFilter
        {
            get
            {
                return currentFilter;
            }
            set
            {
                currentFilter = value;
                EmployesCollectionView.Filter = FilterEmployes;
                OnPropertyChanged("CurrentFilter");
            }
        }
        private Etat_VM currentFilter;



        #endregion

        #region Commands
        //ICommand
        public ICommand EmployeSaveCommand { get; set; }
        public ICommand EmployeAddCommand { get; set; }
        public ICommand EmployeDeleteCommand { get; set; }
        public ICommand FilterClearCommand { get; set; }

        // Méthodes OnCommand
        private void OnSaveCommand()
        {
            if (Reference_ViewModel.Header.CurrentHotel == null) {  
                MessageBox.Show($"Aucun hôtel ne vous a été assigné  ", "Impossible d'enregistrer  !");
                Employes.Remove(CurrentEmploye);
                return;
            }
            if (CurrentEmploye.Nom == null || CurrentEmploye.Prenom == null || CurrentEmploye.Etat == null)
            {
                MessageBox.Show($"Impossible de sauvgarder l'employeur actuel !", "Remarque !");
                return;
            }
            Guid? monID = null;
            if (CurrentEmploye.Id != default) monID = CurrentEmploye.Id;
            var param = $@"
                    <employe>
                        <id>{monID}</id>
                        <nom>{CurrentEmploye.Nom}</nom>
                        <prenom>{CurrentEmploye.Prenom}</prenom>
                        <etat>{CurrentEmploye.Etat.Id}</etat>
                        <commentaire>{CurrentEmploye.Commentaire}</commentaire>       
                    </employe>";
            var ids = MakfiData.Employe_Save(param);
            if (ids.Count == 0) throw new Exception("Rien n'a été sauvgardé ! ");
            CurrentEmploye.Id = ids[0].Id;
            /* --------------------------------------*/
            if(monID == default)
            {
                param = $@"
                    <hotelEmploye>
                        <hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel>
                        <employe>{ids[0].Id}</employe>       
                    </hotelEmploye>";
                var ids2 = MakfiData.HotelEmploye_Save(param);
                if (ids2.Count == 0) throw new Exception("Rien n'a été sauvgardé ! ");

            }
            CurrentEmploye.SaveColor = "Navy";
            EmployesCollectionView.Refresh();
          
        }
        private void OnEmployeAddCommand()
        {
            CurrentEmploye = new Employe_VM { Nom = "(A définir)" , Etat= EtatEmploye.Where(e=>e.Libelle== "Non disponible").SingleOrDefault() };
            Employes.Add(CurrentEmploye);
            EmployesCollectionView.Refresh();
        }
        private void OnEmployeDeleteCommand()
        {
            var param = MakfiData.HotelEmploye_Delete($"<hotelEmploye><employe>{CurrentEmploye.Id}</employe><hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel></hotelEmploye>");
            if (param)
                Employes.Remove(CurrentEmploye);
            else
                MessageBox.Show($" Suppression impossible de l'employé : {CurrentEmploye.Nom}", "Remarque !");
        }
        private void OnFilterClearCommand()
        {
            CurrentFilter = null;
        }

        // Méthodes OnCanExecuteCommand
        private bool OnCanExecuteSaveCommand()
        {
            if (CurrentEmploye != null) return true;
            else return false;
        }
        private bool OnCanExecuteEmployeDeleteCommand()
        {
            if (CurrentEmploye != null) return true;
            else return false;
        }
        private bool OnCanExecuteFilterClearCommand()
        {
            if (CurrentFilter != null) return true;
            else return false;
        }

        // Divers
        public bool FilterEmployes(object item)
        {
            if(item is Employe_VM employe)
            {
                if (CurrentFilter == null) return true;
                if (CurrentFilter.Libelle == "Disponible")
                    return (employe.Etat.Libelle == "Disponible");
                if (CurrentFilter.Libelle == "Non disponible")
                    return (employe.Etat.Libelle == "Non disponible");
                if (CurrentFilter.Libelle == "Arrêt maladie")
                    return (employe.Etat.Libelle == "Arrêt maladie");
            }
            return true;
        }

        #endregion

        #region Load
        public void Load_Employes() 
        {
            Guid? idHotel = default;
            if (Reference_ViewModel.Header.CurrentHotel != null)
                 idHotel = Reference_ViewModel.Header.CurrentHotel.Id;
            else
                MessageBox.Show($"Aucun hôtel ne vous a été assigné  ", "Impossible de se connecter  !");

            Employes = new ObservableCollection<Employe_VM>(
                  MakfiData.Employe_Read($"<employes><hotel>{idHotel}</hotel></employes>")
                  .Select(x => new Employe_VM
                  {
                      Id = x.Id,
                      Nom=x.Nom,
                      Prenom = x.Prenom,
                      Etat = EtatList.Where(e=>e.Id==x.Etat.Id).Single(),
                      Commentaire = x.Commentaire,
                      SaveColor =  "Navy"
                  }).ToList());

            EmployesCollectionView = new ListCollectionView(Employes);
            EmployesCollectionView.Refresh();
        }
        private void Load_Etat()
        {
            EtatList = new ObservableCollection<Etat_VM>(
              MakfiData.Etat_Read()
              .Select(x => new Etat_VM
              {
                  Id = x.Id,
                  Libelle = x.Libelle,
                  Icone = x.Icone,
                  Couleur = x.Couleur,
                  Entite = x.Entite,
                  EtatEtat = x.EtatEtat
              }).ToList()); ;
            EtatListCollectionView = new ListCollectionView(EtatList);
            EtatListCollectionView.Refresh();
        }
        private void Load_EtatEmploye()
        {
            EtatEmploye = new ObservableCollection<Etat_VM>(
               EtatList.Where(x => x.Entite == EntiteEnum.Employe).ToList());
            EtatEmployeCollectionView = new ListCollectionView(EtatEmploye);
            EtatEmployeCollectionView.Refresh();
            CurrentFilter = null;
        }

        #endregion
    }
}
