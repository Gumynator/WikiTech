using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace APIPrototype.Models
{
    public partial class Tag
    {
        public Tag()
        {
            Referencer = new HashSet<Referencer>();
            Referencer2 = new HashSet<Referencer2>();
        }

        public int IdTag { get; set; }
        public string NomTag { get; set; }

        public virtual ICollection<Referencer> Referencer { get; set; }
        public virtual ICollection<Referencer2> Referencer2 { get; set; }
    }
}
