﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikiTechWebApp.Exceptions
{
    public class ExceptionLiaisonApi : Exception
    {
        private string message;

        public ExceptionLiaisonApi()
        {
        
        }


        public string getMessage()
        {
            message = "Impossible d'executer l'opération sur l'API";
            return message;
        }



    }
}
