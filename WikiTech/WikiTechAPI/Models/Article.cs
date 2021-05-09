using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WikiTechAPI.Models
{
    public partial class Article
    {
        public Article()
        {
            Apprecier = new HashSet<Apprecier>();
            Changement = new HashSet<Changement>();
            Logger = new HashSet<Logger>();
            Message = new HashSet<Message>();
            Referencer = new HashSet<Referencer>();
            Voir = new HashSet<Voir>();
        }

        
        public int IdArticle { get; set; }
        public string Id { get; set; }
        [Required(ErrorMessage = "Vous devez selectionner une section")]
        public int IdSection { get; set; }
        [Required(ErrorMessage = "Vous devez Entrer un titre à l'article")]
        public string TitreArticle { get; set; }
        [Required(ErrorMessage = "Vous devez entrer du contenu à l'article")]
        public string TextArticle { get; set; }
        public DateTime? DatepublicationArticle { get; set; }
        public bool IsactiveArticle { get; set; }
        public bool IsqualityArticle { get; set; }
        [Required(ErrorMessage = "Vous devez entrer une description")]
        public string DescriptionArticle { get; set; }

        [ForeignKey("Id")]
        public virtual AspNetUsers IdNavigation { get; set; }
        public virtual Section IdSectionNavigation { get; set; }
        public virtual ICollection<Apprecier> Apprecier { get; set; }
        public virtual ICollection<Changement> Changement { get; set; }
        public virtual ICollection<Logger> Logger { get; set; }
        public virtual ICollection<Message> Message { get; set; }
        public virtual ICollection<Referencer> Referencer { get; set; }
        public virtual ICollection<Voir> Voir { get; set; }

        public static implicit operator Article(Stream v)
        {
            throw new NotImplementedException();
        }
    }
}
