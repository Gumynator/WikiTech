using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikiTechAPI.Models;

namespace WikiTechAPI.Utility
{
    public class DataStorage
    {
        public static List<PdfFacture> GetAllEmployees() =>
        new List<PdfFacture>
    {
                new PdfFacture { Name="Mike", LastName="Turner", Age=35, Gender="Male"},
                new PdfFacture { Name="Sonja", LastName="Markus", Age=22, Gender="Female"},
                new PdfFacture { Name="Luck", LastName="Martins", Age=40, Gender="Male"},
                new PdfFacture { Name="Sofia", LastName="Packner", Age=30, Gender="Female"},
                new PdfFacture { Name="John", LastName="Doe", Age=45, Gender="Male"}
    };
    }
}
