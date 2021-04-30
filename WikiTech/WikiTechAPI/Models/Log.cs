using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WikiTechAPI.Models
{
    public partial class Log
    {
        public Log()
        {
            Logger = new HashSet<Logger>();
        }

        public int IdLog { get; set; }
        public DateTime DateLog { get; set; }

        public virtual ICollection<Logger> Logger { get; set; }
    }
}
