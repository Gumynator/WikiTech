using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WikiTechAPI.Models;
using WikiTechWebApp.ApiFunctions;

namespace WikiTechWebApp.Controllers
{
    public class FacturesController : Controller
    {
        //private readonly WikiTechDBContext _context;
        static HttpClient client = new HttpClient();
        public FacturesController()
        {
            //_context = context;
            client = ConfigureHttpClient.configureHttpClient(client);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Auteur : Pancini Marco
        //Création : 07.05.2021
        //Modification : 10.05.2021
        //Description : Fonction qui permet de récupérer les Factures d'un utilisateur via l'API
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        public async Task<ActionResult<IEnumerable<Facture>>> IndexAsync()
        {
            var IdUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<Facture> facturesList = null;
            HttpResponseMessage response = await client.GetAsync("Factures/FacturesByUserId/" + IdUser);
            if (response.IsSuccessStatusCode)
            {

                facturesList = response.Content.ReadAsAsync<IEnumerable<Facture>>().Result;

            }

            return View(facturesList);
            //IEnumerable<Facture> FactureList;
            //HttpResponseMessage response = client.GetAsync("Factures/").Result;
            //FactureList = response.Content.ReadAsAsync<IEnumerable<Facture>>().Result;


        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Auteur : Pancini Marco
        //Création : 25.04.2021
        //Modification : 30.05.2021
        //Description : Function qui demande à l'api de créer stocker et envoyer le pdf d'une facture
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        // GET: Factures/GetPdf/5
        public async Task<IActionResult> GetPdf(int? id)
        {
            Facture factures = null;
            HttpResponseMessage response = await client.GetAsync("Factures/" + id);
            if (response.IsSuccessStatusCode)
            {

                factures = await response.Content.ReadAsAsync<Facture>();
            }
            HttpResponseMessage postPdfFacture = await client.PostAsJsonAsync("PdfCreator/CreatePdfFacture/", factures);
            string url = client.BaseAddress.AbsoluteUri;
            url = url.Remove(37,4);

            using (var result = await client.GetAsync(url + "pdffactures/" + factures.IdFacture + ".pdf"))
            {
                if (result.IsSuccessStatusCode)
                {
                    var file = await result.Content.ReadAsByteArrayAsync();
                    return File(file, "application/pdf", factures.IdFacture + ".pdf");
                }

            }
            return null;
            //récuperer le fichier 
            //var filecontent = await postPdfFacture.Content.ReadAsByteArrayAsync();
            //return File(filecontent, "application/pdf", factures.IdFacture + ".pdf");
        }

    }
}
