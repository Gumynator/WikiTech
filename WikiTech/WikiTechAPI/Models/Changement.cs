using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WikiTechAPI.Models
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
        [Required(ErrorMessage = "Vous devez Entrer un titre à l'article")]
        public string TitreChangement { get; set; }
        [Required(ErrorMessage = "Vous devez entrer du contenu à l'article")]
        public string DescriptionChangement { get; set; }
        [Required(ErrorMessage = "Vous devez entrer du contenu à l'article")]
        public string TextChangement { get; set; }
        [Required(ErrorMessage = "Vous devez entrer une description du changement")]
        public string ResumeChangement { get; set; }
        public DateTime? DatepublicationChangement { get; set; }

        public virtual Article IdArticleNavigation { get; set; }
        public virtual AspNetUsers IdNavigation { get; set; }
        public virtual ICollection<Referencer2> Referencer2 { get; set; }
    }
}
