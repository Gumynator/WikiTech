////////////////////////////////////////////////////////////////////////////////////////////////////////
//Auteur : Pancini Marco
//Création : 11.04.2021
//Modification : 10.05.2021
//Description : Model pour le formulaire de dons
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikiTechWebApp.Models
{
    public class DonsModelView
    {
        public string Token { get; set; }
        public decimal Total { get; set; }
    }
}
