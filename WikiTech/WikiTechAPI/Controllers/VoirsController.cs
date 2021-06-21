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
    public class VoirsController : ControllerBase
    {
        private readonly WikiTechDBContext _context;

        public VoirsController(WikiTechDBContext context)
        {
            _context = context;
        }

        // GET: api/Voirs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Voir>>> GetVoir()
        {
            return await _context.Voir.ToListAsync();
        }

        // GET: api/Voirs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Voir>> GetVoir(string id)
        {
            var voir = await _context.Voir.FindAsync(id);

            if (voir == null)
            {
                return NotFound();
            }

            return voir;
        }

        // PUT: api/Voirs/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVoir(string id, Voir voir)
        {
            if (id != voir.Id)
            {
                return BadRequest();
            }

            _context.Entry(voir).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VoirExists(id))
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

        // POST: api/Voirs
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Voir>> PostVoir(Voir voir)
        {
            _context.Voir.Add(voir);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (VoirExists(voir.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetVoir", new { id = voir.Id }, voir);
        }

        // DELETE: api/Voirs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Voir>> DeleteVoir(string id)
        {
            var voir = await _context.Voir.FindAsync(id);
            if (voir == null)
            {
                return NotFound();
            }

            _context.Voir.Remove(voir);
            await _context.SaveChangesAsync();

            return voir;
        }

        private bool VoirExists(string id)
        {
            return _context.Voir.Any(e => e.Id == id);
        }
    }
}
