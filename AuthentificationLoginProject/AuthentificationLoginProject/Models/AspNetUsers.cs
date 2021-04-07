using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace AuthentificationLoginProject.Models
{
    public partial class AspNetUsers
    {
        public AspNetUsers()
        {
            Apprecier = new HashSet<Apprecier>();
            Article = new HashSet<Article>();
            AspNetUserClaims = new HashSet<AspNetUserClaims>();
            AspNetUserLogins = new HashSet<AspNetUserLogins>();
            AspNetUserRoles = new HashSet<AspNetUserRoles>();
            AspNetUserTokens = new HashSet<AspNetUserTokens>();
            Changement = new HashSet<Changement>();
            Don = new HashSet<Don>();
            Facture = new HashSet<Facture>();
            Logger = new HashSet<Logger>();
            Message = new HashSet<Message>();
            Postulation = new HashSet<Postulation>();
            Remuneration = new HashSet<Remuneration>();
            Voir = new HashSet<Voir>();
        }

        public string Id { get; set; }
        public int IdGrade { get; set; }
        public int? IdAbonnement { get; set; }
        public int IdVille { get; set; }
        public int IdGenre { get; set; }
        public string NomAspnetuser { get; set; }
        public string PrenomAspnetuser { get; set; }
        public string UserName { get; set; }
        public bool IsactiveAspnetuser { get; set; }
        public bool IsprivateAspnetuser { get; set; }
        public string AdresseAspnetuser { get; set; }
        public DateTime DatecreationAspnetuser { get; set; }
        public byte NbpointAspnetuser { get; set; }
        public int? NumcarteAspnetuser { get; set; }
        public byte? CvvcarteAspnetuser { get; set; }
        public string ExpirationcarteAspnetuser { get; set; }
        public int? CreditAspnetuser { get; set; }
        public int? IbanAspnetuser { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }

        public virtual Abonnement IdAbonnementNavigation { get; set; }
        public virtual Genre IdGenreNavigation { get; set; }
        public virtual Grade IdGradeNavigation { get; set; }
        public virtual Ville IdVilleNavigation { get; set; }
        public virtual ICollection<Apprecier> Apprecier { get; set; }
        public virtual ICollection<Article> Article { get; set; }
        public virtual ICollection<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual ICollection<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual ICollection<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual ICollection<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual ICollection<Changement> Changement { get; set; }
        public virtual ICollection<Don> Don { get; set; }
        public virtual ICollection<Facture> Facture { get; set; }
        public virtual ICollection<Logger> Logger { get; set; }
        public virtual ICollection<Message> Message { get; set; }
        public virtual ICollection<Postulation> Postulation { get; set; }
        public virtual ICollection<Remuneration> Remuneration { get; set; }
        public virtual ICollection<Voir> Voir { get; set; }
    }
}
