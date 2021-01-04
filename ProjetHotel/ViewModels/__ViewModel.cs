using Makrisoft.Makfi.Dal;
using Makrisoft.Makfi.Models;
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
    public class ViewModel<VM, M> : ViewModelBase
        where VM : ViewModelBase
        where M : Modele, new()
    {
        #region Propriétés
        protected EntiteEnum EtatType;
        protected SortDescription[] SortDescriptions = null;
        public LoadEnum Loads = LoadEnum.None;

        #endregion

        #region Constructeur
        public void Init<T>() where T : Modele, new()
        {
            // Icommand
            SaveCommand = new RelayCommand(p => OnSaveCommand<T>(), p => OnCanExecuteSaveCommand());
            AddCommand = new RelayCommand(p => OnAddCommand(), p => true);
            DeleteCommand = new RelayCommand(p => OnDeleteCommand(null, null), p => OnCanExecuteDeleteCommand());
            FilterClearCommand = new RelayCommand(p => OnFilterClearCommand());
            ChangeViewCommand = new RelayCommand(p => OnChangeViewCommand(), p => OnCanExecuteChangeView());

            // DgSourceCollectionView
            DgSource = new ObservableCollection<VM>();
            DgSourceCollectionView = new ListCollectionView(DgSource) { Filter = DgSource_Filter };
            if (SortDescriptions != null) foreach (var d in SortDescriptions) DgSourceCollectionView.SortDescriptions.Add(d);
        }


        #endregion

        #region Binding
        // DgSource
        public ObservableCollection<VM> DgSource
        {
            get { return dgSource; }
            set { dgSource = value; OnPropertyChanged("DgSource"); }
        }
        private ObservableCollection<VM> dgSource;
        public VM CurrentDgSource
        {
            get { return currentDgSource; }
            set
            {
                currentDgSource = value;
                IsModifierEnabled = currentDgSource != null;
                Reference_ViewModel.Header.MessageClear();
                DgSource_Change();
                OnPropertyChanged("CurrentDgSource");
            }
        }

        public virtual void DgSource_Change()
        {
        }

        private VM currentDgSource;
        public ListCollectionView DgSourceCollectionView
        {
            get { return dgSourceCollectionView; }
            set
            {
                dgSourceCollectionView = value;
                OnPropertyChanged("DgSourceCollectionView");
            }
        }
        private ListCollectionView dgSourceCollectionView;

        // IsModifierEnabled
        public bool IsModifierEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                OnPropertyChanged("IsModifierEnabled");
            }
        }
        private bool isEnabled;

        // Etats
        public ObservableCollection<Etat_VM> Etats
        {
            get { return etats; }
            set
            {
                etats = value;
                OnPropertyChanged("Etats");
            }
        }
        private ObservableCollection<Etat_VM> etats;

        // Employes
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

        // Chambres
        public ObservableCollection<Chambre_VM> Chambres
        {
            get { return chambres; }
            set
            {
                chambres = value;
                OnPropertyChanged("Chambres");
            }
        }
        private ObservableCollection<Chambre_VM> chambres;

        // Etages
        public ObservableCollection<Etage_VM> Etages
        {
            get { return etages; }
            set
            {
                etages = value;
                OnPropertyChanged("Etages");
            }
        }
        private ObservableCollection<Etage_VM> etages;

        // Interventions
        public ObservableCollection<Intervention_VM> Interventions
        {
            get { return interventions; }
            set
            {
                interventions = value;
                OnPropertyChanged("Interventions");
            }
        }
        private ObservableCollection<Intervention_VM> interventions;

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

        // Messages
        public ObservableCollection<Message_VM> Messages
        {
            get { return messages; }
            set
            {
                messages = value;
                OnPropertyChanged("Messages");
            }
        }
        private ObservableCollection<Message_VM> messages;


        // Filter
        public Utilisateur_VM FilterUtilisateur
        {
            get { return filterUtilisateur; }
            set
            {
                filterUtilisateur = value;
                OnPropertyChanged("FilterUtilisateur");
                DgSourceCollectionView.Refresh();
            }
        }
        private Utilisateur_VM filterUtilisateur;
        public Etat_VM FilterEtat
        {
            get { return filterEtat; }
            set
            {
                filterEtat = value;
                OnPropertyChanged("FilterEtat");
                DgSourceCollectionView.Refresh();
            }
        }
        private Etat_VM filterEtat;
        public Etage_VM FilterEtage
        {
            get { return filterEtage; }
            set
            {
                filterEtage = value;
                if (FilterEtage != null)
                {
                    filterEtage.Chambres = new ObservableCollection<Chambre_VM>(
                        MakfiData.Crud<Chambre>(
                            "Chambre_Read",
                            $"<chambres><hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel><groupeChambre>{filterEtage.Id}</groupeChambre></chambres>",
                            e =>
                            {
                                e.Id = (Guid)MakfiData.Reader["Id"];
                                e.Nom = MakfiData.Reader["Nom"] as string;
                                e.Etat = (Guid)MakfiData.Reader["Etat"];
                                e.Commentaire = MakfiData.Reader["Commentaire"] as string;
                            })
                            .Select(x => new Chambre_VM
                            {
                                Id = x.Id
                            }));
                }
                OnPropertyChanged("FilterEtage");
                DgSourceCollectionView.Refresh();
            }
        }
        private Etage_VM filterEtage;
        public Employe_VM FilterEmploye
        {
            get { return filterEmploye; }
            set
            {
                filterEmploye = value;
                OnPropertyChanged("FilterEmploye");
                DgSourceCollectionView.Refresh();
            }
        }
        private Employe_VM filterEmploye;
        public DateTime? FilterDateDebut
        {
            get { return filterDateDebut; }
            set
            {
                filterDateDebut = value;
                OnPropertyChanged("FilterDateDebut");
                DgSourceCollectionView.Refresh();

            }
        }
        private DateTime? filterDateDebut = null;
        public DateTime? FilterDateFin
        {
            get { return filterDateFin; }
            set
            {
                filterDateFin = value;
                OnPropertyChanged("FilterDateFin");
                DgSourceCollectionView.Refresh();
            }
        }
        private DateTime? filterDateFin = null;

        // Retour à cette page
        public bool RetourIntervention
        {
            get { return revientIci; }
            set { revientIci = value; }
        }
        public bool revientIci = false;

        public string Title { get { return title; } set { title = value; OnPropertyChanged("Title"); } }
        private string title = "Non défini";
        #endregion

        #region Commands
        // ICommand
        public ICommand SaveCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand FilterClearCommand { get; set; }
        public ICommand ChangeViewCommand { get; set; }

        // Méthodes OnCommand
        public virtual void OnSaveCommand<T>() where T : Modele, new()
        {
            DgSource_Save(null, null);
            //DgSourceCollectionView.Refresh();
        }
        public virtual void OnAddCommand() { }
        public virtual void OnDeleteCommand(string spName, string spParam)
        {
            var canDeletes = MakfiData.CanDelete( spName.Replace("_Delete", "_CanDelete"), spParam);
            if (MakfiData.Erreur == string.Empty)
            {
                if (canDeletes.Count() != 0)
                {
                    Reference_ViewModel.Header.MessageBox($"Suppression impossible !");
                    return;
                }
                var ids = MakfiData.Crud<M>(spName, spParam);
                if (ids.Count == 0) throw new Exception($"{spName.Replace("_Delete", "ViewModel")}.OnDeleteCommand");
                if (MakfiData.Erreur == string.Empty)
                {
                    DgSource.Remove(CurrentDgSource);
                    return;
                }
            }
            MessageBox.Show($"{MakfiData.Erreur}{Environment.NewLine}{spParam}");
        }

        public virtual void OnFilterClearCommand()
        {
            FilterEtat = null;
            FilterDateDebut = null;
            FilterDateFin = null;
            FilterEtage = null;
            FilterEmploye = null;
            FilterUtilisateur = null;
        }
        public virtual void OnChangeViewCommand() { }

        // Méthodes OnCanExecuteCommand
        public virtual bool OnCanExecuteSaveCommand() { return CurrentDgSource != null; }
        public virtual bool OnCanExecuteDeleteCommand() { return CurrentDgSource != null; }
        public virtual bool OnCanExecuteChangeView() { return CurrentDgSource != null && CurrentDgSource.SaveColor != "Red"; }

        //Filter 
        public virtual bool DgSource_Filter(object item) { return true; }
        #endregion

        #region Load
        // Global
        public override void Load()
        {
            // Divers
            Reference_ViewModel.Header.MessagesVisibility = this is MessageViewModel ? Visibility.Hidden : Visibility.Visible;
            RetourIntervention = false;

            // Load
            if (Loads.HasFlag(LoadEnum.Messages)) Load_Messages("Message_Read", null);
            if (Loads.HasFlag(LoadEnum.Etats)) Load_Etats(null, null);
            if (Loads.HasFlag(LoadEnum.Employes)) Load_Employes("Employe_Read", $"<employes><hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel></employes>");
            if (Loads.HasFlag(LoadEnum.Gouvernantes)) Load_Gouvernantes("Utilisateur_Read", $"<utilisateurs><statut>{(int)RoleEnum.Gouvernante}</statut></utilisateurs>");
            if (Loads.HasFlag(LoadEnum.Receptions)) Load_Receptions("Utilisateur_Read", $"<utilisateurs><statut>{(int)RoleEnum.Reception}</statut></utilisateurs>");
            if (Loads.HasFlag(LoadEnum.Chambres)) Load_Chambres("Chambre_Read", $"<chambres><hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel></chambres>");
            if (Loads.HasFlag(LoadEnum.Etages)) Load_Etages("GroupeChambre_Read", $"<groupeChambres><hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel></groupeChambres>");
            if (Loads.HasFlag(LoadEnum.Interventions)) Load_Interventions("Intervention_Read", $@"<interventions><hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel><delete>{Reference_ViewModel.Intervention.CurrentDgSource.Id}</delete></interventions>");
            if (Loads.HasFlag(LoadEnum.Utilisateurs)) Load_Utilisateurs("Utilisateur_Read", null);

            Load_DgSource();
        }

        // Détails
        private void Load_Employes(string spName, string spParam)
        {
            var items = MakfiData
                .Crud<Employe>(
                    spName,
                    spParam,
                    e =>
                    {
                        e.Id = (Guid)MakfiData.Reader["Id"];
                        e.Nom = MakfiData.Reader["Nom"] as string;
                        e.Prenom = MakfiData.Reader["Prenom"] as string;
                        e.Commentaire = MakfiData.Reader["Commentaire"] as string;
                        e.Etat = (Guid)MakfiData.Reader["Etat"];
                    })
                .Select(x => new Employe_VM
                {
                    Id = x.Id,
                    Nom = x.Nom,
                    Prenom = x.Prenom,
                    Etat = MakfiData.Etats.Where(e => e.Id == x.Etat).Single(),
                    Commentaire = x.Commentaire,
                    SaveColor = "Navy"
                });
            if (Employes == null)
                Employes = new ObservableCollection<Employe_VM>(items);
            else
            {
                Employes.Clear();
                foreach (var item in items) Employes.Add(item);
            }
        }
        private void Load_Chambres(string spName, string spParam)
        {
            var items = MakfiData.Crud<Chambre>(
                spName,
                spParam,
                e =>
                {
                    e.Id = (Guid)MakfiData.Reader["Id"];
                    e.Nom = MakfiData.Reader["Nom"] as string;
                    e.Etat = (Guid)MakfiData.Reader["Etat"];
                    e.Commentaire = MakfiData.Reader["Commentaire"] as string;
                })
                .Select(x => new Chambre_VM
                {
                    Id = x.Id,
                    Nom = x.Nom,
                    Etat = MakfiData.Etats.Where(e => e.Id == x.Etat).Single(),
                    Commentaire = x.Commentaire,
                    SaveColor = "Navy"
                })
                .OrderBy(c => c.Nom);
            if (Chambres == null)
                Chambres = new ObservableCollection<Chambre_VM>(items);
            else
            {
                Chambres.Clear();
                foreach (var item in items) Chambres.Add(item);
            }
        }
        private void Load_Etages(string spName, string spParam)
        {
            var items = MakfiData.Crud<Etage>(
                spName,
                spParam,
                e =>
                {
                    e.Id = (Guid)MakfiData.Reader["Id"];
                    e.Nom = MakfiData.Reader["Nom"] as string;
                    e.Commentaire = MakfiData.Reader["Commentaire"] as string;
                })
                .Select(x => new Etage_VM
                {
                    Id = x.Id,
                    Nom = x.Nom,
                    Commentaire = x.Commentaire,
                    SaveColor = "Navy"
                }).OrderBy(x => x.Nom).ToList();

            if (Etages == null)
                Etages = new ObservableCollection<Etage_VM>(items);
            else
            {
                Etages.Clear();
                foreach (var item in items) Etages.Add(item);
            }
        }
        private void Load_Etats(string spName, string spParam)
        {
            var items = MakfiData.Etats.Where(x => x.Entite == EtatType);
            if (Etats == null)
                Etats = new ObservableCollection<Etat_VM>(items);
            else
            {
                Etats.Clear();
                foreach (var item in items) Etats.Add(item);
            }
        }
        private void Load_Interventions(string spName, string spParam)
        {
            var items = MakfiData.Crud<Intervention>(
                spName,
                spParam,
                e =>
                {
                    e.Id = (Guid)MakfiData.Reader["Id"];
                    e.Libelle = MakfiData.Reader["Libelle"] as string;
                    e.Etat = (Guid)MakfiData.Reader["Etat"];
                    e.Date1 = (DateTime)MakfiData.Reader["Date1"];
                    e.Commentaire = MakfiData.Reader["Commentaire"] as string;
                    e.IsModele = (bool)MakfiData.Reader["Model"];
                })
              .Select(x => new Intervention_VM
              {
                  Id = x.Id,
                  Libelle = x.Libelle,
                  Etat = MakfiData.Etats.Where(e => e.Id == x.Etat).Single(),
                  Date1 = x.Date1,
                  Commentaire = x.Commentaire,
                  Model = x.IsModele,
                  SaveColor = "Navy"
              }).OrderBy(x => x.Libelle).ToList();


            if (Interventions == null)
                Interventions = new ObservableCollection<Intervention_VM>(items);
            else
            {
                Interventions.Clear();
                foreach (var item in items) Interventions.Add(item);
            }
        }
        private void Load_Utilisateurs(string spName, string spParam)
        {
            var items = new ObservableCollection<Utilisateur_VM>(
                MakfiData.Crud<Utilisateur>(
                   spName,
                   spParam,
                    e =>
                    {
                        e.Id = (Guid)MakfiData.Reader["Id"];
                        e.Nom = MakfiData.Reader["Nom"] as string;
                        e.CodePin = MakfiData.Reader["CodePin"] as string;
                        e.Statut = (RoleEnum)(byte)MakfiData.Reader["Statut"];
                    })
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
            if (Utilisateurs == null)
                Utilisateurs = new ObservableCollection<Utilisateur_VM>(items);
            else
            {
                Utilisateurs.Clear();
                foreach (var item in items) Utilisateurs.Add(item);
            }
        }
        private void Load_Gouvernantes(string spName, string spParam)
        {
            var items = new ObservableCollection<Utilisateur_VM>(
                MakfiData.Crud<Utilisateur>(
                   spName,
                   spParam,
                    e =>
                    {
                        e.Id = (Guid)MakfiData.Reader["Id"];
                        e.Nom = MakfiData.Reader["Nom"] as string;
                        e.CodePin = MakfiData.Reader["CodePin"] as string;
                        e.Statut = (RoleEnum)(byte)MakfiData.Reader["Statut"];
                    })
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
            if (Gouvernantes == null)
                Gouvernantes = new ObservableCollection<Utilisateur_VM>(items);
            else
            {
                Gouvernantes.Clear();
                foreach (var item in items) Gouvernantes.Add(item);
            }
        }
        private void Load_Receptions(string spName, string spParam)
        {
            var items = new ObservableCollection<Utilisateur_VM>(
                MakfiData.Crud<Utilisateur>(
                   spName,
                   spParam,
                    e =>
                    {
                        e.Id = (Guid)MakfiData.Reader["Id"];
                        e.Nom = MakfiData.Reader["Nom"] as string;
                        e.CodePin = MakfiData.Reader["CodePin"] as string;
                        e.Statut = (RoleEnum)(byte)MakfiData.Reader["Statut"];
                    })
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
                Receptions.Clear();
                foreach (var item in items) Receptions.Add(item);
            }
        }
        private void Load_Messages(string spName, string spParam)
        {
            var items = new ObservableCollection<Message_VM>(MakfiData.Crud<Message>(
                 spName,
                spParam,
                e =>
                {
                    e.Id = (Guid)MakfiData.Reader["Id"];
                    e.IdHisto = MakfiData.Reader["IdHisto"] as Guid?;
                    e.De = MakfiData.Reader["De"] as Guid?;
                    e.A = MakfiData.Reader["A"] as Guid?;
                    e.DateEnvoi = (DateTime)MakfiData.Reader["EnvoyeLe"];
                    e.Etat = (Guid)MakfiData.Reader["Etat"];
                    e.Libelle = MakfiData.Reader["Libelle"] as string;
                    e.Objet = MakfiData.Reader["Objet"] as string;
                })
                .Select(x => new Message_VM
                {
                    Id = x.Id,
                    SaveColor = "Navy"
                }));
            if (Messages == null)
                Messages = new ObservableCollection<Message_VM>(items);
            else
            {
                Messages.Clear();
                foreach (var item in items) Messages.Add(item);
            }
        }

        // Load_DgSource
        public void Load_DgSource()
        {
            var items = DgSource_Read();
            if (items == null) return;
            dgSource.Clear();
            foreach (var item in items) dgSource.Add(item);
        }
        #endregion

        #region DgSource_Read - DgSource_Save
        public virtual IEnumerable<VM> DgSource_Read() { return null; }
        public virtual void DgSource_Save(string spName, string spParam)
        {
            var ids = MakfiData.Crud<M>(spName, spParam);
            if (ids.Count == 0) throw new Exception($"{spName.Replace("_Save", "ViewModel")}.DgSource_Save");
            CurrentDgSource.Id = ids[0].Id;
            CurrentDgSource.SaveColor = "Navy";
        }
        #endregion
    }

    [Flags]
    public enum LoadEnum
    {
        None = 0,
        Etats = 1,
        Employes = 2,
        Chambres = 4,
        Etages = 8,
        DateDebut = 16,
        DateFin = 32,
        Interventions = 64,
        Utilisateurs = 128,
        Gouvernantes = 256,
        Receptions = 512,
        Messages = 1024
    }

}
