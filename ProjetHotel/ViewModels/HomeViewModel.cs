using Makrisoft.Makfi.Dal;
using Makrisoft.Makfi.Tools;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Input;

namespace Makrisoft.Makfi.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        #region Constructeur
        public HomeViewModel()
        {
            ChangeViewCommand = new RelayCommand(p => OnChangeViewCommand(p));
        }
        #endregion

        #region Binding
        public bool IsAdmin
        {
            get { return isAdmin; }
            set
            {
                isAdmin = value;
                OnPropertyChanged("IsAdmin");
            }
        }
        private bool isAdmin = true;

        public bool WithHotel
        {
            get { return withHotel; }
            set
            {
                withHotel = value;
                OnPropertyChanged("WithHotel");
            }
        }
        private bool withHotel;

        //Nbr de controle 
        public string NIntervNonTermine
        {
            get { return nIntervNonTermine; }
            set { nIntervNonTermine = value; OnPropertyChanged("NIntervNonTermine"); }
        }
        private string nIntervNonTermine;

        public string EtatControle
        {
            get { return etatControle; }
            set { etatControle = value; OnPropertyChanged("EtatControle"); }
        }
        private string etatControle;


        //Dernière intervention
        public String DerniereIntervention
        {
            get { return derniereIntervention; }
            set { derniereIntervention = value; OnPropertyChanged("DerniereIntervention"); }
        }

        internal void Load(ViewEnum exView)
        {
            DerniereIntervention = "";
            EtatControle = "Intervention du jour";

            var interventions = MakfiData.Intervention_Read($"<interventions><hotel>{Reference_ViewModel.Header.CurrentHotel.Id}</hotel></interventions>")
                .Select(x => new Intervention_VM
                {
                    Id = x.Id,
                    Libelle = x.Libelle,
                    Etat = MakfiData.Etats.Where(e => e.Id == x.Etat).Single(),
                    Date1 = x.Date1,
                    Commentaire = x.Commentaire,
                    Model = x.Model,
                    SaveColor = "Navy"
                })
                .Where(x => x.Etat != null && x.Etat.Libelle != "Terminée").ToList();

            NIntervNonTermine = interventions.Count().ToString();
            if (interventions.Count() > 0)
            {
                LastIntervention = interventions[0];
                DerniereIntervention = "Intervention du " + interventions[0].Date1.ToString("dddd dd MMMM", CultureInfo.CurrentCulture);
                EtatControle = "Contrôle";
            }
        }

        private String derniereIntervention;
        private Intervention_VM LastIntervention;
        #endregion

        #region ICommand

        //ICommand
        public ICommand ChangeViewCommand { get; set; }
        // Méthodes OnCommand
        private void OnChangeViewCommand(object view)
        {
            if ((ViewEnum)view == ViewEnum.InterventionDetail && EtatControle == "Intervention du jour")
            {
                Reference_ViewModel.Intervention.OnAddCommand();
                Reference_ViewModel.Intervention.OnSaveCommand();
            }
            else
            {
                Reference_ViewModel.Intervention.CurrentDgSource =  LastIntervention;

            }
            Reference_ViewModel.Main.ViewSelected = (ViewEnum)view;

        }

        // Méthodes OnCanExecuteCommand

        #endregion

        #region Load

        #endregion

    }
}
