using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stripe;
using StripeProto.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace StripeProto.Controllers
{
    public class AbonnementController : Controller
    {
        private readonly ILogger<AbonnementController> _logger;

        public AbonnementController(ILogger<AbonnementController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Charge(string stripeEmail, string stripeToken, int prix)
        {
            var customers = new CustomerService();
            var charges = new ChargeService();

            var customer = customers.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            var charge = charges.Create(new ChargeCreateOptions
            {
                Amount = prix,
                Description = "Test",
                Currency = "CHF",
                Customer = customer.Id,
                ReceiptEmail = stripeEmail,
                Metadata = new Dictionary<string, string>
                {
                    {"OrderId" , "111" },
                    { "Postcode" , "1829" },
                }
            });

            //Confirmation validation du payement 
            if (charge.Status == "succeeded")
            {

                string BalanceTransactionId = charge.BalanceTransactionId;

                ///creation du contenu de l'email
                MailMessage mailMessage = new MailMessage("MovieToGoOnline@gmail.com", stripeEmail)
                {
                    Subject = "Facture de WikiTech",
                    Body = "Vous avez payé "+ prix.ToString()+" vous pouvez maintenant profiter de votre abonnement à WikiTech"
                };
                mailMessage.IsBodyHtml = true;

                ///smtp de gmail
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential()
                {
                    UserName = "MovieToGoOnline@gmail.com",
                    Password = "Etml$sig2"
                };

                client.EnableSsl = true;
                client.Send(mailMessage);

                return View();
            }
            else
            {

            }

            return View();
        }

        public IActionResult Achat(int prix)
        {
            ViewBag.prix = prix;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
