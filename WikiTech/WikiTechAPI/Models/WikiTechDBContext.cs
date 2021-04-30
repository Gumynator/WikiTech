using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WikiTechAPI.Models
{
    public partial class WikiTechDBContext : DbContext
    {
        public WikiTechDBContext()
        {
        }

        public WikiTechDBContext(DbContextOptions<WikiTechDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Abonnement> Abonnement { get; set; }
        public virtual DbSet<Apprecier> Apprecier { get; set; }
        public virtual DbSet<Article> Article { get; set; }
        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Changement> Changement { get; set; }
        public virtual DbSet<Don> Don { get; set; }
        public virtual DbSet<Facture> Facture { get; set; }
        public virtual DbSet<Genre> Genre { get; set; }
        public virtual DbSet<Grade> Grade { get; set; }
        public virtual DbSet<Log> Log { get; set; }
        public virtual DbSet<Logger> Logger { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<Postulation> Postulation { get; set; }
        public virtual DbSet<Referencer> Referencer { get; set; }
        public virtual DbSet<Referencer2> Referencer2 { get; set; }
        public virtual DbSet<Remuneration> Remuneration { get; set; }
        public virtual DbSet<Section> Section { get; set; }
        public virtual DbSet<Tag> Tag { get; set; }
        public virtual DbSet<Textstatique> Textstatique { get; set; }
        public virtual DbSet<Ville> Ville { get; set; }
        public virtual DbSet<Voir> Voir { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=WikiTechDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Abonnement>(entity =>
            {
                entity.HasKey(e => e.IdAbonnement)
                    .HasName("PK_ABONNEMENT");

                entity.ToTable("abonnement");

                entity.Property(e => e.IdAbonnement).HasColumnName("Id_abonnement");

                entity.Property(e => e.NomAbonnement)
                    .IsRequired()
                    .HasColumnName("Nom_abonnement")
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.PrixAbonnement)
                    .HasColumnName("Prix_abonnement")
                    .HasColumnType("money");
            });

            modelBuilder.Entity<Apprecier>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.IdArticle })
                    .HasName("PK_APPRECIER");

                entity.ToTable("apprecier");

                entity.Property(e => e.IdArticle).HasColumnName("Id_article");

                entity.Property(e => e.Apprecie).HasColumnName("apprecie");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.Apprecier)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_APPRECIER_ASPNETUSERS");

                entity.HasOne(d => d.IdArticleNavigation)
                    .WithMany(p => p.Apprecier)
                    .HasForeignKey(d => d.IdArticle)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_APPRECIER_ARTICLE");
            });

            modelBuilder.Entity<Article>(entity =>
            {
                entity.HasKey(e => e.IdArticle)
                    .HasName("PK_ARTICLE");

                entity.ToTable("article");

                entity.Property(e => e.IdArticle).HasColumnName("Id_article");

                entity.Property(e => e.DatepublicationArticle)
                    .HasColumnName("Datepublication_article")
                    .HasColumnType("datetime");

                entity.Property(e => e.DescriptionArticle)
                    .IsRequired()
                    .HasColumnName("Description_article")
                    .HasColumnType("text");

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.IdSection).HasColumnName("Id_section");

                entity.Property(e => e.IsactiveArticle).HasColumnName("Isactive_article");

                entity.Property(e => e.IsqualityArticle).HasColumnName("Isquality_article");

                entity.Property(e => e.TextArticle)
                    .IsRequired()
                    .HasColumnName("Text_article")
                    .HasColumnType("text");

                entity.Property(e => e.TitreArticle)
                    .IsRequired()
                    .HasColumnName("Titre_article")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.Article)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ARTICLE_ASPNETUSERS");

                entity.HasOne(d => d.IdSectionNavigation)
                    .WithMany(p => p.Article)
                    .HasForeignKey(d => d.IdSection)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ARTICLE_SECTION");
            });

            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.Property(e => e.RoleId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.Property(e => e.AdresseAspnetuser)
                    .HasColumnName("Adresse_aspnetuser")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CreditAspnetuser).HasColumnName("Credit_aspnetuser");

                entity.Property(e => e.CvvcarteAspnetuser).HasColumnName("Cvvcarte__aspnetuser");

                entity.Property(e => e.DatecreationAspnetuser)
                    .HasColumnName("Datecreation_aspnetuser")
                    .HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.ExpirationcarteAspnetuser)
                    .HasColumnName("Expirationcarte_aspnetuser")
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.IbanAspnetuser).HasColumnName("Iban_aspnetuser");

                entity.Property(e => e.IdAbonnement).HasColumnName("Id_abonnement");

                entity.Property(e => e.IdGenre).HasColumnName("Id_genre");

                entity.Property(e => e.IdGrade).HasColumnName("Id_grade");

                entity.Property(e => e.IdVille).HasColumnName("Id_ville");

                entity.Property(e => e.IsactiveAspnetuser).HasColumnName("Isactive_aspnetuser");

                entity.Property(e => e.IsprivateAspnetuser).HasColumnName("Isprivate__aspnetuser");

                entity.Property(e => e.NbpointAspnetuser).HasColumnName("Nbpoint_aspnetuser");

                entity.Property(e => e.NomAspnetuser)
                    .IsRequired()
                    .HasColumnName("Nom_aspnetuser")
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.NumcarteAspnetuser).HasColumnName("Numcarte_aspnetuser");

                entity.Property(e => e.PrenomAspnetuser)
                    .IsRequired()
                    .HasColumnName("prenom_aspnetuser")
                    .HasMaxLength(24)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.UserName).HasMaxLength(256);

                entity.HasOne(d => d.IdAbonnementNavigation)
                    .WithMany(p => p.AspNetUsers)
                    .HasForeignKey(d => d.IdAbonnement)
                    .HasConstraintName("FK_ASPNETUSERS_ABONNEMENT");

                entity.HasOne(d => d.IdGenreNavigation)
                    .WithMany(p => p.AspNetUsers)
                    .HasForeignKey(d => d.IdGenre)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ASPNETUSERS_GENRE");

                entity.HasOne(d => d.IdGradeNavigation)
                    .WithMany(p => p.AspNetUsers)
                    .HasForeignKey(d => d.IdGrade)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ASPNETUSERS_GRADE");

                entity.HasOne(d => d.IdVilleNavigation)
                    .WithMany(p => p.AspNetUsers)
                    .HasForeignKey(d => d.IdVille)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ASPNETUSERS_VILLE");
            });

            modelBuilder.Entity<Changement>(entity =>
            {
                entity.HasKey(e => e.IdChangement)
                    .HasName("PK_CHANGEMENT");

                entity.Property(e => e.IdChangement).HasColumnName("Id_changement");

                entity.Property(e => e.DatepublicationChangement)
                    .HasColumnName("Datepublication_changement")
                    .HasColumnType("datetime");

                entity.Property(e => e.DescriptionChangement)
                    .IsRequired()
                    .HasColumnName("Description_changement")
                    .HasColumnType("text");

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.IdArticle).HasColumnName("Id_article");

                entity.Property(e => e.ResumeChangement)
                    .IsRequired()
                    .HasColumnName("Resume_changement")
                    .HasColumnType("text");

                entity.Property(e => e.TextChangement)
                    .IsRequired()
                    .HasColumnName("Text_changement")
                    .HasColumnType("text");

                entity.Property(e => e.TitreChangement)
                    .IsRequired()
                    .HasColumnName("Titre_changement")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.Changement)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CHANGEMENT_ASPNETUSERS");

                entity.HasOne(d => d.IdArticleNavigation)
                    .WithMany(p => p.Changement)
                    .HasForeignKey(d => d.IdArticle)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CHANGEMENT_ARTICLE");
            });

            modelBuilder.Entity<Don>(entity =>
            {
                entity.HasKey(e => e.IdDon)
                    .HasName("PK_DON");

                entity.Property(e => e.IdDon)
                    .HasColumnName("Id_don")
                    .ValueGeneratedNever();

                entity.Property(e => e.DateDon)
                    .HasColumnName("Date_don")
                    .HasColumnType("datetime");

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.MessageDon)
                    .HasColumnName("Message_don")
                    .HasColumnType("text");

                entity.Property(e => e.MontantDon)
                    .HasColumnName("Montant_don")
                    .HasColumnType("money");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.Don)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DON_ASPNETUSERS");
            });

            modelBuilder.Entity<Facture>(entity =>
            {
                entity.HasKey(e => e.IdFacture)
                    .HasName("PK_FACTURE");

                entity.Property(e => e.IdFacture).HasColumnName("Id_facture");

                entity.Property(e => e.DateFacture)
                    .HasColumnName("Date_facture")
                    .HasColumnType("datetime");

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.MontantFacture)
                    .HasColumnName("Montant_facture")
                    .HasColumnType("money");

                entity.Property(e => e.TitreFacture)
                    .IsRequired()
                    .HasColumnName("Titre_facture")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.Facture)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FACTURE_ASPNETUSERS");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasKey(e => e.IdGenre)
                    .HasName("PK_GENRE");

                entity.Property(e => e.IdGenre).HasColumnName("Id_genre");

                entity.Property(e => e.NomGenre)
                    .IsRequired()
                    .HasColumnName("Nom_genre")
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Grade>(entity =>
            {
                entity.HasKey(e => e.IdGrade)
                    .HasName("PK_GRADE");

                entity.Property(e => e.IdGrade).HasColumnName("Id_grade");

                entity.Property(e => e.MinpointGrade).HasColumnName("Minpoint_grade");

                entity.Property(e => e.NomGrade)
                    .IsRequired()
                    .HasColumnName("Nom_grade")
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.HasKey(e => e.IdLog)
                    .HasName("PK_LOG");

                entity.Property(e => e.IdLog).HasColumnName("Id_log");

                entity.Property(e => e.DateLog)
                    .HasColumnName("Date_log")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Logger>(entity =>
            {
                entity.HasKey(e => new { e.IdLogger, e.IdArticle, e.Id })
                    .HasName("PK_LOGGER");

                entity.Property(e => e.IdLogger).HasColumnName("Id_logger");

                entity.Property(e => e.IdArticle).HasColumnName("Id_article");

                entity.Property(e => e.MessageLogger)
                    .HasColumnName("Message_logger")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.Logger)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LOGGER_ASPNETUSERS");

                entity.HasOne(d => d.IdArticleNavigation)
                    .WithMany(p => p.Logger)
                    .HasForeignKey(d => d.IdArticle)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LOGGER_ARTICLE");

                entity.HasOne(d => d.IdLoggerNavigation)
                    .WithMany(p => p.Logger)
                    .HasForeignKey(d => d.IdLogger)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LOGGER_LOG");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.IdMessage)
                    .HasName("PK_MESSAGE");

                entity.Property(e => e.IdMessage).HasColumnName("Id_message");

                entity.Property(e => e.CorpsMessage)
                    .IsRequired()
                    .HasColumnName("Corps_message")
                    .HasColumnType("text");

                entity.Property(e => e.DateMessage)
                    .HasColumnName("Date_message")
                    .HasColumnType("datetime");

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.IdArticle).HasColumnName("Id_article");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.Message)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MESSAGE_ASPNETUSERS");

                entity.HasOne(d => d.IdArticleNavigation)
                    .WithMany(p => p.Message)
                    .HasForeignKey(d => d.IdArticle)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MESSAGE_ARTICLE");
            });

            modelBuilder.Entity<Postulation>(entity =>
            {
                entity.HasKey(e => e.IdPostulation)
                    .HasName("PK_POSTULATION");

                entity.Property(e => e.IdPostulation).HasColumnName("Id_postulation");

                entity.Property(e => e.DatePostulation)
                    .HasColumnName("Date_postulation")
                    .HasColumnType("datetime");

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.TextPostulation)
                    .IsRequired()
                    .HasColumnName("Text_postulation")
                    .HasColumnType("text");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.Postulation)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_POSTULATION_ASPNETUSERS");
            });

            modelBuilder.Entity<Referencer>(entity =>
            {
                entity.HasKey(e => new { e.IdTag, e.IdArticle })
                    .HasName("PK_REFERENCER");

                entity.Property(e => e.IdTag).HasColumnName("Id_tag");

                entity.Property(e => e.IdArticle).HasColumnName("Id_article");

                entity.HasOne(d => d.IdArticleNavigation)
                    .WithMany(p => p.Referencer)
                    .HasForeignKey(d => d.IdArticle)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_REFERENCER_ARTICLE");

                entity.HasOne(d => d.IdTagNavigation)
                    .WithMany(p => p.Referencer)
                    .HasForeignKey(d => d.IdTag)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_REFERENCER_TAG");
            });

            modelBuilder.Entity<Referencer2>(entity =>
            {
                entity.HasKey(e => new { e.IdTag, e.IdChangement })
                    .HasName("PK_REFERENCER2");

                entity.Property(e => e.IdTag).HasColumnName("Id_tag");

                entity.Property(e => e.IdChangement).HasColumnName("Id_changement");

                entity.HasOne(d => d.IdChangementNavigation)
                    .WithMany(p => p.Referencer2)
                    .HasForeignKey(d => d.IdChangement)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_REFERENCER2_CHANGEMENT");

                entity.HasOne(d => d.IdTagNavigation)
                    .WithMany(p => p.Referencer2)
                    .HasForeignKey(d => d.IdTag)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_REFERENCER2_TAG");
            });

            modelBuilder.Entity<Remuneration>(entity =>
            {
                entity.HasKey(e => e.IdRemuneration)
                    .HasName("PK_REMUNERATION");

                entity.ToTable("remuneration");

                entity.Property(e => e.IdRemuneration).HasColumnName("Id_remuneration");

                entity.Property(e => e.DateRemuneration)
                    .HasColumnName("Date_remuneration")
                    .HasColumnType("datetime");

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.MontantRemuneration)
                    .HasColumnName("Montant_remuneration")
                    .HasColumnType("money");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.Remuneration)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_REMUNERATION_ASPNETUSERS");
            });

            modelBuilder.Entity<Section>(entity =>
            {
                entity.HasKey(e => e.IdSection)
                    .HasName("PK_SECTION");

                entity.ToTable("section");

                entity.Property(e => e.IdSection).HasColumnName("Id_section");

                entity.Property(e => e.NomSection)
                    .IsRequired()
                    .HasColumnName("Nom_section")
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasKey(e => e.IdTag)
                    .HasName("PK_TAG");

                entity.Property(e => e.IdTag).HasColumnName("Id_tag");

                entity.Property(e => e.NomTag)
                    .IsRequired()
                    .HasColumnName("Nom_tag")
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Textstatique>(entity =>
            {
                entity.HasKey(e => e.IdTextstatique)
                    .HasName("PK_TEXTSTATIQUE");

                entity.Property(e => e.IdTextstatique).HasColumnName("Id_textstatique");

                entity.Property(e => e.TitreTextstatique)
                    .IsRequired()
                    .HasColumnName("Titre_textstatique")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.TxtTextstatique)
                    .IsRequired()
                    .HasColumnName("Txt_textstatique")
                    .HasColumnType("text");
            });

            modelBuilder.Entity<Ville>(entity =>
            {
                entity.HasKey(e => e.IdVille)
                    .HasName("PK_VILLE");

                entity.Property(e => e.IdVille).HasColumnName("Id_ville");

                entity.Property(e => e.CodeVille).HasColumnName("Code_ville");

                entity.Property(e => e.NomVille)
                    .IsRequired()
                    .HasColumnName("Nom_ville")
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Voir>(entity =>
            {
                entity.HasKey(e => new { e.IdVoir, e.IdArticle })
                    .HasName("PK_VOIR");

                entity.Property(e => e.IdVoir).HasColumnName("Id_voir");

                entity.Property(e => e.IdArticle).HasColumnName("Id_article");

                entity.HasOne(d => d.IdArticleNavigation)
                    .WithMany(p => p.Voir)
                    .HasForeignKey(d => d.IdArticle)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VOIR_ARTICLE");

                entity.HasOne(d => d.IdVoirNavigation)
                    .WithMany(p => p.Voir)
                    .HasForeignKey(d => d.IdVoir)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VOIR_ASPNETUSERS");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
