﻿using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace AuthentificationLoginProject.Models
{
    public partial class Remuneration
    {
        public int IdRemuneration { get; set; }
        public string Id { get; set; }
        public decimal MontantRemuneration { get; set; }
        public DateTime DateRemuneration { get; set; }

        public virtual AspNetUsers IdNavigation { get; set; }
    }
}
