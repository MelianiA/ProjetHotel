using Makrisoft.Makfi.Dal;
using Makrisoft.Makfi.Tools;
using System;
using System.Windows;

namespace Makrisoft.Makfi
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
 
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MakfiData.Init(
                Makfi.Properties.Settings.Default.MakfiConnectionString,
                BoiteACoucou);
        }
        
        private void BoiteACoucou(string exceptionMsg, string spName, string spParams)
        {
            var simpleMsg = $"{exceptionMsg}{Environment.NewLine}{spParams}";

            // Des mesures empêchent cette suppression
            if (exceptionMsg.Contains("DataMesure") && spName.Contains("Delete"))
                simpleMsg = "Des mesures empêchent cette suppression";

            // FK_... Save
            else if (exceptionMsg.Contains("FK_Entite_EntiteType") && spName.Contains("_Save"))
                simpleMsg = "Le type d'entité doit être spécifié";
            else if (exceptionMsg.Contains("FK_Utilisateur_Entite") && spName.Contains("_Save"))
                simpleMsg = "Une entité doit être spécifié";
            else if (exceptionMsg.Contains("FK_Utilisateur_UtilisateurType") && spName.Contains("_Save"))
                simpleMsg = "Un rôle doit être spécifié";
            else if (exceptionMsg.Contains("FK_Utilisateur_LanguageType") && spName.Contains("_Save"))
                simpleMsg = "Une langue doit être spécifié";
            else if (exceptionMsg.Contains("FK_Cahier_CahierType") && spName.Contains("_Save"))
                simpleMsg = "Un type de cahier doit être spécifié";

            // FK_... Delete
            else if (exceptionMsg.Contains("FK_Utilisateur_Entite") && spName.Contains("_Delete"))
                simpleMsg = "Un utilisateur lié empêche la suppression";

            // NULL dans la colonne
            else if (exceptionMsg.Contains("NULL dans la colonne 'Nom'"))
                simpleMsg = "Le nom doit être spécifié";
            else if (exceptionMsg.Contains("NULL dans la colonne 'Prenom'"))
                simpleMsg = "Le prénom doit être spécifié";
            else if (exceptionMsg.Contains("NULL dans la colonne 'Login'"))
                simpleMsg = "Le matricule doit être spécifié";
            else if (exceptionMsg.Contains("NULL dans la colonne 'Password'"))
                simpleMsg = "Le mot de passe doit être spécifié";
            else if (exceptionMsg.Contains("NULL dans la colonne 'Reference'"))
                simpleMsg = "Une référence doit être spécifiée";
            else if (exceptionMsg.Contains("NULL dans la colonne 'LibelleLong'"))
                simpleMsg = "Une description doit être spécifié";
            else if (exceptionMsg.Contains("NULL dans la colonne 'LibelleCourt'"))
                simpleMsg = "Un libellé doit être spécifié";
            else if (exceptionMsg.Contains("NULL dans la colonne 'Config'"))
                simpleMsg = "Un nom de configuration doit être spécifié";
            else if (exceptionMsg.Contains("NULL dans la colonne 'Unite'"))
                simpleMsg = "Une unité doit être spécifiée";

            Dispatcher.BeginInvoke(new Action(() => System.Windows.MessageBox.Show(simpleMsg, spName)));
            Log.WriteLog($"{spName}{Environment.NewLine}{exceptionMsg}{Environment.NewLine}{spParams}");
        }
    }
}
