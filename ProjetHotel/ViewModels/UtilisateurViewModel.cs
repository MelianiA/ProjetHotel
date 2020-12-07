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
    public class UtilisateurViewModel : ViewModelBase
    {
        #region Constructeur
        public UtilisateurViewModel()
        {
            RelayCommand();
            Utilisateurs = new ObservableCollection<Utilisateur_VM>(MakfiData.Utilisateur_Read()
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
            CurrentUtilisateur = Utilisateurs.First();
            UtilisateurCollectionView = new ListCollectionView(Utilisateurs);

        }

        public bool FilterUtilisateurs(object item)
        {
            if (item is Utilisateur_VM utilisateur)
            {
                return
                (RoleAdminFilter && utilisateur.Statut == RoleEnum.Admin) ||
                (RoleGouvFilter && utilisateur.Statut == RoleEnum.Gouvernante) ||
                (RoleReceptionFilter && utilisateur.Statut == RoleEnum.Reception);
            }
            return true;

        }
        #endregion

        #region Bindings

        // RoleFilter
        public bool RoleAdminFilter
        {
            get { return roleAdminFilter; }
            set
            {
                CurrentUtilisateur = null;
                roleAdminFilter = value;
                OnPropertyChanged("RoleAdminFilter");
                UtilisateurCollectionView.Filter = FilterUtilisateurs;
                UtilisateurCollectionView.Refresh();
            }
        }
        protected bool roleAdminFilter = true;

        public bool RoleGouvFilter
        {
            get { return roleGouvFilter; }
            set
            {
                CurrentUtilisateur = null;
                roleGouvFilter = value;
                OnPropertyChanged("RoleGouvFilter");
                UtilisateurCollectionView.Filter = FilterUtilisateurs;
                UtilisateurCollectionView.Refresh();
            }
        }
        protected bool roleGouvFilter = true;

        public bool RoleReceptionFilter
        {
            get { return roleReceptionFilter; }
            set
            {
                CurrentUtilisateur = null;
                roleReceptionFilter = value;
                OnPropertyChanged("RoleReceptionFilter");
                UtilisateurCollectionView.Filter = FilterUtilisateurs;
                UtilisateurCollectionView.Refresh();

            }
        }
        protected bool roleReceptionFilter = true;

        // Utilisateurs
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
                return currentUtilisateur;
            }
            set
            {
                currentUtilisateur = value;
                if (currentUtilisateur == null) IsEnabled = false;
                else IsEnabled = true;
                OnPropertyChanged("CurrentUtilisateur");

            }
        }
        private Utilisateur_VM currentUtilisateur;


        public ListCollectionView UtilisateurCollectionView
        {
            get { return utilisateurCollectionView; }
            set
            {
                utilisateurCollectionView = value;
                OnPropertyChanged("UtilisateurCollectionView");
            }
        }
        private ListCollectionView utilisateurCollectionView;

        //IsEnabled
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }
        private bool isEnabled;

        #endregion

        #region Commands
        //ICommand
        public ICommand UtilisateurModifiedSaveCommand { get; set; }
        public ICommand UtilisateurSelectedAddCommand { get; set; }
        public ICommand UtilisateurSelectedDeleteCommand { get; set; }
        public ICommand RoleCommand { get; set; }
        public ICommand FilterClearCommand { get; set; }

        // RelayCommand
        private void RelayCommand()
        {  // Icommand
            UtilisateurModifiedSaveCommand = new RelayCommand(p => OnSaveCommand(), p => OnCanExecuteSaveCommand());
            UtilisateurSelectedDeleteCommand = new RelayCommand(p => OnDeleteCommand(), p => OnCanExecuteDeleteCommand());
            UtilisateurSelectedAddCommand = new RelayCommand(p => OnAddCommand(), p => OnCanExecuteAddCommand());
            FilterClearCommand = new RelayCommand(p => OnFilterClearCommand());
            RoleCommand = new RelayCommand(p => OnRoleCommand(p));
        }

        // Méthodes OnCommand
        private void OnRoleCommand(object p)
        {
            if (currentUtilisateur == null) return;
            CurrentUtilisateur.Statut = (RoleEnum)p;
        }
        private void OnAddCommand()
        {
            CurrentUtilisateur = new Utilisateur_VM { Nom = "(A définir)", Statut = RoleEnum.Gouvernante };
            Utilisateurs.Add(CurrentUtilisateur);
            UtilisateurCollectionView.Refresh();
        }
        private void OnSaveCommand()
        {
            Guid? monID = null;
            if (CurrentUtilisateur.Id != default) monID = CurrentUtilisateur.Id;
            var param = $@"<utilisateur>
                                    <id>{monID}</id>
                                    <nom>{CurrentUtilisateur.Nom}</nom>
                                    <codePin>{CurrentUtilisateur.CodePin}</codePin>
                                    <statut>{(byte)CurrentUtilisateur.Statut}</statut>
                                </utilisateur>";
            var ids = MakfiData.Utilisateur_Save(param);
            if (ids.Count == 0) throw new Exception("Rien n'a été sauvgardé ! ");
            if (monID == null) CurrentUtilisateur.Id = ids[0].Id;
            CurrentUtilisateur.SaveColor = "Navy";
            Reference_ViewModel.Hotel.GouvernanteListLoad();
            Reference_ViewModel.Hotel.ReceptionListLoad();
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
                    Reference_ViewModel.Hotel.GouvernanteListLoad();
                    Reference_ViewModel.Hotel.ReceptionListLoad();
                }
            }
            else
            {
                MessageBox.Show($" Suppression impossible de l'utilsateur : {CurrentUtilisateur.Nom }", "Utilisateur_CanDelete");
            }
            ///

        }
        private void OnFilterClearCommand()
        {
            RoleGouvFilter = RoleReceptionFilter = RoleAdminFilter = true;
            CurrentUtilisateur = Utilisateurs.Count > 0 ? Utilisateurs[0] : null;
            UtilisateurCollectionView.Refresh();
        }


        // Méthodes OnCanExecuteCommand
        private bool OnCanExecuteAddCommand()
        {
            return true;

        }
        private bool OnCanExecuteSaveCommand()
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
        #endregion
    }
}
