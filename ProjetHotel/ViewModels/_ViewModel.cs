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
    public class ViewModel<VM> : ViewModelBase where VM : ViewModelBase
    {
        #region Propriétés
        protected EntiteEnum EtatType;
        protected SortDescription[] SortDescriptions = null;
        public LoadEnum Loads = LoadEnum.None;

        #endregion

        #region Constructeur
        public void Init()
        {
            // Icommand
            SaveCommand = new RelayCommand(p => OnSaveCommand(), p => OnCanExecuteSaveCommand());
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
                        MakfiData.Read<Chambre>(
                            "Chambre_Read",
                            $"<chambres><groupeChambre>{filterEtage.Id}</groupeChambre></chambres>",
                            e =>
                            {
                                e.Id = (Guid)MakfiData.Reader["Id"];
                                e.Nom = MakfiData.Reader["Nom"] as string;
                                e.Etat = (Guid)MakfiData.Reader["Etat"];
                                e.Etage = MakfiData.Reader["GroupeChambre"] as Guid?;
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
        public void OnSaveCommand()
        {

            DgSource_Save();
            DgSourceCollectionView.Refresh();
        }
        public virtual void OnAddCommand() { }
        public virtual void OnDeleteCommand(string spName, string spParam)
        {
            var canDeletes = MakfiData.CanDelete(spName, spParam);
            if (MakfiData.Erreur == string.Empty && canDeletes.Count() == 0)
            {
                var param = MakfiData.Delete(spName, spParam);
                if (MakfiData.Erreur == string.Empty && param)
                {
                    DgSource.Remove(CurrentDgSource);
                    return;
                }
            }
            MessageBox.Show(MakfiData.Erreur, "ChambreViewModel.OnDeleteCommand");
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

        public override void Load()
        {
            // Divers
            if (!(this is MessageViewModel)) Reference_ViewModel.Header.MessagesVisibility = Visibility.Visible;
            RetourIntervention = false;

            // Load
            if (Loads.HasFlag(LoadEnum.Messages)) Load_Messages();
            if (Loads.HasFlag(LoadEnum.Etats)) Load_Etats();
            if (Loads.HasFlag(LoadEnum.Employes)) Load_Employes();
            if (Loads.HasFlag(LoadEnum.Gouvernantes)) Load_Gouvernantes();
            if (Loads.HasFlag(LoadEnum.Receptions)) Load_Receptions();
            if (Loads.HasFlag(LoadEnum.Chambres)) Load_Chambres();
            if (Loads.HasFlag(LoadEnum.Etages)) Load_Etages();
            if (Loads.HasFlag(LoadEnum.Interventions)) Load_Interventions(
                $@"
                    <interventions>
                        <hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel>
                        <delete>{Reference_ViewModel.Intervention.CurrentDgSource.Id}</delete>
                    </interventions>");
            if (Loads.HasFlag(LoadEnum.Utilisateurs)) Load_Utilisateurs();

            Load_DgSource();
            //if (CurrentDgSource == null) CurrentDgSource = DgSource.FirstOrDefault();
        }
        public void Load_Chambres()
        {
            var items = MakfiData.Read<Chambre>(
                "Chambre_Read",
                $"<chambres><hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel></chambres>",
                e =>
                {
                    e.Id = (Guid)MakfiData.Reader["Id"];
                    e.Nom = MakfiData.Reader["Nom"] as string;
                    e.Etat = (Guid)MakfiData.Reader["Etat"];
                    e.Etage = MakfiData.Reader["GroupeChambre"] as Guid?;
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
        public void Load_DgSource()
        {
            var items = DgSource_Read();
            if (items == null) return;
            dgSource.Clear();
            foreach (var item in items) dgSource.Add(item);
        }
        public void Load_Employes()
        {
            var items = MakfiData
                .Read<Employe>(
                    "Employe_Read",
                    $"<employes><hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel></employes>",
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
        public void Load_Etages()
        {
            var items = MakfiData.Read<Etage>(
                "Etage_Read",
                $"<groupeChambres><hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel></groupeChambres>",
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
        public void Load_Etats()
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
        public void Load_Interventions(string xml)
        {
            var items = MakfiData.Read<Intervention>(
                "Intervention_Read",
                xml,
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
        private void Load_Utilisateurs()
        {
            var items = new ObservableCollection<Utilisateur_VM>(
                MakfiData.Read<Utilisateur>(
                   "Read_Utilisateur",
                   null,
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
        public void Load_Gouvernantes()
        {
            var items = new ObservableCollection<Utilisateur_VM>(
                MakfiData.Read<Utilisateur>(
                   "Read_Utilisateur",
                   null,
                    e =>
                    {
                        e.Id = (Guid)MakfiData.Reader["Id"];
                        e.Nom = MakfiData.Reader["Nom"] as string;
                        e.CodePin = MakfiData.Reader["CodePin"] as string;
                        e.Statut = (RoleEnum)(byte)MakfiData.Reader["Statut"];
                    })
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
                }));
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
            var items = new ObservableCollection<Utilisateur_VM>(
                MakfiData.Read<Utilisateur>(
                   "Read_Utilisateur",
                   null,
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
        public void Load_Messages()
        {
            var items = new ObservableCollection<Message_VM>(MakfiData.Read<Message>(
                 "Message_Read",
                null,
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

        #endregion

        #region DgSource
        public virtual IEnumerable<VM> DgSource_Read() { return null; }
        public virtual void DgSource_Save() { }
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
