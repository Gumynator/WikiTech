using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace APIPrototype.Models
{
    public partial class Abonnement
    {
        public Abonnement()
        {
            AspNetUsers = new HashSet<AspNetUsers>();
        }

        public int IdAbonnement { get; set; }
        public string NomAbonnement { get; set; }
        public decimal PrixAbonnement { get; set; }

        public virtual ICollection<AspNetUsers> AspNetUsers { get; set; }
    }
}
