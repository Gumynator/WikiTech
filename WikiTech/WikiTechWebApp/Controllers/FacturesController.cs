using System;
using System.Collections.Generic;
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
        [Authorize]
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

        //// GET: Factures/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var facture = await _context.Facture
        //        .Include(f => f.IdNavigation)
        //        .FirstOrDefaultAsync(m => m.IdFacture == id);
        //    if (facture == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(facture);
        //}

        //// GET: Factures/Create
        //public IActionResult Create()
        //{
        //    ViewData["Id"] = new SelectList(_context.AspNetUsers, "Id", "Id");
        //    return View();
        //}

        //// POST: Factures/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("IdFacture,Id,MontantFacture,DateFacture,TitreFacture")] Facture facture)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(facture);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["Id"] = new SelectList(_context.AspNetUsers, "Id", "Id", facture.Id);
        //    return View(facture);
        //}

        //// GET: Factures/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var facture = await _context.Facture.FindAsync(id);
        //    if (facture == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["Id"] = new SelectList(_context.AspNetUsers, "Id", "Id", facture.Id);
        //    return View(facture);
        //}

        //// POST: Factures/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("IdFacture,Id,MontantFacture,DateFacture,TitreFacture")] Facture facture)
        //{
        //    if (id != facture.IdFacture)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(facture);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!FactureExists(facture.IdFacture))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["Id"] = new SelectList(_context.AspNetUsers, "Id", "Id", facture.Id);
        //    return View(facture);
        //}

        //// GET: Factures/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var facture = await _context.Facture
        //        .Include(f => f.IdNavigation)
        //        .FirstOrDefaultAsync(m => m.IdFacture == id);
        //    if (facture == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(facture);
        //}

        //// POST: Factures/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var facture = await _context.Facture.FindAsync(id);
        //    _context.Facture.Remove(facture);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool FactureExists(int id)
        //{
        //    return _context.Facture.Any(e => e.IdFacture == id);
        //}
    }
}
