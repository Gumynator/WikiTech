using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WikiTechAPI.Models
{
    public partial class Genre
    {
        public Genre()
        {
            AspNetUsers = new HashSet<AspNetUsers>();
        }

        public int IdGenre { get; set; }
        public string NomGenre { get; set; }

        public virtual ICollection<AspNetUsers> AspNetUsers { get; set; }
    }
}
