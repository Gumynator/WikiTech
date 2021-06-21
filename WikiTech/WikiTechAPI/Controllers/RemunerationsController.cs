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
    public class RemunerationsController : ControllerBase
    {
        private readonly WikiTechDBContext _context;

        public RemunerationsController(WikiTechDBContext context)
        {
            _context = context;
        }

        // GET: api/Remunerations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Remuneration>>> GetRemuneration()
        {
            return await _context.Remuneration.ToListAsync();
        }

        // GET: api/Remunerations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Remuneration>> GetRemuneration(int id)
        {
            var remuneration = await _context.Remuneration.FindAsync(id);

            if (remuneration == null)
            {
                return NotFound();
            }

            return remuneration;
        }

        // PUT: api/Remunerations/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRemuneration(int id, Remuneration remuneration)
        {
            if (id != remuneration.IdRemuneration)
            {
                return BadRequest();
            }

            _context.Entry(remuneration).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RemunerationExists(id))
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

        // POST: api/Remunerations
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Remuneration>> PostRemuneration(Remuneration remuneration)
        {
            _context.Remuneration.Add(remuneration);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRemuneration", new { id = remuneration.IdRemuneration }, remuneration);
        }

        // DELETE: api/Remunerations/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Remuneration>> DeleteRemuneration(int id)
        {
            var remuneration = await _context.Remuneration.FindAsync(id);
            if (remuneration == null)
            {
                return NotFound();
            }

            _context.Remuneration.Remove(remuneration);
            await _context.SaveChangesAsync();

            return remuneration;
        }

        private bool RemunerationExists(int id)
        {
            return _context.Remuneration.Any(e => e.IdRemuneration == id);
        }
    }
}
