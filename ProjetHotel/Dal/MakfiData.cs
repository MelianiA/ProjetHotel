using Makrisoft.Makfi.Models;
using Makrisoft.Makfi.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;

namespace Makrisoft.Makfi.Dal
{
    public enum ViewEnum
    {
        Header, Login, Home, Intervention, Chambre, InterventionNew, Employe, Synthese, Administration, Etage, InterventionDetail,
        Utilisateur, None, Hotel, InterventionAjouter, InterventionSupprimer, Message, Parametre
    }
    public enum RoleEnum { None = 0, Gouvernante = 1, Reception = 2, Admin = 255 }
    public enum EntiteEnum
    {
        Employe = 1, Chambre = 2, Intervention = 3, InterventionDetail = 4, Message = 5, InterventionAjouter = 6,
        None = 0
    }
    public partial class CanDelete : Modele
    {
        public string Table { get; set; }
        public Nullable<int> Nombre { get; set; }
    }
    public static class MakfiData
    {
        #region Propriétés
        public static string PasswordAdmin;
        public static string PasswordChange;
        public static List<Etat_VM> Etats;
        public static string Erreur;
        #endregion

        #region Constructeur
        public static string Init(string connectionString)
        {
            ConnectionString = connectionString;

            // Lecture table Info : PasswordAdmin - PasswordChange - Etats
            var infos = new ObservableCollection<Info_VM>(MakfiData.Crud<Info>
                (
                 "Info_Read",
                null,
                e =>
                {
                    e.Id = (Guid)MakfiData.Reader["Id"];
                    e.Cle = MakfiData.Reader["Cle"] as string;
                    e.Valeur = MakfiData.Reader["Valeur"] as string;
                })
                .Select(x => new Info_VM
                {
                    Id = x.Id,
                    Cle = x.Cle,
                    Valeur = x.Valeur
                }));
            PasswordAdmin = infos.Where(i => i.Cle == "PasswordAdmin").Select(i => i.Valeur).FirstOrDefault();
            PasswordChange = infos.Where(i => i.Cle == "PasswordChange").Select(i => i.Valeur).FirstOrDefault();

            if (infos.Count > 0)
            {
                Etats = MakfiData.Crud<Etat>
                    (
                    "Etat_Read",
                    null,
                    e =>
                    {
                        e.Id = (Guid)Reader["Id"];
                        e.Libelle = Reader["Libelle"] as string;
                        e.Icone = Reader["Icone"] as string;
                        e.Couleur = Reader["Couleur"] as string;
                        e.Entite = (EntiteEnum)(byte)Reader["Entite"];
                        e.EtatEtat = (bool)Reader["EtatEtat"];
                    })
                    .Select(x => new Etat_VM
                    {
                        Id = x.Id,
                        Libelle = x.Libelle,
                        Icone = x.Icone,
                        Couleur = x.Couleur,
                        Entite = x.Entite,
                        EtatEtat = x.EtatEtat
                    }).ToList();
            }
            return MakfiData.Erreur;
        }
        #endregion

        #region SqlServer 
        private static string ConnectionString;
        private static SqlCommand Command = null;
        private static SqlConnection Cnx = null;
        public static SqlDataReader Reader = null;
        private static bool ExecuteReader(string spName, string spParam = null)
        {
            Erreur = string.Empty;
            if (Cnx == null)
            {
                Cnx = new SqlConnection(ConnectionString);
                try
                {
                    Cnx.Open();
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null) if (ex.InnerException.InnerException != null) Erreur = ex.InnerException.InnerException.Message; else Erreur = ex.InnerException.Message;
                }
            }
            try
            {
                Command = new SqlCommand
                {
                    Connection = Cnx,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = spName
                };
                if (spParam != null) Command.Parameters.Add(new SqlParameter("data", spParam));
                Reader = Command.ExecuteReader();
                return true;
            }
            catch (Exception ex)
            {
                Erreur = ex.Message;
                if (ex.InnerException != null) if (ex.InnerException.InnerException != null) Erreur = ex.InnerException.InnerException.Message; else Erreur = ex.InnerException.Message;
                return false;
            }
        }
        private static void Close()
        {
            if (Reader != null) Reader.Close();
        }
        #endregion

        #region Queries
        public static List<T> Crud<T>(string spName, string spParam, Action<T> spFields = null) where T : Modele, new()
        {
            if (spFields == null)
            {
                spFields = e =>
                {
                     e.Id = (Guid)MakfiData.Reader["Id"];
                     e.Insere = (bool)MakfiData.Reader["insere"];
                };
            }
            List<T> entities = new List<T>();
            T entite;
            if (ExecuteReader(spName, spParam))
                while (Reader.Read())
                {
                    entite = new T();
                    spFields(entite);
                    entities.Add(entite);
                }
            Close();
            return entities;
        }
        //public static List<T> Save<T>(string spName, string spParam) where T : Modele, new()
        //{
        //    return Read<T>
        //    (
        //    spName,
        //    spParam,
        //    e =>
        //    {
        //        e.Id = (Guid)Reader["Id"];
        //        e.Insere = (bool)Reader["insere"];
        //    }
        //    );
        //}
        //        public static List<T> Delete<T>(string spName, string spParam) */where T : Modele, new()
        //        {
        //            return Read<T>
        //            (
        //            spName,
        //            spParam,
        //            e =>
        //            {
        //                e.Id = (Guid) Reader["Id"];
        //        e.Insere = (bool) Reader["insere"];
        //    }
        //            );
        //            if (Cnx == null)
        //            {
        //                Cnx = new SqlConnection(ConnectionString);
        //                try
        //                {
        //                    Cnx.Open();
        //                }
        //                catch (Exception ex)
        //{
        //    Erreur = ex.Message;
        //    if (ex.InnerException != null) if (ex.InnerException.InnerException != null) Erreur = ex.InnerException.InnerException.Message; else Erreur = ex.InnerException.Message;
        //}
        //            }
        //            Command = new SqlCommand
        //            {
        //                Connection = Cnx,
        //                CommandType = System.Data.CommandType.StoredProcedure,
        //                CommandText = spName
        //            };
        //if (spParam != null) Command.Parameters.Add(new SqlParameter("data", spParam));
        //try
        //{
        //    Command.ExecuteNonQuery();
        //    return true;
        //}
        //catch (Exception ex)
        //{
        //    Erreur = ex.Message;
        //    if (ex.InnerException != null) if (ex.InnerException.InnerException != null) Erreur = ex.InnerException.InnerException.Message; else Erreur = ex.InnerException.Message;
        //    return false;
        //}
        //        }
        public static IEnumerable<CanDelete> CanDelete(string spName, string spParam)
        {
            return Crud<CanDelete>
                (
                spName,
                spParam,
                e =>
                {
                    e.Table = Reader["tableName"].ToString();
                    e.Nombre = Reader["n"] as int?;
                }
                ).Where(x => x.Nombre > 0);
        }

        #endregion
    }
}
