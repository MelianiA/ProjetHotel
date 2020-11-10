using Makrisoft.Makfi.Dal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Makrisoft.Makfi.ViewModels
{
    public class EmployeViewModel : ViewModelBase
    {

        #region Constructeur
        public EmployeViewModel()
        {
            // Icommand


            // ListeView
            Load_Employes();

        }
        #endregion

        #region Binding
        //Employe
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
        public ListCollectionView EmployeCollectionView
        {
            get { return employeCollectionView; }
            set { employeCollectionView = value; OnPropertyChanged("EmployeCollectionView"); }
        }
        private ListCollectionView employeCollectionView;
        public Employe_VM CurrentEmploye
        {
            get
            {

                return currentEmploye;

            }
            set
            {
                currentEmploye = value;
                OnPropertyChanged("CurrentEmploye");
            }
        }
        private Employe_VM currentEmploye;

        //EmployeEtat
        public ObservableCollection<Etat_VM> EmployeEtatList
        {
            get { return employeEtatList; }
            set
            {
                employeEtatList = value;
                OnPropertyChanged("EmployeEtatList");
            }
        }
        private ObservableCollection<Etat_VM> employeEtatList;



        //........


        #endregion

        #region Commands
        //ICommand


        // Méthodes OnCommand


        // Méthodes OnCanExecuteCommand


        #endregion

        #region Load
        private void Load_Employes()
        {
            Employes = new ObservableCollection<Employe_VM>(
              MakfiData.Employe_Read()
              .Select(x => new Employe_VM
              {
                  Id = x.Id,
                  Nom = x.Nom,
                  //Image = x.Image != null ? $"/Makrisoft.Makfi;component/Assets/hotels/{x.Image}" : $"/Makrisoft.Makfi;component/Assets/hotels/hotel.png",
                  Prenom= x.Prenom,
                  Etat = x.Etat.Id,
                  Commentaire = x.Commentaire,
                  SaveColor = "Navy"
              }).OrderBy(x => x.Nom).ToList());;
            EmployeCollectionView = new ListCollectionView(Employes);
            EmployeCollectionView.Refresh();
        }
        #endregion
    }
}
