using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetHotel.Models
{
    public class Utilisateur : Modele<Utilisateur>
    {
        public Utilisateur() { }
        public Utilisateur(bool roleAdminFilter, bool roleGouvFilter, bool roleReceptionFilter)
        {
            //CreateNew(Data.ViewEnum.None);
            Role = roleAdminFilter ? RoleEnum.Admin : 0;
            Role |= roleGouvFilter ? RoleEnum.Gouvernante : 0;
            Role |= roleReceptionFilter ? RoleEnum.Reception : 0;
        }
        public string Nom { get; set; }
        public string Password { get; internal set; }
        public RoleEnum Role { get; set; } = RoleEnum.None;
        public List<Hotel> HotelList { get; set; }
        public Hotel HotelSelected { get; set; }
        public List<Message> MessageList { get; set; }
        public bool IsAdmin
        {
            get
            {
                return (Role | RoleEnum.Admin) == Role;
            }
        }
        internal void Restore()
        {
            Id = Origin.Id;
            Nom = Origin.Nom;
            Commentaire = Origin.Commentaire;
        }
        internal void SetModified()
        {
            if (Id == default || Nom != Origin.Nom)
            {
                DateModified = DateTime.Now;
                DateModifiedIcone = "Update";
            }
            else
            {
                DateModified = default;
                DateModifiedIcone = "None";
            }
        }

        public override Utilisateur Clone()
        {
            return new Utilisateur
            {
                Id = this.Id,
                Nom = this.Nom,
                Commentaire = this.Commentaire
            };
        }
    }
    public enum RoleEnum { None = 0, Admin = 1, Gouvernante = 2, Reception = 4 }
}
