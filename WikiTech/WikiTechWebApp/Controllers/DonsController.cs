﻿using System;
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
    public class DonsController : Controller
    {
        private readonly WikiTechDBContext _context;
        static HttpClient client = new HttpClient();
        public DonsController()
        {
            client = ConfigureHttpClient.configureHttpClient(client);
            client.DefaultRequestHeaders.Add("ApiKey", "61c08ad1-0823-4c38-9853-700675e3c8fc");
        }

        // GET: Dons
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> ChargeAsync( DonsModelView data)
        {
            var IdUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customers = new CustomerService();
            var charges = new ChargeService();
            var user = await FunctionAPI.GetUserByIdAsync(client, IdUser);
            var price = Convert.ToInt32(data.Total);
            string description = "Dons d'un montant de "+price+" de la part de "+user.UserName;

            var customer = customers.Create(new CustomerCreateOptions
            {
                Email = user.UserName,
                Source = data.Token
            });

            var charge = charges.Create(new ChargeCreateOptions
            {
                Amount = price*100,
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

                Don newDon = new Don();
                newDon.MontantDon = price;
                newDon.DateDon = DateTime.Now.Date;
                newDon.MessageDon = description;
                newDon.Id = user.Id;
                HttpResponseMessage postDons = await client.PostAsJsonAsync("Dons", newDon);

                ViewBag.nom = "Dons";
                ViewBag.prix = price;


                return View();
            }
            else
            {

            }

            return View();
        }

        // GET: Dons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var don = await _context.Don
                .Include(d => d.IdNavigation)
                .FirstOrDefaultAsync(m => m.IdDon == id);
            if (don == null)
            {
                return NotFound();
            }

            return View(don);
        }

        // GET: Dons/Create
        public IActionResult Create()
        {
            ViewData["Id"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        // POST: Dons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDon,Id,MontantDon,DateDon,MessageDon")] Don don)
        {
            if (ModelState.IsValid)
            {
                _context.Add(don);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Id"] = new SelectList(_context.AspNetUsers, "Id", "Id", don.Id);
            return View(don);
        }

        // GET: Dons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var don = await _context.Don.FindAsync(id);
            if (don == null)
            {
                return NotFound();
            }
            ViewData["Id"] = new SelectList(_context.AspNetUsers, "Id", "Id", don.Id);
            return View(don);
        }

        // POST: Dons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDon,Id,MontantDon,DateDon,MessageDon")] Don don)
        {
            if (id != don.IdDon)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(don);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonExists(don.IdDon))
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
            ViewData["Id"] = new SelectList(_context.AspNetUsers, "Id", "Id", don.Id);
            return View(don);
        }

        // GET: Dons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var don = await _context.Don
                .Include(d => d.IdNavigation)
                .FirstOrDefaultAsync(m => m.IdDon == id);
            if (don == null)
            {
                return NotFound();
            }

            return View(don);
        }

        // POST: Dons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var don = await _context.Don.FindAsync(id);
            _context.Don.Remove(don);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DonExists(int id)
        {
            return _context.Don.Any(e => e.IdDon == id);
        }
    }
}