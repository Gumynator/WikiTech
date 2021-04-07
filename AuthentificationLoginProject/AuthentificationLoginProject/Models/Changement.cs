using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace AuthentificationLoginProject.Models
{
    public partial class Changement
    {
        public Changement()
        {
            Referencer2 = new HashSet<Referencer2>();
        }

        public int IdChangement { get; set; }
        public string Id { get; set; }
        public int IdArticle { get; set; }
        public string TitreChangement { get; set; }
        public string DescriptionChangement { get; set; }
        public string TextChangement { get; set; }
        public string ResumeChangement { get; set; }
        public DateTime? DatepublicationChangement { get; set; }

        public virtual Article IdArticleNavigation { get; set; }
        public virtual AspNetUsers IdNavigation { get; set; }
        public virtual ICollection<Referencer2> Referencer2 { get; set; }
    }
}
