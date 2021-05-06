using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikiTechWebApp.Exceptions
{
    public class ExceptionImg : Exception
    {
        private string message;
        public ExceptionImg()
        {

        }

        public string getMessage()
        {
            message = "Image non uploadée";
            return message;
        }


    }
}
