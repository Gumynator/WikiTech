//Auteur    : Pancini Marco
//Date      : 06.05.2021
//Fichier   : DonsController.cs
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Stripe;
using WikiTechAPI.Models;
using WikiTechWebApp.Models;
using WikiTechWebApp.ApiFunctions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace WikiTechWebApp.Controllers
{
    public class DonsController : Controller
    {
        //private readonly WikiTechDBContext _context;
        static HttpClient client = new HttpClient();
        static IEmailSender _sender;
        public DonsController(IEmailSender sender)
        {
            client = ConfigureHttpClient.configureHttpClient(client);
            _sender = sender;
        }

        // GET: Dons
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Auteur : Pancini Marco
        //Création : 07.05.2021
        //Modification : 10.05.2021
        //Description : Fonction qui permet de récupérer les Factures d'un utilisateur via l'API
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        [Authorize]
        public async Task<ActionResult<IEnumerable<Don>>> MesDons()
        {
            var IdUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<Don> donsList = null;
            HttpResponseMessage response = await client.GetAsync("Dons/GetDonsByUserId/" + IdUser);
            if (response.IsSuccessStatusCode)
            {

                donsList = response.Content.ReadAsAsync<IEnumerable<Don>>().Result;

            }

            return View(donsList);
            //IEnumerable<Facture> FactureList;
            //HttpResponseMessage response = client.GetAsync("Factures/").Result;
            //FactureList = response.Content.ReadAsAsync<IEnumerable<Facture>>().Result;


        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Auteur : Pancini Marco
        //Création : 27.04.2021
        //Modification : 30.05.2021
        //Description : Function qui demande à l'api de créer stocker et envoyer le pdf d'une facture
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        public async Task<IActionResult> GetPdf(int? id)
        {
            Don dons = null;
            HttpResponseMessage response = await client.GetAsync("Dons/" + id);
            if (response.IsSuccessStatusCode)
            {

                dons = await response.Content.ReadAsAsync<Don>();
            }
            HttpResponseMessage postPdfDon = await client.PostAsJsonAsync("PdfCreator/CreatePdfDon/", dons);
            string url = client.BaseAddress.AbsoluteUri;
            url = url.Remove(22, 4);

            using (var result = await client.GetAsync(url + "pdfdons/" + dons.IdDon + ".pdf"))
            {
                if (result.IsSuccessStatusCode)
                {
                    var file = await result.Content.ReadAsByteArrayAsync();
                    return File(file, "application/pdf", dons.IdDon + ".pdf");
                }

            }
            return null;
            //récuperer le fichier 
            //var filecontent = await postPdfFacture.Content.ReadAsByteArrayAsync();
            //return File(filecontent, "application/pdf", factures.IdFacture + ".pdf");
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Auteur : Pancini Marco
        //Création : 27.05.2021
        //Modification : 29.05.2021
        //Description : Fonction qui fait une requête à l'api afin de récupérer les 20 meilleures dons trier de façon décroissante
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        public async Task<ActionResult<IEnumerable<Donateurs>>> DonateursAsync()
        {
            IEnumerable<Donateurs> donList = null;
            HttpResponseMessage response = await client.GetAsync("Dons/GetAllDons/");
            if (response.IsSuccessStatusCode)
            {

                donList = response.Content.ReadAsAsync<IEnumerable<Donateurs>>().Result;

            }

            return View(donList);
            //IEnumerable<Facture> FactureList;
            //HttpResponseMessage response = client.GetAsync("Factures/").Result;
            //FactureList = response.Content.ReadAsAsync<IEnumerable<Facture>>().Result;


        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Auteur : Pancini Marco
        //Création : 06.05.2021
        //Modification : 10.05.2021
        //Description : Fonction Charge qui permet le paiement d'un don par l'utilisateur en utilisant l'API Stripe
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        [Authorize]
        public async Task<IActionResult> ChargeAsync( DonsModelView data)
        {
            //var
            var IdUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await FunctionAPI.GetUserByIdAsync(client, IdUser);
            var ville = await FunctionAPI.GetVilleByIdAsync(client, user.IdVille);
            var price = Convert.ToInt32(data.Total);
            string description = "Dons de "+price+" de la part de "+user.NomAspnetuser + user.PrenomAspnetuser;

            try//stripeexception
            {
                ///stripe var
                var customers = new CustomerService();
                var charges = new ChargeService();
                var customer = customers.Create(new CustomerCreateOptions
                {
                    Email = user.UserName,
                    Source = data.Token
                });

                var charge = charges.Create(new ChargeCreateOptions
                {
                    Amount = price * 100,
                    Description = description,
                    Currency = "CHF",
                    Customer = customer.Id,
                    ReceiptEmail = user.UserName,
                    Metadata = new Dictionary<string, string>
                {
                    {"UserName" , Convert.ToString(user.NomAspnetuser +" "+ user.PrenomAspnetuser) },
                    {"Postcode" , Convert.ToString(ville.CodeVille)},
                }
                });

                //Confirmation validation du payement 
                if (charge.Status == "succeeded")
                {
                    string BalanceTransactionId = charge.BalanceTransactionId;

                    Don newDon = new Don();
                    newDon.MontantDon = price;
                    newDon.DateDon = DateTime.Now.Date;
                    newDon.MessageDon = description;
                    newDon.Id = user.Id;
                    FunctionAPI.IncreasePointForUser(client, user.Id, 2);
                    HttpResponseMessage postDons = await client.PostAsJsonAsync("Dons", newDon);
                    await _sender.SendEmailAsync(user.Email, "Don à WikiTech ", "Bonjour, merci de pour votre don à WikiTech de "+newDon.MontantDon+" CHF.");

                    ViewBag.nom = "Dons";
                    ViewBag.prix = price;

                    
                    return View();
                }
                else
                {
                    return View("Erreur");
                }

            }
            catch (StripeException)
            {

                return View("Erreur");
            }
        }

    }
}
