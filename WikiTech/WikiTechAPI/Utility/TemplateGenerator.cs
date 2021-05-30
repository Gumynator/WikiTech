//Auteur    : Pancini Marco
//Date      : 17.05.2021
//Fichier   : TemplateGenerator.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiTechAPI.Models;

namespace WikiTechAPI.Utility
{
    public class TemplateGenerator
    {
        public static string GetHTMLFactureString(Facture newPdfFacture)
        {
            var PdfFacture = newPdfFacture;
            var sb = new StringBuilder();
            sb.Append(@"
                        <html>
                            <head>
                            </head>
                            <body>
                                <div class='header'><h1>Preuve de paiement à un abonnement WikiTech</h1></div>
                                <table align='center'>
                                    <tr>
                                        <th>Abonnement</th>
                                        <th>Montant</th>
                                        <th>Date</th>
                                    </tr>");
                sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                  </tr>", PdfFacture.TitreFacture, PdfFacture.MontantFacture+" CHF", PdfFacture.DateFacture);
            sb.Append(@"
                                </table>
                            </body>
                        </html>");
            return sb.ToString();
        }

        public static string GetHTMLDonString(Don newPdfDon)
        {
            var PdfDon = newPdfDon;
            var sb = new StringBuilder();
            sb.Append(@"
                        <html>
                            <head>
                            </head>
                            <body>
                                <div class='header'><h1>Preuve de don à WikiTech</h1></div>
                                <a>
                                <table align='center'>
                                    <tr>
                                        <th>Message</th>
                                        <th>Montant</th>
                                        <th>Date</th>
                                    </tr>");
            sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                  </tr>", PdfDon.MessageDon, PdfDon.MontantDon + " CHF", PdfDon.DateDon);
            sb.Append(@"
                                </table>
                            </body>
                        </html>");
            return sb.ToString();
        }
    }
}
