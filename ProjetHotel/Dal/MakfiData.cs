using Makrisoft.Makfi.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makrisoft.Makfi.Dal
{
    public enum ViewEnum
    {
        Header, Login, Home, Intervention, Chambre, InterventionNew, Employe, Synthese, Administration, ChambreGroupe, InterventionDetail,
        Utilisateur, None, Hotel, DecoupageNew, Decoupage
    }
    public static class MakfiData
    {
        #region Constructeur
        private static Action<string, string, string> OnError;
        public static void Init(string connectionString, Action<string, string, string> onError)
        {
            ConnectionString = connectionString;
            OnError = onError;
        }
        #endregion

        #region SqlServer objects
        private static string ConnectionString;

        private static SqlCommand Command = null;
        private static SqlConnection Cnx = null;
        private static SqlDataReader Reader = null;
        #endregion

        #region Execute queries
        private static bool ExecuteReader(string spName, string spParam = null)
        {
            string message = string.Empty;
            if (Cnx == null)
            {
                Cnx = new SqlConnection(ConnectionString);
                try
                {
                    Cnx.Open();
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null) if (ex.InnerException.InnerException != null) message = ex.InnerException.InnerException.Message; else message = ex.InnerException.Message;
                    OnError(message, spName, spParam);
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
                message = ex.Message;
                if (ex.InnerException != null) if (ex.InnerException.InnerException != null) message = ex.InnerException.InnerException.Message; else message = ex.InnerException.Message;
                OnError(message, spName, spParam);
                return false;
            }
        }



        private static void Close()
        {
            if (Reader != null) Reader.Close();
        }
        private static bool ExecuteNonQuery(string spName, string spParam = null)
        {
            string message;

            if (Cnx == null)
            {
                Cnx = new SqlConnection(ConnectionString);
                try
                {
                    Cnx.Open();
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    if (ex.InnerException != null) if (ex.InnerException.InnerException != null) message = ex.InnerException.InnerException.Message; else message = ex.InnerException.Message;
                    OnError(message, spName, spParam);
                }
            }
            Command = new SqlCommand
            {
                Connection = Cnx,
                CommandType = System.Data.CommandType.StoredProcedure,
                CommandText = spName
            };
            if (spParam != null) Command.Parameters.Add(new SqlParameter("data", spParam));
            try
            {
                Command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                if (ex.InnerException != null) if (ex.InnerException.InnerException != null) message = ex.InnerException.InnerException.Message; else message = ex.InnerException.Message;
                OnError(message, spName, spParam);
                return false;
            }
        }
        public static int CanDelete(string spName, string tableName, Guid id)
        {
            int c = 0;
            if (ExecuteReader(spName, $"<{tableName}><id>{id}</id></{tableName}>"))
                while (Reader.Read())
                    c += (int)Reader["n"];
            Close();
            return c;
        }
        public static List<T> ReadAll<T>(string spName, Action<T> p, string spParam = null) where T : new()
        {
            List<T> entities = new List<T>();
            T entite = default(T);
            if (ExecuteReader(spName, spParam))
                while (Reader.Read())
                {
                    entite = new T();
                    p(entite);
                    entities.Add(entite);
                }
            Close();
            return entities;
        }

        #endregion

        #region Toutes les requêtes

        #region Utilisateur
        public static IEnumerable<Utilisateur> Utilisateur_Read(string spParam = null)
        {
            return ReadAll<Utilisateur>
                (
                "Utilisateur_Read",
                e =>
                {
                    e.Id = (Guid)Reader["Id"];
                    e.Nom = Reader["Nom"] as string;
                    e.Image = Reader["Image"] as string;
                    e.CodePin = Reader["CodePin"] as string;
                    e.Statut = (byte)Reader["Statut"];
                },
                spParam
                );
        }

        public static bool SaveUtilisateurPassword(Guid id, string password)
        {
            Cnx = new SqlConnection();
            Cnx.ConnectionString = ConnectionString;

            // à finir quand j'aurai besoin ...
            return true;
        }
        #endregion

        #region Hotel
        internal static IEnumerable<Hotel> Hotel_Read(string spParam = null)
        {
            return ReadAll<Hotel>
                            (
                            "Hotel_Read",
                            e =>
                            {
                                e.Id = (Guid)Reader["Id"];
                                e.Nom = Reader["Nom"] as string;
                                e.Image = Reader["Image"] as string;
                                e.Gouvernante = (Guid)Reader["Gouvernante"];
                            },
                            spParam
                            );
        }
        #endregion


        #endregion

    }
}
