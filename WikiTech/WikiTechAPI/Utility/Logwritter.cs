using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WikiTechAPI.Utility
{
    public class Logwritter
    {
        const String PATH = "./Logs/ApiLog.txt";
        private String message;
        private DateTime date;

        public Logwritter(String _message)
        {
            message = _message;
            date = DateTime.Now;
        }

        public void writeLog()
        {
            using (StreamWriter outputFile = new StreamWriter(PATH, true))
            {
                String line = "LOG : " + date + " | Message : " + message;
                outputFile.WriteLine(line);
            }

        }


    }


}
