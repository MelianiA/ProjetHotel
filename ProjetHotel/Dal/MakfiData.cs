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
        Header, Login, Home, Intervention, Chambre, InterventionNew, Employe, Synthese, Administration, ChambreGroupe, InterventionDetail,
        Utilisateur, None, Hotel,InterventionAjouter, InterventionSupprimer
    }
    public enum RoleEnum { None = 0, Gouvernante = 1, Reception = 2, Admin = 255 }
    public enum EntiteEnum { Employe = 1, Chambre = 2, Intervention = 3 }

    public static class MakfiData
    {
        public static string PasswordAdmin;
        public static string PasswordChange;
        #region Constructeur
        private static Action<string, string, string> OnError;
        public static string Init(string connectionString, Action<string, string, string> onError)
        {
            ConnectionString = connectionString;
            OnError = onError;

            //Premier accès
            var infoList = new ObservableCollection<Info_VM>(
             MakfiData.Info_Read()
             .Select(x => new Info_VM
             {
                 Id = x.Id,
                 Cle = x.Cle,
                 Valeur = x.Valeur
             }).ToList());
            var etat = infoList.Where(i => i.Cle == "Etat").Select(i => i.Valeur).FirstOrDefault();
            PasswordAdmin = infoList.Where(i => i.Cle == "PasswordAdmin").Select(i => i.Valeur).FirstOrDefault();
            PasswordChange = infoList.Where(i => i.Cle == "PasswordChange").Select(i => i.Valeur).FirstOrDefault();
            if (etat == "0")
            {
                if (PremierAcces())
                {
                    try
                    {
                        Info_Save("<info><cle>Etat</cle><valeur>1</valeur></info>");

                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                }
            }
            return "";
        }



        private static bool PremierAcces()
        {
            // Ajoût admin
            Utilisateur_Save("<utilisateur><nom>Admin</nom><codePin>#69!</codePin><statut>255</statut></utilisateur>");
            // Table Etat
            Etat_Save(@"
                <etats>
                         <etat><libelle>Fait</libelle>                  <icone>TimelineHelp</icone>             <couleur>green</couleur>            <entite>3</entite> </etat>
                         <etat> <libelle>En cours</libelle>             <icone>TableLock</icone>                <couleur>orange</couleur>          <entite>3</entite> </etat>
                         <etat><libelle>None</libelle>                  <icone>TimelineHelp</icone>             <couleur>gray</couleur>           <entite>3</entite> </etat>
                         <etat> <libelle>Incident</libelle>             <icone>TimelineHelp</icone>             <couleur>red</couleur>             <entite>3</entite> </etat>
                         <etat><libelle>Disponible</libelle>            <icone>FaceWomanShimmer</icone>         <couleur>green</couleur>           <entite>1</entite> </etat>
                         <etat><libelle>Arrêt maladie</libelle>         <icone>FaceWomanShimmer</icone>         <couleur>red</couleur>             <entite>1</entite> </etat>
                         <etat><libelle>Non disponible</libelle>        <icone>FaceWomanShimmer</icone>         <couleur>black</couleur>           <entite>1</entite> </etat>
                         <etat><libelle>Fait</libelle>                  <icone>TableLock</icone>                <couleur>green</couleur>           <entite>2</entite> </etat>
                         <etat><libelle>Pas encore fait</libelle>       <icone>TableLock</icone>                <couleur>Red</couleur>             <entite>2</entite> </etat>
                </etats>
            ");
            return true;
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
                                // e.Etat = new Etat { Id = (Guid)Reader["Etat"] };
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


        internal static IEnumerable<Intervention> Intervention_Read(string spParam)
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


        private static IEnumerable<Info> Info_Read(string spParam = null)
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
                                         e.Employe = (Guid)Reader["EmployeAffecte"];
                                         e.Chambre = (Guid)Reader["ChambreAffectee"];
                                         e.Etat = (Guid)Reader["Etat"];
                                         e.Commentaire= Reader["Commentaire"] as string;
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
