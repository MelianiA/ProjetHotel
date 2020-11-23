using Makrisoft.Makfi.Dal;
using Makrisoft.Makfi.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Markup;

namespace Makrisoft.Makfi
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitBD();
            Reference_ViewModel.Main = new MainViewModel();
            DataContext = Reference_ViewModel.Main;
            InitializeComponent();
        }

        private bool EtatIsEmpty()
        {
            var EtatList = new ObservableCollection<Etat_VM>(
              MakfiData.Etat_Read()
              .Select(x => new Etat_VM
              {
                  Id = x.Id,
                  Libelle = x.Libelle,
                  Icone = x.Icone,
                  Couleur = x.Couleur,
                  Entite = x.Entite
              }).ToList()); ;
            return EtatList.Count == 0;
        }
        private bool RemplirEtat()
        {
            var Etats = new List<Etat_VM>(){
                new Etat_VM {Libelle="Aucune information !  ",Icone="TimelineHelp",Couleur="gray",Entite=EntiteEnum.Intervention },
                new Etat_VM { Libelle = "Retardée", Icone = "TimelineHelp", Couleur = "orange", Entite = EntiteEnum.Intervention},
                new Etat_VM { Libelle = "Pas encore fait", Icone = "TableLock", Couleur = "red", Entite = EntiteEnum.Chambre},
                new Etat_VM { Libelle = "Disponible", Icone = "FaceWomanShimmer", Couleur = "green", Entite = EntiteEnum.Employe},
                new Etat_VM { Libelle = "Fait", Icone = "TimelineHelp", Couleur = "green", Entite = EntiteEnum.Intervention},
                new Etat_VM { Libelle = "Fait", Icone = "TableLock", Couleur = "green", Entite = EntiteEnum.Chambre},
                new Etat_VM { Libelle = "Pas encore fait", Icone = "TimelineHelp", Couleur = "red", Entite = EntiteEnum.Intervention},
                new Etat_VM { Libelle = "Arrêt maladie", Icone = "FaceWomanShimmer", Couleur = "red", Entite = EntiteEnum.Employe},
                new Etat_VM { Libelle = "Non disponible", Icone = "FaceWomanShimmer", Couleur = "black", Entite = EntiteEnum.Employe},
            };

            if (EtatIsEmpty())
            {
                foreach (var item in Etats)
                {
                    var param = $@"
                    <etat>
                        <libelle>{item.Libelle}</libelle>
                        <icone>{item.Icone}</icone>
                        <couleur>{item.Couleur}</couleur>
                        <entite>{(byte)item.Entite}</entite>    
                      </etat>";
                    var ids = MakfiData.Etat_Save(param);
                    if (ids.Count == 0) throw new Exception("Rien n'a été sauvgardé ! ");
                }
                MessageBox.Show("Insertions dans la table Etat .. .. .  100% !! ", " Etat");
                return true;
            }
            else { return false; }

        }
    
        private void InitBD()
        {
            RemplirEtat();

        }
    }
}
