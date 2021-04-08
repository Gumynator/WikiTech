using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace APIPrototype.Models
{
    public partial class Facture
    {
        public int IdFacture { get; set; }
        public string Id { get; set; }
        public decimal MontantFacture { get; set; }
        public DateTime DateFacture { get; set; }
        public string TitreFacture { get; set; }

        public virtual AspNetUsers IdNavigation { get; set; }
    }
}
