using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace AuthentificationLoginProject.Models
{
    public partial class Section
    {
        public Section()
        {
            Article = new HashSet<Article>();
        }

        public int IdSection { get; set; }
        public string NomSection { get; set; }

        public virtual ICollection<Article> Article { get; set; }
    }
}
