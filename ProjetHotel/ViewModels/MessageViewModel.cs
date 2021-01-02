using Makrisoft.Makfi.Dal;
using Makrisoft.Makfi.Models;
using Makrisoft.Makfi.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Serialization;

namespace Makrisoft.Makfi.ViewModels
{
    public class MessageViewModel : ViewModel<Message_VM, Message>
    {

        #region Constructeur
        public MessageViewModel()
        {
            EtatType = EntiteEnum.Message;
            SortDescriptions = new SortDescription[1] { new SortDescription("DateCreation", System.ComponentModel.ListSortDirection.Descending) };
            Loads = LoadEnum.Etats | LoadEnum.Utilisateurs;
            Title = "Les messages";

            Init<Message>();
        }
        #endregion

        #region DgSource
        public override IEnumerable<Message_VM> DgSource_Read()
        {
            var SupprXml = !Reference_ViewModel.Parametre.VoirMsgArchives ? "<exclude>Supprimé</exclude>" : "";
            var messages = MakfiData.Read<Message>
                (
                "Message_Read",
                $"<messages>{SupprXml}<deOuA>{Reference_ViewModel.Header.CurrentUtilisateur.Id}</deOuA></messages>",
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
                  IdHisto = x.IdHisto,
                  De = Utilisateurs.Single(u => u.Id == x.De),
                  A = Utilisateurs.Single(u => u.Id == x.A),
                  DateCreation = x.DateEnvoi,
                  Libelle = x.Libelle,
                  Objet = x.Objet,
                  Etat = MakfiData.Etats.Where(e => e.Id == x.Etat).Single(),
                  SaveColor = "Navy"
              }).ToList();

            // Envoyés deviennent non lus
            foreach (var message in messages)
            {
                if (message.A.Id == Reference_ViewModel.Header.CurrentUtilisateur.Id &&
                    message.Etat == MakfiData.Etats.Where(e => e.Entite == EntiteEnum.Message && e.Libelle == "Envoyé").Single())
                {
                    message.Etat = MakfiData.Etats.Where(e => e.Entite == EntiteEnum.Message && e.Libelle == "Non lu").Single();
                    message.SaveColor = "Red";
                    var spParam = $@"
                    <messages>
                        <id>{message.Id}</id>
                        <de>{message.De?.Id}</de>
                        <a>{message.A?.Id}</a>
                        <envoyeLe>{message.DateCreation}</envoyeLe>
                        <libelle>{message.Libelle}</libelle>
                        <objet>{message.Objet}</objet>
                        <etat>{message.Etat.Id}</etat>
                     </messages>";
                    var ids = MakfiData.Save<Message>("Message_Save", spParam);
                }
            }
            return messages;
        }
        public override void DgSource_Save(string spName, string spParam)
        {
            CurrentDgSource.Etat = MakfiData.Etats.Where(e => e.Entite == EntiteEnum.Message && e.Libelle == "Envoyé").Single();
            CurrentDgSource.SaveColor = "Navy";

            base.DgSource_Save(
                "Message_Save",
                $@"
                    <messages>
                        <id>{CurrentDgSource.Id}</id>
                        <de>{CurrentDgSource.De?.Id}</de>
                        <a>{CurrentDgSource.A?.Id}</a>
                        <envoyeLe>{CurrentDgSource.DateCreation}</envoyeLe>
                        <libelle>{CurrentDgSource.Libelle}</libelle>
                        <objet>{CurrentDgSource.Objet}</objet>
                        <etat>{CurrentDgSource.Etat.Id}</etat>
                     </messages>");
        }
        public override bool DgSource_Filter(object item)
        {
            var message = (Message_VM)item;
            return
                (!FilterHistorique || CurrentDgSource == null || message.IdHisto == CurrentDgSource.Id) &&
                (FilterUtilisateur == null || message.De.Id == FilterUtilisateur.Id || message.A.Id == FilterUtilisateur.Id) &&
                (FilterEtat == null || Etats.Any(e => message.Etat.Id == FilterEtat.Id));
        }

        #endregion

        #region Command
        public override void OnAddCommand()
        {
            CurrentDgSource =
                new Message_VM
                {
                    Id = null,
                    IdHisto = Guid.NewGuid(),
                    De = Reference_ViewModel.Header.CurrentUtilisateur,
                    DateCreation = DateTime.Now,
                    Etat = MakfiData.Etats.Where(e => e.Entite == EntiteEnum.Message && e.Libelle == "Brouillon").Single(),
                    SaveColor = "Red"
                };
            DgSource.Add(CurrentDgSource);
        }
        public override void OnDeleteCommand(string spName, string spParam)
        {
            spName = "Message_CanDelete";
            spParam = $@"
                    <messages>
                        <archive>{CurrentDgSource.Id}</archive>
                     </messages>";

            base.OnDeleteCommand(spName, spParam);
        }
        public override bool OnCanExecuteSaveCommand() { return CurrentDgSource != null && CurrentDgSource.SaveColor == "Red"; }

        #endregion

        public bool FilterHistorique
        {
            get { return filterHistorique; }
            set
            {
                filterHistorique = value;
                OnPropertyChanged("filterHistorique");
            }
        }
        private bool filterHistorique = false;

        public bool HistoEnabled
        {
            get { return histoEnabled; }
            set
            {
                histoEnabled = value;
                OnPropertyChanged("HistoEnabled");
            }
        }
        private bool histoEnabled;

        public override void DgSource_Change()
        {
            if (CurrentDgSource == null) return;
            if (CurrentDgSource.A != null && 
                CurrentDgSource.A.Id == Reference_ViewModel.Header.CurrentUtilisateur.Id && 
                CurrentDgSource.Etat == MakfiData.Etats.Where(e => e.Entite == EntiteEnum.Message && e.Libelle == "Non lu").Single())
            {
                CurrentDgSource.Etat = MakfiData.Etats.Where(e => e.Entite == EntiteEnum.Message && e.Libelle == "Lu").Single();
                CurrentDgSource.SaveColor = "Navy";
                // Save
                var spParam = $@"
                    <messages>
                        <id>{CurrentDgSource.Id}</id>
                        <de>{CurrentDgSource.De?.Id}</de>
                        <a>{CurrentDgSource.A?.Id}</a>
                        <envoyeLe>{CurrentDgSource.DateCreation}</envoyeLe>
                        <libelle>{CurrentDgSource.Libelle}</libelle>
                        <objet>{CurrentDgSource.Objet}</objet>
                        <etat>{CurrentDgSource.Etat.Id}</etat>
                     </messages>";
                var ids = MakfiData.Save<Message>("Message_Save", spParam);

            }
            HistoEnabled = CurrentDgSource != null;
        }

    }
}
