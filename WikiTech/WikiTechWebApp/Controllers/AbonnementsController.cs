//Auteur    : Pancini Marco
//Date      : 01.05.2021
//Fichier   : AbonnementsController.cs
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

namespace WikiTechWebApp.Controllers
{
    public class AbonnementsController : Controller
    {
        //private readonly WikiTechDBContext _context;
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
        //Création : 01.05.2021
        //Modification : 06.05.2021
        //Description : Fonction qui permet la récupération des informations de l'abonnement séléctionné
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        [Authorize]
        public async Task<IActionResult> Achat(int? id)
        {
            //var
            var subscription = await FunctionAPI.GetAbonnementByIdAsync(client, id);
            decimal price = subscription.PrixAbonnement;
            string name = subscription.NomAbonnement;
            //
            ViewBag.price = price;
            ViewBag.Displayprice = price;
            ViewBag.id = id;
            ViewBag.name = name;
            return View();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Auteur : Pancini Marco
        //Création : 11.04.2021
        //Modification : 10.05.2021
        //Description : Fonction Charge qui permet le paiement d'un abonnement par l'utilisateur en utilisant l'API Stripe
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        [Authorize]
        public async Task<IActionResult> ChargeAsync(int? id, AbonnementsModelView data)
        {
            //var
            var IdUser = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            var user = await FunctionAPI.GetUserByIdAsync(client, IdUser);
            var ville = await FunctionAPI.GetVilleByIdAsync(client, user.IdVille);
            var subscription = await FunctionAPI.GetAbonnementByIdAsync(client, id);
            var price = Convert.ToInt32(subscription.PrixAbonnement) * 100;
            var description = subscription.NomAbonnement;

            try//stripeexception
            {
                //stripe var
                var charges = new ChargeService();
                var customers = new CustomerService();
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
                    {"UserName" , Convert.ToString(user.NomAspnetuser +" "+ user.PrenomAspnetuser) },
                    {"Postcode" , Convert.ToString(ville.CodeVille)},
                }
                });

                //Confirmation validation du payement 
                if (charge.Status == "succeeded")
                {
                    string BalanceTransactionId = charge.BalanceTransactionId;

                    Facture newFacture = new Facture();
                    newFacture.MontantFacture = price / 100;
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

                    charge.Metadata.Add("IdFacture", Convert.ToString(newFacture.IdFacture));
                    ViewBag.nom = subscription.NomAbonnement;
                    ViewBag.prix = subscription.PrixAbonnement;


                    return View();
                }
                else
                {
                    return View("Erreur");
                }
            }
            catch (StripeException)///carte sans crédit : 4000 0000 0000 0002
            {

                return View("Erreur");
            }
        }


    //    // GET: Abonnements/Details/5
    //    public async Task<IActionResult> Details(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return NotFound();
    //        }

    //        var abonnement = await _context.Abonnement
    //            .FirstOrDefaultAsync(m => m.IdAbonnement == id);
    //        if (abonnement == null)
    //        {
    //            return NotFound();
    //        }

    //        return View(abonnement);
    //    }

    //    // GET: Abonnements/Create
    //    public IActionResult Create()
    //    {
    //        return View();
    //    }

    //    // POST: Abonnements/Create
    //    // To protect from overposting attacks, enable the specific properties you want to bind to.
    //    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Create([Bind("IdAbonnement,NomAbonnement,PrixAbonnement")] Abonnement abonnement)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            _context.Add(abonnement);
    //            await _context.SaveChangesAsync();
    //            return RedirectToAction(nameof(Index));
    //        }
    //        return View(abonnement);
    //    }

    //    // GET: Abonnements/Edit/5
    //    public async Task<IActionResult> Edit(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return NotFound();
    //        }

    //        var abonnement = await _context.Abonnement.FindAsync(id);
    //        if (abonnement == null)
    //        {
    //            return NotFound();
    //        }
    //        return View(abonnement);
    //    }

    //    // POST: Abonnements/Edit/5
    //    // To protect from overposting attacks, enable the specific properties you want to bind to.
    //    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Edit(int id, [Bind("IdAbonnement,NomAbonnement,PrixAbonnement")] Abonnement abonnement)
    //    {
    //        if (id != abonnement.IdAbonnement)
    //        {
    //            return NotFound();
    //        }

    //        if (ModelState.IsValid)
    //        {
    //            try
    //            {
    //                _context.Update(abonnement);
    //                await _context.SaveChangesAsync();
    //            }
    //            catch (DbUpdateConcurrencyException)
    //            {
    //                if (!AbonnementExists(abonnement.IdAbonnement))
    //                {
    //                    return NotFound();
    //                }
    //                else
    //                {
    //                    throw;
    //                }
    //            }
    //            return RedirectToAction(nameof(Index));
    //        }
    //        return View(abonnement);
    //    }

    //    // GET: Abonnements/Delete/5
    //    public async Task<IActionResult> Delete(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return NotFound();
    //        }

    //        var abonnement = await _context.Abonnement
    //            .FirstOrDefaultAsync(m => m.IdAbonnement == id);
    //        if (abonnement == null)
    //        {
    //            return NotFound();
    //        }

    //        return View(abonnement);
    //    }

    //    // POST: Abonnements/Delete/5
    //    [HttpPost, ActionName("Delete")]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> DeleteConfirmed(int id)
    //    {
    //        var abonnement = await _context.Abonnement.FindAsync(id);
    //        _context.Abonnement.Remove(abonnement);
    //        await _context.SaveChangesAsync();
    //        return RedirectToAction(nameof(Index));
    //    }

    //    private bool AbonnementExists(int id)
    //    {
    //        return _context.Abonnement.Any(e => e.IdAbonnement == id);
    //    }
    }
}
