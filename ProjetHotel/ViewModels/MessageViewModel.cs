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



            //Load 
            Utilisateurs = Reference_ViewModel.Header.Utilisateurs;
            Messages = new ObservableCollection<Message_VM>();
            MessagesCollectionView = new ListCollectionView(Messages);
            Load_Etat();
            Load_Message();

        }

        private bool OnCanExecuteSupprimerMessageCommand()
        {
            return CurrentMessage != null;
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

                    var messageToRemove = Messages.Where(m => m.Id == message.Id).SingleOrDefault();
                    if (messageToRemove != null)
                        Messages.Remove(messageToRemove);
                }
            }
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
            set { currentMessage = value; OnPropertyChanged("CurrentMessage"); }
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
        #endregion

        #region Command
        // ICommand
        //public ICommand PersistMessageCommand { get; set; }
        public ICommand AjouterMessage { get; set; }
        public ICommand ActualiserMessage { get; set; }
        public ICommand MessageSaveCommand { get; set; }
        public ICommand SupprimerMessage { get; set; }

        // Méthode OnCommand
        private void OnAjouterMessage()
        {
            CurrentMessage = new Message_VM
            {
                Id = Guid.NewGuid(),
                De = Reference_ViewModel.Header.CurrentUtilisateur,
                DateCreation = DateTime.Now,
                Etat = EtatMessage.Where(x => x.Libelle == "Non lu").Single()
            };
            Messages.Add(CurrentMessage);
        }
        private void OnActualiserMessage()
        {
            Load_Message();
        }
        private void OnMessageSaveCommand()
        {
            var s = new XmlSerializer(typeof(Message_VM));
            var writer = new StreamWriter($@"{Properties.Settings.Default.MessagePath}\{CurrentMessage.Id}.xml");
            s.Serialize(writer, CurrentMessage);
            writer.Close();
            CurrentMessage.SaveColor = "Navy";
        }

        //Méthode on canExcute
        private bool OnCanExecuteMessageSaveCommand()
        {
            return CurrentMessage != null;
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
            EtatMessage = new ObservableCollection<Etat_VM>(
               EtatList.Where(x => x.Entite == EntiteEnum.Message).ToList());
            EtatMessageCollectionView = new ListCollectionView(EtatMessage);
            EtatMessageCollectionView.Refresh();
        }

        public void Load_Message()
        {
            Messages.Clear();
            foreach (var file in Directory.GetFiles(Properties.Settings.Default.MessagePath))
            {
                var s = new XmlSerializer(typeof(Message_VM));
                var reader = new StreamReader(file);
                var message = (Message_VM)s.Deserialize(reader);
                if (message.Etat.Libelle != "Supprimé")
                    Messages.Add(message);
                reader.Close();
            }
        }

        #endregion
    }
}
