using System;
using System.Collections.Generic;
using System.Linq;
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

namespace WikiTechWebApp.Controllers
{
    public class AbonnementsController : Controller
    {
        private readonly WikiTechDBContext _context;
        static HttpClient client = new HttpClient();
        public AbonnementsController()
        {
            client = ConfigureHttpClient.configureHttpClient(client);
        }

        // GET: Abonnements
        [Authorize]
        public IActionResult Index()
        {
            IEnumerable<Abonnement> AbonnementList;
            HttpResponseMessage response = client.GetAsync("Abonnements").Result;
            AbonnementList = response.Content.ReadAsAsync<IEnumerable<Abonnement>>().Result;

            return View(AbonnementList);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Auteur : Pancini Marco
        //Création : 11.04.2021
        //Modification : 19.04.2021
        //Description : Fonction Temporaire, qui permet la récupération du prix lors de la séléction de l'abonnement
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        [Authorize]
        public async Task<IActionResult> Achat(int? id)
        {
            var subscription = await FunctionAPI.GetAbonnementByIdAsync(client, id);
            decimal price = subscription.PrixAbonnement;
            string name = subscription.NomAbonnement;
            //passer l'id de l'abonnement au controller
            ViewBag.price = price ;
            ViewBag.Displayprice = price;
            ViewBag.id = id;
            ViewBag.name = name;
            return View();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Auteur : Pancini Marco
        //Création : 11.04.2021
        //Modification : 09.05.2021
        //Description : Fonction Charge, qui permet l'utilisation de l'API Stripe et envoie d'email
        // Cette fonction va être modifier pour utiliser l'API et ainsi utiliser les ID des abonnements au lieu d'entrer les données en dures
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        [Authorize]
        public async Task<IActionResult> ChargeAsync(int? id, PayModelView data)
        {
            var IdUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customers = new CustomerService();
            var charges = new ChargeService();
            var user = await FunctionAPI.GetUserByIdAsync(client, IdUser);
            var subscription = await FunctionAPI.GetAbonnementByIdAsync(client, id);
            var price = Convert.ToInt32(subscription.PrixAbonnement)*100;
            var description = subscription.NomAbonnement;

            var customer = customers.Create(new CustomerCreateOptions
            {
                Email = user.UserName,
                Source = data.Token
            });

            var charge = charges.Create(new ChargeCreateOptions
            {
                Amount = price,
                Description = description,
                Currency = "CHF",
                Customer = customer.Id,
                ReceiptEmail = user.UserName,
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

                Facture newFacture = new Facture();
                newFacture.MontantFacture = price/100;
                newFacture.DateFacture = DateTime.Now.Date;
                newFacture.TitreFacture = subscription.NomAbonnement;
                newFacture.Id = user.Id;
                HttpResponseMessage postFacture = await client.PostAsJsonAsync("Factures", newFacture);

                user.IdAbonnement = subscription.IdAbonnement;
                HttpResponseMessage putUser = await client.PutAsJsonAsync("AspNetUsers/" + user.Id, user);
                if (putUser.IsSuccessStatusCode)
                {

                    user = await putUser.Content.ReadAsAsync<AspNetUsers>();
                }


                ViewBag.nom = subscription.NomAbonnement;
                ViewBag.prix = subscription.PrixAbonnement;


                return View();
            }
            else
            {

            }

            return View();
        }


        // GET: Abonnements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var abonnement = await _context.Abonnement
                .FirstOrDefaultAsync(m => m.IdAbonnement == id);
            if (abonnement == null)
            {
                return NotFound();
            }

            return View(abonnement);
        }

        // GET: Abonnements/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Abonnements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAbonnement,NomAbonnement,PrixAbonnement")] Abonnement abonnement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(abonnement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(abonnement);
        }

        // GET: Abonnements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var abonnement = await _context.Abonnement.FindAsync(id);
            if (abonnement == null)
            {
                return NotFound();
            }
            return View(abonnement);
        }

        // POST: Abonnements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAbonnement,NomAbonnement,PrixAbonnement")] Abonnement abonnement)
        {
            if (id != abonnement.IdAbonnement)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(abonnement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AbonnementExists(abonnement.IdAbonnement))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(abonnement);
        }

        // GET: Abonnements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var abonnement = await _context.Abonnement
                .FirstOrDefaultAsync(m => m.IdAbonnement == id);
            if (abonnement == null)
            {
                return NotFound();
            }

            return View(abonnement);
        }

        // POST: Abonnements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var abonnement = await _context.Abonnement.FindAsync(id);
            _context.Abonnement.Remove(abonnement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AbonnementExists(int id)
        {
            return _context.Abonnement.Any(e => e.IdAbonnement == id);
        }
    }
}
