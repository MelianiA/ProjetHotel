﻿using Makrisoft.Makfi.Models;
using Makrisoft.Makfi.ViewModels;
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
    public enum RoleEnum { None = 0, Admin = 1, Gouvernante = 2, Reception = 4 }
    public enum EntiteEnum { Employe = 1, Chambre = 2, Intervention = 3 }

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



        public static List<CanDelete> CanDelete(string spName, string spParam)
        {
            var list = ReadAll<CanDelete>
                (
                spName,
                e =>
                {
                    e.Table = Reader["tableName"].ToString();
                    e.Nombre = Reader["n"] as int?;
                },
                spParam
                );
            return list;
        }

        #endregion

        #region Toutes les requêtes

        #region _Read
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
                                e.Gouvernante =Reader["Gouvernante"] as Guid?;
                                e.Reception =  Reader["Reception"] as Guid?;
                                e.Commentaire = Reader["Commentaire"] as string;
                            },
                            spParam
                            );
        }



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
                    e.Statut = (RoleEnum)(byte)Reader["Statut"];
                },
                spParam
                );
        }
        internal static IEnumerable<Employe> Employe_Read(string spParam = null)
        {
            return ReadAll<Employe>
                            (
                            "Employe_Read",
                            e =>
                            {
                                e.Id = (Guid)Reader["Id"];
                                e.Nom = Reader["Nom"] as string;
                                e.Prenom = Reader["Prenom"] as string;
                                e.Commentaire = Reader["Commentaire"] as string;
                                e.Etat = new Etat { Id = (Guid)Reader["Etat"] };
                            },
                            spParam
                            );
        }
 
        internal static IEnumerable<Chambre> Chambre_Read(string spParam = null)
        {
            return ReadAll<Chambre>
                             (
                             "Chambre_Read",
                             e =>
                             {
                                 e.Id = (Guid)Reader["Id"];
                                 e.Nom = Reader["Nom"] as string;
                                 e.Etat = new Etat { Id = (Guid)Reader["Etat"] };
                                 e.Commentaire = Reader["Commentaire"] as string;
                             },
                             spParam
                             );
        }

        internal static IEnumerable<Etat> Etat_Read(string spParam = null)
        {
            return ReadAll<Etat>
                             (
                             "Etat_Read",
                             e =>
                             {
                                 e.Id = (Guid)Reader["Id"];
                                 e.Libelle = Reader["Libelle"] as string;
                                 e.Icone = Reader["Icone"] as string;
                                 e.Couleur = Reader["Couleur"] as string;
                                 e.Entite = (EntiteEnum)(byte)Reader["Entite"];
                             },
                             spParam
                             );
        }



        internal static IEnumerable<HotelEmploye> HotelEmploye_Read(string spParam = null)
        {
            return ReadAll<HotelEmploye>
                             (
                             "HotelEmploye_Read",
                             e =>
                             {
                                 e.Employe = (Guid)Reader["Employe"];
                             },
                             spParam
                             );
        }



        internal static IEnumerable<ChambreGroupeChambre> ChambreGroupeChambre_Read(string spParam = null)
        {
            return ReadAll<ChambreGroupeChambre>
                          (
                          "ChambreGroupeChambre_Read",
                          e =>
                          {
                              e.Id = (Guid)Reader["Id"];
                              e.Nom = Reader["Nom"] as string;
                              e.Etat = (Guid)Reader["Etat"];
                              e.Commentaire = Reader["Commentaire"] as string;
                              e.GroupeChambre = Reader["GroupeChambre"] as string;

                          },
                          spParam
                          );
        }
        internal static IEnumerable<GroupeChambre> GroupeChambre_Read(string spParam = null)
        {
            return ReadAll<GroupeChambre>
                         (
                         "GroupeChambre_Read",
                         e =>
                         {
                             e.Id = (Guid)Reader["Id"];
                             e.Nom = Reader["Nom"] as string;
                             e.Commentaire = Reader["Commentaire"] as string;
                         },
                         spParam
                         );
        }

        internal static IEnumerable<ChambreByGroupe> ChambreByGroupe_Read(string spParam = null)
        {
            return ReadAll<ChambreByGroupe>
                          (
                          "ChambreByGroupe_Read",
                          e =>
                          {
                              e.IdDelaChambre = (Guid)Reader["IdDelaChambre"];
                              e.GroupeChambre = Reader["GroupeChambre"] as Guid?;
                              e.Nom = Reader["Nom"] as string;
                              e.NomChambre = Reader["NomChambre"] as string;
                          },
                          spParam
                          );
        }


        internal static IEnumerable<Intervention> Interventions_Read(string spParam)
        {
            return ReadAll<Intervention>
                                    (
                                    "Intervention_Read",
                                    e =>
                                    {
                                        e.Id = (Guid)Reader["Id"];
                                        e.Libelle = Reader["Libelle"] as string;
                                        e.Etat =  Reader["Etat"] as Guid?;
                                        e.Date1=  (DateTime)Reader["Date1"];
                                        e.Commentaire = Reader["Commentaire"] as string;
                                        e.GroupeChambre = Reader["GroupeChambre"] as Guid?;
                                    },
                                    spParam
                                    );
        }


        #endregion

        #region _Save
        public static bool Utilisateur_Save(string spParam = null)
        {
            return ExecuteNonQuery("Utilisateur_Save", spParam);
        }
        internal static List<Hotel> Hotel_Save(string spParam)
        {
            return ReadAll<Hotel>
               (
               "Hotel_Save",
               e =>
               {
                   e.Id = (Guid)Reader["Id"];
               },
               spParam
               );

        }
        internal static List<Employe> Employe_Save(string spParam)
        {
            return ReadAll<Employe>
               (
               "Employe_Save",
               e =>
               {
                   e.Id = (Guid)Reader["Id"];
               },
               spParam
               );

        }


        internal static List<Chambre> Chambre_Save(string spParam)
        {
            return ReadAll<Chambre>
              (
              "Chambre_Save",
              e =>
              {
                  e.Id = (Guid)Reader["Id"];
              },
              spParam
              );
        }

        internal static List<HotelEmploye> HotelEmploye_Save(string spParam)
        {
            return ReadAll<HotelEmploye>
               (
               "HotelEmploye_Save",
               e =>
               {
                   e.Id = (Guid)Reader["Id"];
               },
               spParam
               );
        }

        internal static List<GroupeChambre> GroupeChambre_Save(string spParam)
        {
            return ReadAll<GroupeChambre>
              (
              "GroupeChambre_Save",
              e =>
              {
                  e.Id = (Guid)Reader["Id"];
              },
              spParam
              );
        }
        internal static List<GroupeChambre> ChambreGroupeChambre_Save(string spParam)
        {
            return ReadAll<GroupeChambre>
             (
             "ChambreGroupeChambre_Save",
             e =>
             {
                 e.Id = (Guid)Reader["Id"];
             },
             spParam
             );
        }
        internal static List<Intervention> Intervention_Save(string spParam)
        {
            {
                return ReadAll<Intervention>
                 (
                 "Intervention_Save",
                 e =>
                 {
                     e.Id = (Guid)Reader["Id"];
                 },
                 spParam
                 );
            }
        }

        #endregion

        #region _CanDelete
        public static IEnumerable<CanDelete> Utilisateur_CanDelete(string spParam = null)
        {
            return CanDelete("Utilisateur_CanDelete", spParam)
                .Where(x => x.Nombre > 0);
        }

        public static IEnumerable<CanDelete> Hotel_CanDelete(string spParam)
        {
            return CanDelete("Hotel_CanDelete", spParam)
               .Where(x => x.Nombre > 0);
        }

        public static IEnumerable<CanDelete> Employe_CanDelete(string spParam)
        {
            return CanDelete("Employe_CanDelete", spParam)
                .Where(x => x.Nombre > 0);
        }

        public static IEnumerable<CanDelete> Chambre_CanDelete(string spParam)
        {
            return CanDelete("Chambre_CanDelete", spParam)
                 .Where(x => x.Nombre > 0);
        }


        public static IEnumerable<CanDelete> GroupeChambre_CanDelete(string spParam)
        {
            return CanDelete("GroupeChambre_CanDelete", spParam)
                 .Where(x => x.Nombre > 0);
        }
        public static IEnumerable<CanDelete> Intervention_CanDelete(string spParam)
        {
            return CanDelete("Intervention_CanDelete", spParam)
                .Where(x => x.Nombre > 0);
        }

        #endregion

        #region _Delete
        internal static bool Utilisateur_Delete(string spParam = null)
        {
            return ExecuteNonQuery("Utilisateur_Delete", spParam);

        }
        internal static bool Hotel_Delete(string spParam)
        {
            return ExecuteNonQuery("Hotel_Delete", spParam);
        }
        internal static bool Employe_Delete(string spParam)
        {
            return ExecuteNonQuery("Employe_Delete", spParam);
        }
        internal static bool Chambre_Delete(string spParam)
        {
            return ExecuteNonQuery("Chambre_Delete", spParam);
        }

        internal static bool HotelEmploye_Delete(string spParam)
        {
            return ExecuteNonQuery("HotelEmploye_Delete", spParam);
        }
        internal static bool ChambreGroupeChambre_Delete(string spParam)
        {
            return ExecuteNonQuery("ChambreGroupeChambre_Delete", spParam);

        }

        internal static bool GroupeChambre_Delete(string spParam)
        {
            return ExecuteNonQuery("GroupeChambre_Delete", spParam);
        }

        internal static bool Intervention_Delete(string spParam)
        {
            return ExecuteNonQuery("Intervention_Delete", spParam);
        }

        #endregion

        #endregion
    }
    public partial class CanDelete
    {
        public string Table { get; set; }
        public Nullable<int> Nombre { get; set; }
    }
}
