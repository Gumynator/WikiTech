using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace IdentityProto.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "Int(32)")]
        public int Id_grade { get; set; }

        [PersonalData]
        [Column(TypeName = "Int(32)")]
        public int Id_ville { get; set; }

        [PersonalData]
        [Column(TypeName = "Int(32)")]
        public int Id_genre { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(50)")]
        public string Prenom_aspnetuser { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(50)")]
        public string Nom_aspnetuser { get; set; }

        [PersonalData]
        [Column(TypeName = "bit")]
        public bool Isactive_aspnetuser { get; set; }

        [PersonalData]
        [Column(TypeName = "bit")]
        public bool Isprivate_aspnetuser { get; set; }

        [PersonalData]
        [Column(TypeName = "Date")]
        public DateTime Datecreation_aspnetuser { get; set; }

       [PersonalData]
       [Column(TypeName = "Int(32)")]
        public int Nbpoint_aspnetuser { get; set; }

    }
}
