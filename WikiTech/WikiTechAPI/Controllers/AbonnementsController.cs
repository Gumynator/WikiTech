using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WikiTechAPI.Models;

namespace WikiTechAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AbonnementsController : ControllerBase
    {
        private readonly WikiTechDBContext _context;

        public AbonnementsController(WikiTechDBContext context)
        {
            _context = context;
        }

        // GET: api/Abonnements
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Abonnement>>> GetAbonnement()
        {
            return await _context.Abonnement.ToListAsync();
        }

        // GET: api/Abonnements/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Abonnement>> GetAbonnement(int id)
        {
            var abonnement = await _context.Abonnement.FindAsync(id);

            if (abonnement == null)
            {
                return NotFound();
            }

            return abonnement;
        }

        // PUT: api/Abonnements/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAbonnement(int id, Abonnement abonnement)
        {
            if (id != abonnement.IdAbonnement)
            {
                return BadRequest();
            }

            _context.Entry(abonnement).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AbonnementExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Abonnements
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Abonnement>> PostAbonnement(Abonnement abonnement)
        {
            _context.Abonnement.Add(abonnement);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAbonnement", new { id = abonnement.IdAbonnement }, abonnement);
        }

        // DELETE: api/Abonnements/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Abonnement>> DeleteAbonnement(int id)
        {
            var abonnement = await _context.Abonnement.FindAsync(id);
            if (abonnement == null)
            {
                return NotFound();
            }

            _context.Abonnement.Remove(abonnement);
            await _context.SaveChangesAsync();

            return abonnement;
        }

        private bool AbonnementExists(int id)
        {
            return _context.Abonnement.Any(e => e.IdAbonnement == id);
        }
    }
}
