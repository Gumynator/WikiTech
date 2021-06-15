//Auteur    : Loris habegger
//Date      : 15.06.2021
//Fichier   : Logwritter.cs (Instance permettant la gestion des logs Ecriture et lecture et suppression)

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
        const int MAX_LINE = 1000;
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
                String line = date + " | Message : " + message;
                outputFile.WriteLine(line);
            }


            //// Eviter la taille max du fichier
            List<String> allLog = new List<string>();

            foreach (var line in File.ReadLines(PATH))
            {
                allLog.Add(line);
            }

            if (allLog.Count > MAX_LINE)
            {
                allLog.RemoveAt(0);
            }
            //// Eviter la taille max du fichier

        }

        static public List<String> readLog()
        {
            List<String> allLog = new List<string>();

            foreach (var line in File.ReadLines(PATH))
            {
                allLog.Add(line);
            }

            //if supérieur à 1000 ligne supprimé celle qui dépasse

            return allLog;
        }


    }


}
