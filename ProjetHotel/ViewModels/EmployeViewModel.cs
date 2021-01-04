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
    public class EmployeViewModel : ViewModel<Employe_VM, Employe>
    {
        #region Constructeur
        public EmployeViewModel()
        {
            EtatType = EntiteEnum.Employe;
            SortDescriptions = new SortDescription[1] { new SortDescription("Nom", System.ComponentModel.ListSortDirection.Ascending) };
            Loads = LoadEnum.Etats;
            Title = "Les employés";

            Init<Employe>();
        }
        #endregion

        #region DgSource
        public override IEnumerable<Employe_VM> DgSource_Read()
        {
            return MakfiData.Crud<Employe>
                (
                    "Employe_Read",
                    $"<employes><hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel></employes>",
                    e =>
                    {
                        e.Id = (Guid)MakfiData.Reader["Id"];
                        e.Nom = MakfiData.Reader["Nom"] as string;
                        e.Prenom = MakfiData.Reader["Prenom"] as string;
                        e.Commentaire = MakfiData.Reader["Commentaire"] as string;
                        e.Etat = (Guid)MakfiData.Reader["Etat"];
                    }
                ).Select(x => new Employe_VM
                    {
                        Id = x.Id,
                        Nom = x.Nom,
                        Prenom = x.Prenom,
                        Etat = MakfiData.Etats.Where(e => e.Id == x.Etat).Single(),
                        Commentaire = x.Commentaire,
                        SaveColor = "Navy"
                    });
        }

        public override void DgSource_Save(string spName, string spParam)
        {
            base.DgSource_Save(
                "Employe_Save",
                $@"
                <employes><employe>
                    <id>{CurrentDgSource.Id}</id>
                    <nom>{CurrentDgSource.Nom}</nom>
                    <hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel>
                    <prenom>{CurrentDgSource.Prenom}</prenom>
                    <etat>{CurrentDgSource.Etat.Id}</etat>
                    <commentaire>{CurrentDgSource.Commentaire}</commentaire>       
                </employe></employes>");
        }
        public override bool DgSource_Filter(object item)
        {
            var employe = (Employe_VM)item;
            return (FilterEtat == null || Etats.Any(e => employe.Etat.Id == FilterEtat.Id));
        }

        #endregion

        #region Command
        public override void OnAddCommand()
        {
            CurrentDgSource = new Employe_VM
            {
                Id = null,
                Nom = "(nom)",
                Prenom = "(prénom)",
                Etat = MakfiData.Etats.Where(e => e.Entite == EntiteEnum.Employe && e.Libelle == "Disponible").Single()
            };
            DgSource.Add(CurrentDgSource);
        }
        public override void OnDeleteCommand(string spName, string spParam)
        {
           base.OnDeleteCommand(
                "Employe_Delete",
                $"<employes><employe><id>{CurrentDgSource.Id}</id></employe></employes>");
        }

        #endregion
    }
}
