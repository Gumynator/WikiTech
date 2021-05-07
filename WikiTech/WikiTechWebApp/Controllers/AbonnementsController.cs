using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Stripe;
using WikiTechAPI.Models;
using WikiTechWebApp.ApiFunctions;

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
        public IActionResult Index()
        {

            //IEnumerable<Article> abonnementList;
            //HttpResponseMessage response = client.GetAsync("Abonnements").Result;
            //abonnementList = response.Content.ReadAsAsync<IEnumerable<Article>>().Result;

            //return View(abonnementList);

            IEnumerable<Abonnement> AbonnementList;
            HttpResponseMessage response = client.GetAsync("Abonnements").Result;
            AbonnementList = response.Content.ReadAsAsync<IEnumerable<Abonnement>>().Result;

            return View(AbonnementList);

        }

        public async Task<IActionResult> Achat(short? idAbonnement)
        {
            var abonnement = await FunctionAPI.GetAbonnementAsync(client, idAbonnement);



            return View();
        }

        public async Task<IActionResult> ChargeAsync(string stripeEmail, string stripeToken, int subscriptionID, string userID)
        {
            var customers = new CustomerService();
            var charges = new ChargeService();
            var user = await _context.AspNetUsers.FindAsync(userID);
            var subscription = await _context.Abonnement.FindAsync(subscriptionID);
            var price = Convert.ToInt32(subscription.PrixAbonnement);
            var description = subscription.NomAbonnement;

            var customer = customers.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            var charge = charges.Create(new ChargeCreateOptions
            {
                Amount = price,
                Description = description,
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
