using Makrisoft.Makfi.Dal;
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
    public class HotelViewModel : ViewModel<Hotel_VM>
    {

        #region Constructeur
        public HotelViewModel()
        {
            EtatType = EntiteEnum.None;
            SortDescriptions = new SortDescription[1] { new SortDescription("Nom", ListSortDirection.Ascending) };
            Loads = LoadEnum.Gouvernantes | LoadEnum.Receptions;
            Title = "Les hôtels";

            Init();
        }
        #endregion

        #region DgSource
        public override IEnumerable<Hotel_VM> DgSource_Read()
        {
            return new ObservableCollection<Hotel_VM>(
                MakfiData.Hotel_Read()
                .Select(x => new Hotel_VM
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

        //public override void Load(ViewEnum exView)
        //{
        //    Reference_ViewModel.Hotel.Load_Receptions();
        //    Reference_ViewModel.Hotel.Load_Gouvernantes();
        //}

        public override void DgSource_Save()
        {
            var reception = CurrentDgSource.Reception?.Id;
            var gouv = CurrentDgSource.Gouvernante?.Id;
            var param = $@"<hotels>
                                <id>{CurrentDgSource.Id}</id>
                                <nom>{CurrentDgSource.Nom}</nom> 
                                <reception>{reception}</reception>
                                <gouvernante>{gouv}</gouvernante>
                                <commentaire>{CurrentDgSource.Commentaire}</commentaire>       
                            </hotels>";
            var ids = MakfiData.Hotel_Save(param);
            if (ids.Count == 0) throw new Exception("Rien n'a été sauvegardé ! ");
            CurrentDgSource.Id = ids[0].Id;
            CurrentDgSource.SaveColor = "Navy";
        }

        #endregion

        #region Command
        public override void OnAddCommand()
        {
            CurrentDgSource = new Hotel_VM { Id = null, Nom = "(A définir)" };
            DgSource.Add(CurrentDgSource);
        }
        public override void OnDeleteCommand()
        {
            var canDeletes = MakfiData.Hotel_CanDelete($"<hotels><id>{CurrentDgSource.Id}</id></hotels>");
            if (canDeletes.Count() == 0)
            {
                var param = MakfiData.Hotel_Delete($"<hotels><id>{CurrentDgSource.Id}</id></hotels>");
                if (param)
                {
                    DgSource.Remove(CurrentDgSource);
                }
            }
            else
            {
                MessageBox.Show($" Suppression impossible de l'hôtel : {CurrentDgSource.Nom }", "Hotel_CanDelete");
            }
        }
        #endregion

    }
}
