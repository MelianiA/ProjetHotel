using Makrisoft.Makfi.Dal;
using Makrisoft.Makfi.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Makrisoft.Makfi.ViewModels
{
    public class UtilisateurViewModel : ViewModelBase
    {
        #region Binding
        // RoleFilter
        public bool RoleAdminFilter
        {
            get { return roleAdminFilter; }
            set
            {
                roleAdminFilter = value;
                if (value == true)
                {
                    var admins = Utilisateurs.Where(x => x.Role == RoleEnum.Admin).ToList();
                    foreach (var item in admins)
                    {
                        if(!UtilisateursFiltre.Contains(item)) UtilisateursFiltre.Add(item);
                    }
                }
                else
                {
                    var admins = Utilisateurs.Where(x => x.Role == RoleEnum.Admin).ToList();
                    foreach (var item in admins) UtilisateursFiltre.Remove(item);
                }

                OnPropertyChanged("RoleAdminFilter");

            }
        }
        protected bool roleAdminFilter = true;
        public bool RoleGouvFilter
        {
            get { return roleGouvFilter; }
            set
            {
                roleGouvFilter = value;
                if (value == true)
                {
                    var Gouv = Utilisateurs.Where(x => x.Role == RoleEnum.Gouvernante).ToList();
                    foreach (var item in Gouv)
                    {
                        if (!UtilisateursFiltre.Contains(item)) UtilisateursFiltre.Add(item);
                    }
                }
                else
                {
                    var Gouv = Utilisateurs.Where(x => x.Role == RoleEnum.Gouvernante).ToList();
                    foreach (var item in Gouv) UtilisateursFiltre.Remove(item);
                }
                OnPropertyChanged("RoleGouvFilter"); 

            }
        }
        protected bool roleGouvFilter = true;
        public bool RoleReceptionFilter
        {
            get { return roleReceptionFilter; }
            set
            {
                roleReceptionFilter = value;
                if (value == true)
                {
                    var Reception = Utilisateurs.Where(x => x.Role == RoleEnum.Reception).ToList();
                    foreach (var item in Reception)
                    {
                        if (!UtilisateursFiltre.Contains(item)) UtilisateursFiltre.Add(item);
                    }
                }
                else
                {
                    var Reception = Utilisateurs.Where(x => x.Role == RoleEnum.Reception).ToList();
                    foreach (var item in Reception) UtilisateursFiltre.Remove(item);                   
                }
                OnPropertyChanged("RoleReceptionFilter"); 

            }
        }
        protected bool roleReceptionFilter = true;

        public ObservableCollection<Utilisateur_VM> UtilisateursFiltre
        {
            get { return utilisateursFiltre; }
            set
            {
                utilisateursFiltre = value;
                OnPropertyChanged("UtilisateursFiltre");
            }
        }
        private ObservableCollection<Utilisateur_VM> utilisateursFiltre;
        public ObservableCollection<Utilisateur_VM> Utilisateurs
        {
            get { return utilisateurs; }
            set
            {
                utilisateurs = value;
                OnPropertyChanged("Utilisateurs");
            }
        }
        private ObservableCollection<Utilisateur_VM> utilisateurs;

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
                OnPropertyChanged("CurrentUtilisateur");
                SaveColor = "Red";
            }
        }
        private Utilisateur_VM currentUtilisateur;
        public ListCollectionView EntityCollectionView
        {
            get { return entityCollectionView; }
            set { entityCollectionView = value; OnPropertyChanged("EntityCollectionView"); }
        }
        private ListCollectionView entityCollectionView = null;
        public string SaveColor { get { return saveColor; } set { saveColor = value; OnPropertyChanged("SaveColor"); } }
        private string saveColor = "Navy";
        #endregion

        #region Commands
        public ICommand UtilisateurModifiedSaveCommand { get; set; }
        public ICommand UtilisateurSelectedAddCommand { get; set; }
        public ICommand UtilisateurSelectedDeleteCommand { get; set; }
        #endregion


        #region Constructeur
        public UtilisateurViewModel()
        {
            Utilisateur_Load();

            RelayCommand();


        }
        #endregion

        #region Méthodes
        private void OnSaveCommand()
        {
            bool param;
            if (CurrentUtilisateur.Id != default)
            {
                param = MakfiData.Utilisateur_Save($"<utilisateur><id>{CurrentUtilisateur.Id}</id><nom>{CurrentUtilisateur.Nom}</nom><statut>{CurrentUtilisateur.Statut}</statut></utilisateur>");
            }
            else
            {
                param = MakfiData.Utilisateur_Save($"<utilisateur><nom>{CurrentUtilisateur.Nom}</nom><statut>{CurrentUtilisateur.Statut}</statut></utilisateur>");
            }
            if (param)
            {
                Utilisateurs.Clear();
                Utilisateur_Load();
                SaveColor = "Navy";
            }
        }
        private void Utilisateur_Load()
        {
            var tmp = MakfiData.Utilisateur_Read()
               .Select(x => new Utilisateur_VM
               {
                   Id = x.Id,
                   Nom = x.Nom,
                   Image = $"/Makrisoft.Makfi;component/Assets/Photos/{x.Image}",
                   Statut = x.Statut,
                   Role = x.Role,
                   DateModified = default
               }).ToList();

            Utilisateurs = new ObservableCollection<Utilisateur_VM>(tmp) ;
            UtilisateursFiltre = new ObservableCollection<Utilisateur_VM>(tmp);

           
        }
        private bool OnCanExecuteSaveCommand()
        {
            if (SaveColor == "Red" && CurrentUtilisateur != null)
            {
                return true;
            }
            else
            {
                //EntityCollectionView.Refresh();
                return false;
            }

        }

        private void RelayCommand()
        {
            //FilterClearCommand = new RelayCommand(p => OnFilterClearCommand());
            UtilisateurModifiedSaveCommand = new RelayCommand(p => OnSaveCommand(), p => OnCanExecuteSaveCommand());
            UtilisateurSelectedDeleteCommand = new RelayCommand(p => OnDeleteCommand(), p => OnCanExecuteDeleteCommand());
            UtilisateurSelectedAddCommand = new RelayCommand(p => OnAddCommand(), p => OnCanExecuteAddCommand());
        }

        private bool OnCanExecuteDeleteCommand()
        {
            if (CurrentUtilisateur != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void OnDeleteCommand()
        {
            var canDeletes = MakfiData.Utilisateur_CanDelete($"<utilisateur><id>{CurrentUtilisateur.Id}</id></utilisateur>");
            if (canDeletes.Count() == 0)
            {
                var param = MakfiData.Utilisateur_Delete($"<utilisateur><id>{CurrentUtilisateur.Id}</id></utilisateur>");
                if (param)
                {
                    Utilisateurs.Remove(CurrentUtilisateur);
                    Utilisateur_Load();
                }
            }
            else
            {
                MessageBox.Show($" Suppression impossible :  {string.Join(",", canDeletes.Select(c => c.Table).ToList())}", "Utilisateur_CanDelete");
            }
            ///


        }

        private bool OnCanExecuteAddCommand()
        {
            if (SaveColor == "Navy") return true;
            else return false;
        }

        private void OnAddCommand()
        {
            CurrentUtilisateur = new Utilisateur_VM { Nom = "(A définir)", Role = RoleEnum.None };
            Utilisateurs.Add(CurrentUtilisateur);
            EntityCollectionView.Refresh();
        }


        #endregion







    }
}
