using Makrisoft.Makfi.Dal;
using Makrisoft.Makfi.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml;
using System.Xml.Serialization;

namespace Makrisoft.Makfi.ViewModels
{
    public class MessageViewModel : ViewModelBase
    {
        #region Constructeur
        public MessageViewModel()
        {

            //Command
            AjouterMessage = new RelayCommand(p => OnAjouterMessage(), p => true);
            ActualiserMessage = new RelayCommand(p => OnActualiserMessage(), p => true);
            MessageSaveCommand = new RelayCommand(p => OnMessageSaveCommand(), p => OnCanExecuteMessageSaveCommand());
            SupprimerMessage = new RelayCommand(p => OnSupprimerMessageCommand(), p => OnCanExecuteSupprimerMessageCommand());
            RepondreMessage = new RelayCommand(p => OnRepondreMessage(), p => OnCanExecuteRepondreMessageCommand());
            FilterClearCommand = new RelayCommand(p => OnFilterClearCommand(), p => OnCanExecuteFilterClearCommand());



            //Load 
            
            Messages = new ObservableCollection<Message_VM>();
            MessagesCollectionView = new ListCollectionView(Messages);
            MessagesCollectionView.SortDescriptions.Add(new System.ComponentModel.SortDescription("DateCreation", System.ComponentModel.ListSortDirection.Descending));

            Load_Etat();
            Load_Message();
            Reference_ViewModel.Header.MessagesCollectionView = MessagesCollectionView;

        }


        #endregion

        #region Bindings
        // Utilisateur
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
        public bool CanChangeUtilisateur
        {
            get { return canChangeUtilisateur; }
            set
            {
                canChangeUtilisateur = value;
                OnPropertyChanged("CanChangeUtilisateur");
            }
        }
        private bool canChangeUtilisateur = true;

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

        //EtatMessage
        public ObservableCollection<Etat_VM> EtatMessage
        {
            get { return etatMessage; }
            set
            {
                etatMessage = value;
                OnPropertyChanged("EtatMessage");
            }
        }
        private ObservableCollection<Etat_VM> etatMessage;
        public ListCollectionView EtatMessageCollectionView
        {
            get { return etatMessageCollectionView; }
            set
            {
                etatMessageCollectionView = value;
                OnPropertyChanged("EtatMessageCollectionView");
            }
        }
        private ListCollectionView etatMessageCollectionView;

        //Message

        public ObservableCollection<Message_VM> Messages
        {
            get { return messages; }
            set { messages = value; OnPropertyChanged("Messages"); }
        }
        private ObservableCollection<Message_VM> messages;

        public Message_VM CurrentMessage
        {
            get { return currentMessage; }
            set
            {
                currentMessage = value;
                if (currentMessage != null)
                {
                    HistEnable = true;
                    if (currentMessage.A != null &&
                    currentMessage.A.Id == Reference_ViewModel.Header.CurrentUtilisateur.Id && currentMessage.ColorEtat == "orange")
                    {
                        currentMessage.Etat = EtatMessage.Where(e => e.Libelle == "Lu").Single();
                        currentMessage.ColorEtat = "black";
                        OnMessageSaveCommand();
                    }
                    if (CurrentMessage.Etat != null)
                    {
                        IsEnabled = false;
                        IsReadOnly = true;
                    }
                    else
                    {
                        IsEnabled = true;
                        IsReadOnly = false;
                        CurrentMessage.Etat = EtatMessage.Where(x => x.Libelle == "Non lu").Single();

                    }
                }
                else
                    HistEnable = false;

                OnPropertyChanged("CurrentMessage");
            }
        }
        private Message_VM currentMessage;

        public ListCollectionView MessagesCollectionView
        {
            get { return messagesCollectionView; }
            set
            {
                messagesCollectionView = value;
                OnPropertyChanged("MessagesCollectionView");
            }
        }
        private ListCollectionView messagesCollectionView;

        //Filter
        public Etat_VM CurrentFilterEtat
        {
            get { return currentFilterEtat; }
            set
            {
                currentFilterEtat = value;
                if (MessagesCollectionView != null)
                    MessagesCollectionView.Filter = FilterMessages;
                OnPropertyChanged("CurrentFilterEtat");
            }
        }
        private Etat_VM currentFilterEtat;

        public Utilisateur_VM CurrentFilterUtilisateur
        {
            get { return currentFilterUtilisateur; }
            set
            {
                currentFilterUtilisateur = value;
                if (MessagesCollectionView != null)
                    MessagesCollectionView.Filter = FilterMessages;
                OnPropertyChanged("CurrentFilterUtilisateur");
            }
        }
        private Utilisateur_VM currentFilterUtilisateur;

        public bool HistoriqueFilter
        {
            get { return historiqueFilter; }
            set
            {
                historiqueFilter = value;
                if (MessagesCollectionView != null)
                    MessagesCollectionView.Filter = FilterMessages;
                OnPropertyChanged("HistoriqueFilter");
            }
        }
        private bool historiqueFilter = false;

        public bool HistEnable
        {
            get { return histEnable; }
            set
            {
                histEnable = value;
                OnPropertyChanged("HistEnable");
            }
        }
        private bool histEnable;

        //IsEnable
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
        public bool IsReadOnly
        {
            get { return isReadOnly; }
            set
            {
                isReadOnly = value;
                OnPropertyChanged("IsReadOnly");
            }
        }
        private bool isReadOnly;

        #endregion

        #region Command
        // ICommand
        //public ICommand PersistMessageCommand { get; set; }
        public ICommand AjouterMessage { get; set; }
        public ICommand ActualiserMessage { get; set; }
        public ICommand MessageSaveCommand { get; set; }
        public ICommand SupprimerMessage { get; set; }
        public ICommand RepondreMessage { get; set; }
        public ICommand FilterClearCommand { get; set; }


        // Méthode OnCommand
        private void OnAjouterMessage()
        {
            IsEnabled = true;
            CurrentMessage = new Message_VM
            {
                Id = Guid.NewGuid(),
                MessageInitial = Guid.NewGuid(),
                De = Reference_ViewModel.Header.CurrentUtilisateur,
                DateCreation = DateTime.Now,
                ColorEtat = "black",
            };
            Messages.Add(CurrentMessage);
        }
        private void OnActualiserMessage()
        {
            Load_Message();
        }
        private void OnMessageSaveCommand()
        {
            //CurrentMessage.Etat = EtatMessage.Where(x => x.Libelle == "Non lu").Single();
            var s = new XmlSerializer(typeof(Message_VM));
            var writer = new StreamWriter($@"{Properties.Settings.Default.MessagePath}\{CurrentMessage.Id}.xml");
            s.Serialize(writer, CurrentMessage);
            writer.Close();
            CurrentMessage.SaveColor = "Navy";
        }
        private void OnSupprimerMessageCommand()
        {
            foreach (var file in Directory.GetFiles(Properties.Settings.Default.MessagePath))
            {
                var s = new XmlSerializer(typeof(Message_VM));

                //Reader
                var reader = new StreamReader(file);
                var message = (Message_VM)s.Deserialize(reader);
                reader.Close();
                if (message.Id == CurrentMessage.Id)
                {
                    message.Etat = EtatMessage.Where(e => e.Libelle == "Supprimé" && e.Entite == EntiteEnum.Message).Single();

                    //Writer
                    var writer = new StreamWriter(file);
                    s.Serialize(writer, message);
                    writer.Close();
                }

            }
            Messages.Remove(CurrentMessage);
        }
        private void OnFilterClearCommand()
        {
            CurrentFilterEtat = null;
            CurrentFilterUtilisateur = null;
            HistoriqueFilter = false;
        }
        private void OnRepondreMessage()
        {
            var msgIntial = currentMessage.MessageInitial;
            var objet = currentMessage.Objet;
            var a = currentMessage.De;
            CurrentMessage = new Message_VM
            {
                Id = Guid.NewGuid(),
                A = a,
                MessageInitial = msgIntial,
                De = Reference_ViewModel.Header.CurrentUtilisateur,
                DateCreation = DateTime.Now,
                ColorEtat = "black",
                Objet = objet
            };
            IsEnabled = false;
            Messages.Add(CurrentMessage);

        }

        //Méthode on canExcute
        private bool OnCanExecuteMessageSaveCommand()
        {
            return CurrentMessage != null && CurrentMessage.SaveColor == "Red" && CurrentMessage.A != null;
        }
        private bool OnCanExecuteSupprimerMessageCommand()
        {
            if (CurrentMessage != null && CurrentMessage.SaveColor != "Red")
                return (CurrentMessage.Etat.Libelle != "Non lu");
            else
                return true;
        }
        private bool OnCanExecuteFilterClearCommand()
        {
            return (CurrentFilterEtat != null || CurrentFilterUtilisateur != null || HistoriqueFilter == true);
        }
        private bool OnCanExecuteRepondreMessageCommand()
        {
            return CurrentMessage != null && CurrentMessage.A != null &&
                CurrentMessage.A.Id == Reference_ViewModel.Header.CurrentUtilisateur.Id;
        }

        //filter 
        private bool FilterMessages(object item)
        {

            if (item is Message_VM message)
            {
                //3filter au meme temps 
                if ((CurrentMessage != null && HistoriqueFilter == true) && CurrentFilterEtat != null && CurrentFilterUtilisateur != null)
                    return message.Etat.Id == currentFilterEtat.Id && message.A.Id == CurrentFilterUtilisateur.Id && message.MessageInitial == CurrentMessage.MessageInitial;

                //etat et utilisateur 
                if (CurrentFilterEtat != null && CurrentFilterUtilisateur != null)
                    return message.Etat.Id == currentFilterEtat.Id && message.A.Id == CurrentFilterUtilisateur.Id;

                //etat et historique 
                if (CurrentFilterEtat != null && (CurrentMessage != null && HistoriqueFilter == true))
                    return message.Etat.Id == currentFilterEtat.Id && message.MessageInitial == CurrentMessage.MessageInitial;

                //utilisateur et hitorique 
                if ((CurrentMessage != null && HistoriqueFilter == true) && CurrentFilterUtilisateur != null)
                    return message.A.Id == CurrentFilterUtilisateur.Id && message.MessageInitial == CurrentMessage.MessageInitial;

                if (CurrentFilterEtat != null)
                    return message.Etat.Id == currentFilterEtat.Id;

                if (CurrentFilterUtilisateur != null)
                    return message.A.Id == CurrentFilterUtilisateur.Id;

                //L'historique 
                if (CurrentMessage != null && HistoriqueFilter == true)
                    return message.MessageInitial == CurrentMessage.MessageInitial;
                else
                    return true;
            }
            return true;
        }

        #endregion

        #region Load

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
            var etatMsg = EtatList.Where(x => x.Entite == EntiteEnum.Message).ToList();
            etatMsg.Remove(etatMsg.Where(e => e.Libelle == "Supprimé" && e.Entite == EntiteEnum.Message).SingleOrDefault());
            EtatMessage = new ObservableCollection<Etat_VM>(EtatList.Where(x => x.Entite == EntiteEnum.Message).ToList());
            EtatMessageCollectionView = new ListCollectionView(etatMsg);
        }

        public void Load_Message()
        {
            Utilisateurs = Reference_ViewModel.Header.Utilisateurs;
            Messages.Clear();
            foreach (var file in Directory.GetFiles(Properties.Settings.Default.MessagePath))
            {
                var s = new XmlSerializer(typeof(Message_VM));
                var reader = new StreamReader(file);
                var message = (Message_VM)s.Deserialize(reader);
                if (message.Etat.Libelle != "Supprimé")
                {
                    if (message.A.Id == Reference_ViewModel.Header.CurrentUtilisateur.Id && message.Etat.Libelle == "Non lu")
                        message.ColorEtat = "orange";
                    else
                    {
                        message.ColorEtat = "black";
                    }
                    message.SaveColor = "Navy";
                    if (message.De.Id == Reference_ViewModel.Header.CurrentUtilisateur.Id || message.A.Id == Reference_ViewModel.Header.CurrentUtilisateur.Id)
                        Messages.Add(message);
                }

                reader.Close();
            }
            CurrentFilterEtat = null;
        }

        #endregion
    }
}
