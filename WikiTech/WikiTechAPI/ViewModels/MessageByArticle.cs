using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikiTechAPI.ViewModels
{
    public class MessageByArticle
    {
        public string PrenomAspnetuser { get; set; }
        public string CorpsMessage { get; set; }
        public DateTime DateMessage { get; set; }
        public int IdArticle { get; set; }

    }
}
