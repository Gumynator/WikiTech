using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikiTechAPI.ViewModels
{
    public class CheckAbonnement
    {
        public DateTime DateFacture { get; set; }
        public int? IdAbonnement { get; set; }
        public string Id { get; set; }

        public string TitreFacture { get; set; }

        public bool Expiration { get; set; }

    }
}
