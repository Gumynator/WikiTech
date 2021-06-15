using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WikiTechAPI.Models;

namespace WikiTechAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostulationsController : ControllerBase
    {
        private readonly WikiTechDBContext _context;
        string rolesArray;

        public PostulationsController(WikiTechDBContext context)
        {

            _context = context;
          
            
        }

        

        // GET: api/Postulations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Postulation>>> GetPostulation()
        {
            
            // Resolve the user via their email
            var user = await _userManager.FindByEmailAsync(model.Email);
            // Get the roles for the user
            var roles = await _userManager.GetRolesAsync(user);
            return await _context.Postulation.Include(p => p.IdNavigation).ToListAsync();
        }

        // GET: api/Postulations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Postulation>> GetPostulation(int id)
        {
            var postulation = await _context.Postulation.FindAsync(id);

            if (postulation == null)
            {
                return NotFound();
            }

            return postulation;
        }

        // PUT: api/Postulations/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPostulation(int id, Postulation postulation)
        {
            if (id != postulation.IdPostulation)
            {
                return BadRequest();
            }

            _context.Entry(postulation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostulationExists(id))
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

        // POST: api/Postulations
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Postulation>> PostPostulation(Postulation postulation)
        {
            _context.Postulation.Add(postulation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPostulation", new { id = postulation.IdPostulation }, postulation);
        }

        // DELETE: api/Postulations/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Postulation>> DeletePostulation(int id)
        {
            var postulation = await _context.Postulation.FindAsync(id);
            if (postulation == null)
            {
                return NotFound();
            }

            _context.Postulation.Remove(postulation);
            await _context.SaveChangesAsync();

            return postulation;
        }

        private bool PostulationExists(int id)
        {
            return _context.Postulation.Any(e => e.IdPostulation == id);
        }
    }
}
