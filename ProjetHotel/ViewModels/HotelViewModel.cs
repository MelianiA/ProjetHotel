﻿using Makrisoft.Makfi.Dal;
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
    public class HotelViewModel : ViewModel<Hotel_VM, Hotel>
    {

        #region Constructeur
        public HotelViewModel()
        {
            EtatType = EntiteEnum.None;
            SortDescriptions = new SortDescription[1] { new SortDescription("Nom", ListSortDirection.Ascending) };
            Loads = LoadEnum.Gouvernantes | LoadEnum.Receptions;
            Title = "Les hôtels";

            Init<Hotel>();
        }
        #endregion

        #region DgSource
        public override IEnumerable<Hotel_VM> DgSource_Read()
        {
            return new ObservableCollection<Hotel_VM>(
                MakfiData.Read<Hotel>
                (
                    "Hotel_Read",
                    null,
                    e =>
                    {
                        e.Id = (Guid)MakfiData.Reader["Id"];
                        e.Nom = MakfiData.Reader["Nom"] as string;
                        e.Gouvernante = MakfiData.Reader["Gouvernante"] as Guid?;
                        e.Reception = MakfiData.Reader["Reception"] as Guid?;
                        e.Commentaire = MakfiData.Reader["Commentaire"] as string;
                    }
                ).Select(x => new Hotel_VM
                {
                    Id = x.Id,
                    Nom = x.Nom,
                    Image = $"/Makrisoft.Makfi;component/Assets/hotels/{x.Nom.ToLower()}.png",
                    Gouvernante = Gouvernantes.Where(u => u.Id == x.Gouvernante).FirstOrDefault(),
                    Reception = Receptions.Where(u => u.Id == x.Reception).FirstOrDefault(),
                    Commentaire = x.Commentaire,
                    SaveColor = "Navy"
                }));
        }

        public override void DgSource_Save(string spName, string spParam)
        {
            base.DgSource_Save(
                "Hotel_Save",
                $@"<hotels>
                        <id>{CurrentDgSource.Id}</id>
                        <nom>{CurrentDgSource.Nom}</nom> 
                        <reception>{CurrentDgSource.Reception?.Id}</reception>
                        <gouvernante>{CurrentDgSource.Gouvernante?.Id}</gouvernante>
                        <commentaire>{CurrentDgSource.Commentaire}</commentaire>       
                    </hotels>");
        }

        #endregion

        #region Command
        public override void OnAddCommand()
        {
            CurrentDgSource = new Hotel_VM { Id = null, Nom = "(A définir)" };
            DgSource.Add(CurrentDgSource);
        }
        public override void OnDeleteCommand(string spName, string spParam)
        {
            spName = "Hotel_CanDelete";
            spParam = $"<hotels><id>{CurrentDgSource.Id}</id></hotels>";

            base.OnDeleteCommand(spName, spParam);
        }
        #endregion

    }
}
