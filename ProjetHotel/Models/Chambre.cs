﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makrisoft.Makfi.Models
{
    public class Chambre
    {
        public Guid Id;
        public string Nom;
        public Guid Etat;
        public Guid? Etage;
        public string Commentaire;
        public Hotel Hotel;
    }
}
