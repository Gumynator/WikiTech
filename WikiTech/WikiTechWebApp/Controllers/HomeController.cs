//Auteur    : Loris habegger
//Date      : 15.06.2021
//Fichier   : HomeController.cs

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WikiTechWebApp.Areas.Identity.Data;
using WikiTechWebApp.Models;
using System.Net.Http;
using System.Net.Mail;
using SendGrid.Helpers.Mail;
using System.IO;
using WikiTechAPI.Utility;
using Microsoft.AspNetCore.Authorization;
using WikiTechWebApp.ApiFunctions;

namespace WikiTechWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        static IEmailSender _sender;

        static HttpClient client = new HttpClient();

        public HomeController(ILogger<HomeController> logger, IEmailSender sender)
        {
            _sender = sender;
            _logger = logger;

            client = ConfigureHttpClient.configureHttpClient(client);

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<ActionResult> ContactUs()
        {
            String messageContact = Request.Form["message"];
            String NomContact = Request.Form["Nom"];
            String telContact = Request.Form["tel"];
            String emailContact = Request.Form["email"];
            String objetContact = Request.Form["objet"];


            //envoie du mail d'état à l'auteur du changement
            await _sender.SendEmailAsync("loris.habegger@eduvaud.ch", "[Formulaire contact] : " + objetContact, messageContact + "\n Soumis par : " + NomContact + "\n tél. " + telContact + "\n email : " + emailContact);

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public IActionResult Log()
        {

            HttpResponseMessage responsearticle = client.GetAsync("ApiLog").Result;
            var logs = responsearticle.Content.ReadAsAsync<List<string>>().Result;

            ViewData["logs"] = logs;

            return View();
        }

    }
}
