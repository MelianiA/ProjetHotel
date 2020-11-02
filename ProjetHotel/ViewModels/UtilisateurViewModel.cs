using Makrisoft.Makfi.Dal;
using Makrisoft.Makfi.Tools;
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

            // ListeView
            Utilisateur_Load();

            //Filter
            UtilisateurCollectionView.Filter += u =>
            {
                var utilisateur = (Utilisateur_VM)u;
                return
                    (RoleAdminFilter && utilisateur.Statut == RoleEnum.Admin) ||
                    (RoleGouvFilter && utilisateur.Statut == RoleEnum.Gouvernante) ||
                    (RoleReceptionFilter && utilisateur.Statut == RoleEnum.Reception);
            };
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
                return
                  currentUtilisateur;
            }
            set
            {

                currentUtilisateur = value;
                OnPropertyChanged("CurrentUtilisateur");


            }
        }
        private Utilisateur_VM currentUtilisateur;
        public ListCollectionView UtilisateurCollectionView
        {
            get { return utilisateurCollectionView; }
            set { utilisateurCollectionView = value; OnPropertyChanged("UtilisateurCollectionView"); }
        }
        private ListCollectionView utilisateurCollectionView;

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
        {             // Icommand
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
            var role = (RoleEnum)p;
            CurrentUtilisateur.Statut = role;
        }
        private void OnAddCommand()
        {
            CurrentUtilisateur = new Utilisateur_VM { Nom = "(A définir)", Statut = RoleEnum.Gouvernante };
            Utilisateurs.Add(CurrentUtilisateur);
            UtilisateurCollectionView.Refresh();
        }
        private void OnSaveCommand()
        {
             bool param; Utilisateur_VM utilTmp;
            if (CurrentUtilisateur.Id != default)
            {
                utilTmp = CurrentUtilisateur;
                param = MakfiData.Utilisateur_Save($"<utilisateur><id>{CurrentUtilisateur.Id}</id><nom>{CurrentUtilisateur.Nom}</nom><statut>{(byte)CurrentUtilisateur.Statut}</statut></utilisateur>");
            }
            else
            {
                utilTmp = CurrentUtilisateur;
                param = MakfiData.Utilisateur_Save($"<utilisateur><nom>{CurrentUtilisateur.Nom}</nom><statut>{(byte)CurrentUtilisateur.Statut}</statut></utilisateur>");
            }
            if (param)
            {
                CurrentUtilisateur.SaveColor = "Navy";
                Utilisateurs.Clear();
                Utilisateur_Load();
                CurrentUtilisateur = Utilisateurs.Where(u => u.Nom == utilTmp.Nom && u.Statut == utilTmp.Statut).FirstOrDefault();
                Reference_ViewModel.Hotel.GouvrnanteListLoad();
                Reference_ViewModel.Hotel.ReceptionListLoad();

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
                    Reference_ViewModel.Hotel.GouvrnanteListLoad();
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

        #region Divers
        private void Utilisateur_Load()
        {
            Utilisateurs = new ObservableCollection<Utilisateur_VM>(MakfiData.Utilisateur_Read()
             .Select(x => new Utilisateur_VM
             {
                 Id = x.Id,
                 Nom = x.Nom,
                 Image = x.Image,
                 Statut = x.Statut,
                 SaveColor = "Navy",
                 DateModified = default
             }).ToList());
            foreach (var item in Utilisateurs)
            {
                if (item.Image != null) item.Image = $"/Makrisoft.Makfi;component/Assets/Photos/{item.Image}";
                else { item.Image = $"/Makrisoft.Makfi;component/Assets/Photos/utilisateur.png"; }
            }
            // ListeView
            UtilisateurCollectionView = new ListCollectionView(Utilisateurs);
            UtilisateurCollectionView.Refresh();
        }

        #endregion
    }
}
