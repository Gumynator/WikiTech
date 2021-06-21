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
    public class AppreciersController : ControllerBase
    {
        private readonly WikiTechDBContext _context;

        public AppreciersController(WikiTechDBContext context)
        {
            _context = context;
        }

        // GET: api/Appreciers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Apprecier>>> GetApprecier()
        {
            return await _context.Apprecier.ToListAsync();
        }

        // GET: api/Appreciers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Apprecier>> GetApprecier(string id)
        {
            var apprecier = await _context.Apprecier.FindAsync(id);

            if (apprecier == null)
            {
                return NotFound();
            }

            return apprecier;
        }

        // PUT: api/Appreciers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApprecier(string id, Apprecier apprecier)
        {
            if (id != apprecier.Id)
            {
                return BadRequest();
            }

            _context.Entry(apprecier).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApprecierExists(id))
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

        // POST: api/Appreciers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Apprecier>> PostApprecier([FromBody]String Id, int IdArticle)
        {
            Apprecier apprecier =null;
            apprecier.Id = Id;
            apprecier.IdArticle = IdArticle;
            _context.Apprecier.Add(apprecier);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ApprecierExists(apprecier.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetApprecier", new { id = apprecier.Id }, apprecier);
        }

        // DELETE: api/Appreciers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Apprecier>> DeleteApprecier(string id)
        {
            var apprecier = await _context.Apprecier.FindAsync(id);
            if (apprecier == null)
            {
                return NotFound();
            }

            _context.Apprecier.Remove(apprecier);
            await _context.SaveChangesAsync();

            return apprecier;
        }

        private bool ApprecierExists(string id)
        {
            return _context.Apprecier.Any(e => e.Id == id);
        }
    }
}
