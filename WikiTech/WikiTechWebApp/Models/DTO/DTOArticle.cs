using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WikiTechAPI.Models;

namespace WikiTechWebApp.Models.DTO
{
    public partial class DTOArticle
    {
        public DTOArticle()
        {
            Apprecier = new HashSet<Apprecier>();
            Changement = new HashSet<Changement>();
            Logger = new HashSet<Logger>();
            Message = new HashSet<Message>();
            Referencer = new HashSet<Referencer>();
            Voir = new HashSet<Voir>();
        }



        

        public int IdArticle { get; set; }
        [Required]
        public string Id { get; set; }
        [Required]
        public int IdSection { get; set; }
        [Required]
        public string TitreArticle { get; set; }
        [Required]
        public string TextArticle { get; set; }

        public DateTime? DatepublicationArticle { get; set; }

        public bool IsactiveArticle { get; set; }

        public bool IsqualityArticle { get; set; }
        [Required]
        public string DescriptionArticle { get; set; }

        public virtual AspNetUsers IdNavigation { get; set; }
        public virtual Section IdSectionNavigation { get; set; }
        public virtual ICollection<Apprecier> Apprecier { get; set; }
        public virtual ICollection<Changement> Changement { get; set; }
        public virtual ICollection<Logger> Logger { get; set; }
        public virtual ICollection<Message> Message { get; set; }
        public virtual ICollection<Referencer> Referencer { get; set; }
        public virtual ICollection<Voir> Voir { get; set; }
    }
}
