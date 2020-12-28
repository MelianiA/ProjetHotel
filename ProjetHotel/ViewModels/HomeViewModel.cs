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
        public string NbrControle
        {
            get { return nbrcontrole; }
            set { nbrcontrole = value; OnPropertyChanged("NbrControle"); }
        }
        private string nbrcontrole;

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
        private String derniereIntervention;

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

            Reference_ViewModel.Main.ViewSelected = (ViewEnum)view;

        }

        // Méthodes OnCanExecuteCommand

        #endregion

        #region Load

        public void ButtonInterventionDuJour()
        {
            Intervention_VM intervention = null;
            if (Reference_ViewModel.Intervention.DgSource != null)
                NbrControle = Reference_ViewModel.Intervention.DgSource.Where(x => x.Etat !=null &&  x.Etat.Libelle != "Terminée").Count().ToString();
            var itervDispo = Reference_ViewModel.Intervention.DgSource.Where(x => x.Etat != null && x.Etat.Libelle != "Terminée").ToList();
            if (itervDispo.Count() > 0)
            {
                intervention = itervDispo[0];
                DerniereIntervention = "Intervention du " + intervention.Date1.ToString("dddd dd MMMM", CultureInfo.CurrentCulture);
                EtatControle = "Contrôle";
            }
            else
            {
                DerniereIntervention = "";
                EtatControle = "Intervention du jour";
            }

            Reference_ViewModel.Intervention.CurrentDgSource = intervention;
            Reference_ViewModel.InterventionDetail.Load_DgSource();
        }

        #endregion

    }
}
