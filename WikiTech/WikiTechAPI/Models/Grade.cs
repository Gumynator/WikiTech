using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WikiTechAPI.Models
{
    public partial class Grade
    {
        public Grade()
        {
            AspNetUsers = new HashSet<AspNetUsers>();
        }

        public int IdGrade { get; set; }
        public string NomGrade { get; set; }
        public int MinpointGrade { get; set; }

        public virtual ICollection<AspNetUsers> AspNetUsers { get; set; }
    }
}
