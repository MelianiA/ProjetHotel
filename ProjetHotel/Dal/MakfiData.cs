using Makrisoft.Makfi.Models;
using Makrisoft.Makfi.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public static class MakfiData
    {
        public static string PasswordAdmin;
        public static string PasswordChange;
        public static List<Etat_VM> Etats;

        #region Constructeur
        private static Action<string, string, string> OnError;
        public static string Init(string connectionString, Action<string, string, string> onError)
        {
            ConnectionString = connectionString;
            OnError = onError;
            var infoList = new ObservableCollection<Info_VM>(
             MakfiData.Info_Read()
             .Select(x => new Info_VM
             {
                 Id = x.Id,
                 Cle = x.Cle,
                 Valeur = x.Valeur
             }).ToList());
            PasswordAdmin = infoList.Where(i => i.Cle == "PasswordAdmin").Select(i => i.Valeur).FirstOrDefault();
            PasswordChange = infoList.Where(i => i.Cle == "PasswordChange").Select(i => i.Valeur).FirstOrDefault();
            if (infoList.Count == 0) return "probleme";

            Etats = MakfiData.Etat_Read()
                  .Select(x => new Etat_VM
                  {
                      Id = x.Id,
                      Libelle = x.Libelle,
                      Icone = x.Icone,
                      Couleur = x.Couleur,
                      Entite = x.Entite,
                      EtatEtat = x.EtatEtat
                  }).ToList();

            return "";
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
            T entite;
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
                                e.Gouvernante = Reader["Gouvernante"] as Guid?;
                                e.Reception = Reader["Reception"] as Guid?;
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
                                e.Etat = (Guid)Reader["Etat"];
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
                                 e.Etat = (Guid)Reader["Etat"];
                                 e.Etage = Reader["GroupeChambre"] as Guid?;
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
                                 e.EtatEtat = (bool)Reader["EtatEtat"];
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



        internal static IEnumerable<ChambreEtage> ChambreGroupeChambre_Read(string spParam = null)
        {
            return ReadAll<ChambreEtage>
                          (
                          "ChambreGroupeChambre_Read",
                          e =>
                          {
                              e.Id = (Guid)Reader["Id"];
                              e.Nom = Reader["Nom"] as string;
                              e.Etat = (Guid)Reader["Etat"];
                              e.Commentaire = Reader["Commentaire"] as string;
                              e.Etage = (Guid)Reader["GroupeChambre"];

                          },
                          spParam
                          );
        }
        internal static IEnumerable<Etage> Etage_Read(string spParam = null)
        {
            return ReadAll<Etage>
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

        internal static IEnumerable<ChambreEtage> ChambreByGroupe_Read(string spParam = null)
        {
            return ReadAll<ChambreEtage>
                          (
                          "ChambreByGroupe_Read",
                          e =>
                          {
                              e.Id = (Guid)Reader["IdDelaChambre"];
                              e.Etage = Reader["GroupeChambre"] as Guid?;
                              e.Nom = Reader["Nom"] as string;
                              e.Nom = Reader["NomChambre"] as string;
                          },
                          spParam
                          );
        }


        internal static IEnumerable<Intervention> Intervention_Read(string spParam=null)
        {
            return ReadAll<Intervention>
                                    (
                                    "Intervention_Read",
                                    e =>
                                    {
                                        e.Id = (Guid)Reader["Id"];
                                        e.Libelle = Reader["Libelle"] as string;
                                        e.Etat = (Guid)Reader["Etat"];
                                        e.Date1 = (DateTime)Reader["Date1"];
                                        e.Commentaire = Reader["Commentaire"] as string;
                                        e.Model = (bool)Reader["Model"];
                                    },
                                    spParam
                                    );
        }


        public static IEnumerable<Info> Info_Read(string spParam = null)
        {
            return ReadAll<Info>
                                    (
                                    "Info_Read",
                                    e =>
                                    {
                                        e.Id = (Guid)Reader["Id"];
                                        e.Cle = Reader["Cle"] as string;
                                        e.Valeur = Reader["Valeur"] as string;
                                    },
                                    spParam
                                    );
        }


        internal static IEnumerable<InterventionDetail> InterventionDetail_Read(string spParam)
        {
            return ReadAll<InterventionDetail>
                                     (
                                     "InterventionDetail_Read",
                                     e =>
                                     {
                                         e.Id = (Guid)Reader["Id"];
                                         e.Employe = new Employe { Id = (Guid)Reader["EmployeAffecte"], Nom = Reader["EmployeNom"] as string, Prenom = Reader["EmployePrenom"] as string };
                                         e.Chambre = new Chambre { Id = (Guid)Reader["ChambreAffectee"], Nom = Reader["ChambreNom"] as string };
                                         e.Etat = (Guid)Reader["Etat"];
                                         e.Commentaire = Reader["Commentaire"] as string;
                                     },
                                     spParam
                                     );
        }


        #endregion

        #region _Save
        internal static List<Utilisateur> Utilisateur_Save(string spParam = null)
        {
            return ReadAll<Utilisateur>
              (
              "Utilisateur_Save",
              e =>
              {
                  e.Id = (Guid)Reader["Id"];
              },
              spParam
              );
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

        internal static List<Etage> Etage_Save(string spParam)
        {
            return ReadAll<Etage>
              (
              "GroupeChambre_Save",
              e =>
              {
                  e.Id = (Guid)Reader["Id"];
              },
              spParam
              );
        }
        internal static List<Etage> ChambreGroupeChambre_Save(string spParam)
        {
            return ReadAll<Etage>
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
        internal static List<Etat> Etat_Save(string spParam)
        {
            {
                return ReadAll<Etat>
                 (
                 "Etat_Save",
                 e =>
                 {
                     e.Id = (Guid)Reader["Id"];
                 },
                 spParam
                 );
            }
        }
        internal static List<Info> Info_Save(string spParam)
        {
            return ReadAll<Info>
             (
             "Info_Save",
             e =>
             {
                 e.Id = (Guid)Reader["Id"];
             },
             spParam
             );
        }

        internal static List<InterventionDetail> InterventionDetail_Save(string spParam)
        {
            return ReadAll<InterventionDetail>
            (
            "InterventionDetail_Save",
            e =>
            {
                e.Id = (Guid)Reader["Id"];
                e.Insere = (bool) Reader["insere"];
            },
            spParam
            );
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


        public static IEnumerable<CanDelete> Etage_CanDelete(string spParam)
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

        internal static bool Etage_Delete(string spParam)
        {
            return ExecuteNonQuery("GroupeChambre_Delete", spParam);
        }

        internal static bool Intervention_Delete(string spParam)
        {
            return ExecuteNonQuery("Intervention_Delete", spParam);
        }

        internal static bool InterventionDetails_Delete(string spParam)
        {
            return ExecuteNonQuery("InterventionDetails_Delete", spParam);
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
