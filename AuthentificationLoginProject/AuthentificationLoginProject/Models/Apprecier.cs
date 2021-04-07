using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace AuthentificationLoginProject.Models
{
    public partial class Apprecier
    {
        public string Id { get; set; }
        public int IdArticle { get; set; }
        public bool? Apprecie { get; set; }

        public virtual Article IdArticleNavigation { get; set; }
        public virtual AspNetUsers IdNavigation { get; set; }
    }
}
