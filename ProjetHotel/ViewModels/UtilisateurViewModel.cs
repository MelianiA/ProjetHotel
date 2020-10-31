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


        // Méthodes OnCanExecuteXXXXCommand

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
       
        //Autres Méthodes
        private bool UtilisateurFilter(object u)
        {
            var utilisateur = (Utilisateur_VM)u;
            return
                (RoleAdminFilter && utilisateur.Statut == RoleEnum.Admin) ||
                (RoleGouvFilter && utilisateur.Statut == RoleEnum.Gouvernante) ||
                (RoleReceptionFilter && utilisateur.Statut == RoleEnum.Reception);
        }
        private void Utilisateur_Load()
        {
             Utilisateurs = new ObservableCollection<Utilisateur_VM>(MakfiData.Utilisateur_Read()
              .Select(x => new Utilisateur_VM
              {
                  Id = x.Id,
                  Nom = x.Nom,
                  Image = $"/Makrisoft.Makfi;component/Assets/Photos/{x.Image}",
                  Statut = x.Statut,
                  SaveColor = "Navy",
                  DateModified = default
              }).ToList());
            // ListeView
            UtilisateurCollectionView = new ListCollectionView(Utilisateurs);
            UtilisateurCollectionView.Refresh();
        }

        #endregion

        #region Constructeur
        public UtilisateurViewModel()
        {
            // Icommand
            UtilisateurModifiedSaveCommand = new RelayCommand(p => OnSaveCommand(), p => OnCanExecuteSaveCommand());
            UtilisateurSelectedDeleteCommand = new RelayCommand(p => OnDeleteCommand(), p => OnCanExecuteDeleteCommand());
            UtilisateurSelectedAddCommand = new RelayCommand(p => OnAddCommand(), p => OnCanExecuteAddCommand());
            FilterClearCommand = new RelayCommand(p => OnFilterClearCommand());
            RoleCommand = new RelayCommand(p => OnRoleCommand(p));

            // ListeView
            Utilisateur_Load();

            //Filter
            UtilisateurCollectionView.Filter += u => UtilisateurFilter(u);

        }



        #endregion

    }
}
