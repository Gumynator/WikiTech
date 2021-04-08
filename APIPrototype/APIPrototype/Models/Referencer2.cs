using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace APIPrototype.Models
{
    public partial class Referencer2
    {
        public int IdTag { get; set; }
        public int IdChangement { get; set; }

        public virtual Changement IdChangementNavigation { get; set; }
        public virtual Tag IdTagNavigation { get; set; }
    }
}
