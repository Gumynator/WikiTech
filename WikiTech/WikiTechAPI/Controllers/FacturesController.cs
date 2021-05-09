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
    public class FacturesController : ControllerBase
    {
        private readonly WikiTechDBContext _context;

        public FacturesController(WikiTechDBContext context)
        {
            _context = context;
        }

        // GET: api/Factures
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Facture>>> GetFacture()
        {
            return await _context.Facture.ToListAsync();
        }

        // GET: api/Factures/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Facture>> GetFacture(int id)
        {
            var facture = await _context.Facture.FindAsync(id);

            if (facture == null)
            {
                return NotFound();
            }

            return facture;
        }

        // PUT: api/Factures/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFacture(int id, Facture facture)
        {
            if (id != facture.IdFacture)
            {
                return BadRequest();
            }

            _context.Entry(facture).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FactureExists(id))
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

        // POST: api/Factures
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Facture>> PostFacture(Facture facture)
        {
            _context.Facture.Add(facture);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFacture", new { id = facture.IdFacture }, facture);
        }

        // DELETE: api/Factures/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Facture>> DeleteFacture(int id)
        {
            var facture = await _context.Facture.FindAsync(id);
            if (facture == null)
            {
                return NotFound();
            }

            _context.Facture.Remove(facture);
            await _context.SaveChangesAsync();

            return facture;
        }

        private bool FactureExists(int id)
        {
            return _context.Facture.Any(e => e.IdFacture == id);
        }
    }
}
