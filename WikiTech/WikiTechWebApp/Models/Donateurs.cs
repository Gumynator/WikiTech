////////////////////////////////////////////////////////////////////////////////////////////////////////
//Auteur : Pancini Marco
//Création : 25.04.2021
//Modification : 25.05.2021
//Description : Modelview pour pour envoyer et afficher les informations voulu pour les meilleures donateurs
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikiTechWebApp.Models
{
    public class Donateurs
    {
        public string NomAspnetuser { get; set; }
        public decimal MontantDon { get; set; }
        public DateTime DateDon { get; set; }
        public string MessageDon { get; set; }

    }
}
