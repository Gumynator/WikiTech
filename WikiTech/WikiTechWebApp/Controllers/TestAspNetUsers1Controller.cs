using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WikiTechAPI.Models;

namespace WikiTechWebApp.Controllers
{
    public class TestAspNetUsers1Controller : Controller
    {
        private readonly WikiTechDBContext _context;

        public TestAspNetUsers1Controller(WikiTechDBContext context)
        {
            _context = context;
        }

        // GET: TestAspNetUsers1
        public async Task<IActionResult> Index()
        {
            var wikiTechDBContext = _context.AspNetUsers.Include(a => a.IdAbonnementNavigation).Include(a => a.IdGenreNavigation).Include(a => a.IdGradeNavigation).Include(a => a.IdVilleNavigation);
            return View(await wikiTechDBContext.ToListAsync());
        }

        // GET: TestAspNetUsers1/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspNetUsers = await _context.AspNetUsers
                .Include(a => a.IdAbonnementNavigation)
                .Include(a => a.IdGenreNavigation)
                .Include(a => a.IdGradeNavigation)
                .Include(a => a.IdVilleNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aspNetUsers == null)
            {
                return NotFound();
            }

            return View(aspNetUsers);
        }

        // GET: TestAspNetUsers1/Create
        public IActionResult Create()
        {
            ViewData["IdAbonnement"] = new SelectList(_context.Abonnement, "IdAbonnement", "NomAbonnement");
            ViewData["IdGenre"] = new SelectList(_context.Genre, "IdGenre", "NomGenre");
            ViewData["IdGrade"] = new SelectList(_context.Grade, "IdGrade", "NomGrade");
            ViewData["IdVille"] = new SelectList(_context.Ville, "IdVille", "NomVille");
            return View();
        }

        // POST: TestAspNetUsers1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdGrade,IdAbonnement,IdVille,IdGenre,NomAspnetuser,PrenomAspnetuser,UserName,IsactiveAspnetuser,IsprivateAspnetuser,AdresseAspnetuser,DatecreationAspnetuser,NbpointAspnetuser,NumcarteAspnetuser,CvvcarteAspnetuser,ExpirationcarteAspnetuser,CreditAspnetuser,IbanAspnetuser,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] AspNetUsers aspNetUsers)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aspNetUsers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdAbonnement"] = new SelectList(_context.Abonnement, "IdAbonnement", "NomAbonnement", aspNetUsers.IdAbonnement);
            ViewData["IdGenre"] = new SelectList(_context.Genre, "IdGenre", "NomGenre", aspNetUsers.IdGenre);
            ViewData["IdGrade"] = new SelectList(_context.Grade, "IdGrade", "NomGrade", aspNetUsers.IdGrade);
            ViewData["IdVille"] = new SelectList(_context.Ville, "IdVille", "NomVille", aspNetUsers.IdVille);
            return View(aspNetUsers);
        }

        // GET: TestAspNetUsers1/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspNetUsers = await _context.AspNetUsers.FindAsync(id);
            if (aspNetUsers == null)
            {
                return NotFound();
            }
            ViewData["IdAbonnement"] = new SelectList(_context.Abonnement, "IdAbonnement", "NomAbonnement", aspNetUsers.IdAbonnement);
            ViewData["IdGenre"] = new SelectList(_context.Genre, "IdGenre", "NomGenre", aspNetUsers.IdGenre);
            ViewData["IdGrade"] = new SelectList(_context.Grade, "IdGrade", "NomGrade", aspNetUsers.IdGrade);
            ViewData["IdVille"] = new SelectList(_context.Ville, "IdVille", "NomVille", aspNetUsers.IdVille);
            return View(aspNetUsers);
        }

        // POST: TestAspNetUsers1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,IdGrade,IdAbonnement,IdVille,IdGenre,NomAspnetuser,PrenomAspnetuser,UserName,IsactiveAspnetuser,IsprivateAspnetuser,AdresseAspnetuser,DatecreationAspnetuser,NbpointAspnetuser,NumcarteAspnetuser,CvvcarteAspnetuser,ExpirationcarteAspnetuser,CreditAspnetuser,IbanAspnetuser,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] AspNetUsers aspNetUsers)
        {
            if (id != aspNetUsers.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aspNetUsers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AspNetUsersExists(aspNetUsers.Id))
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
            ViewData["IdAbonnement"] = new SelectList(_context.Abonnement, "IdAbonnement", "NomAbonnement", aspNetUsers.IdAbonnement);
            ViewData["IdGenre"] = new SelectList(_context.Genre, "IdGenre", "NomGenre", aspNetUsers.IdGenre);
            ViewData["IdGrade"] = new SelectList(_context.Grade, "IdGrade", "NomGrade", aspNetUsers.IdGrade);
            ViewData["IdVille"] = new SelectList(_context.Ville, "IdVille", "NomVille", aspNetUsers.IdVille);
            return View(aspNetUsers);
        }

        // GET: TestAspNetUsers1/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspNetUsers = await _context.AspNetUsers
                .Include(a => a.IdAbonnementNavigation)
                .Include(a => a.IdGenreNavigation)
                .Include(a => a.IdGradeNavigation)
                .Include(a => a.IdVilleNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aspNetUsers == null)
            {
                return NotFound();
            }

            return View(aspNetUsers);
        }

        // POST: TestAspNetUsers1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var aspNetUsers = await _context.AspNetUsers.FindAsync(id);
            _context.AspNetUsers.Remove(aspNetUsers);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AspNetUsersExists(string id)
        {
            return _context.AspNetUsers.Any(e => e.Id == id);
        }
    }
}
