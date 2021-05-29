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

namespace WikiTechWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        static IEmailSender _sender;

        public HomeController(ILogger<HomeController> logger, IEmailSender sender)
        {
            _sender = sender;
            _logger = logger;
            
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

    }
}
