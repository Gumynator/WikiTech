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
    public class AspNetUserRolesController : ControllerBase
    {
        private readonly WikiTechDBContext _context;

        public AspNetUserRolesController(WikiTechDBContext context)
        {
            _context = context;
        }

        // GET: api/AspNetUserRoles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AspNetUserRoles>>> GetAspNetUserRoles()
        {
            return await _context.AspNetUserRoles.ToListAsync();
        }

        // GET: api/AspNetUserRoles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AspNetUserRoles>> GetAspNetUserRoles(string id)
        {
            var aspNetUserRoles = await _context.AspNetUserRoles.FindAsync(id);

            if (aspNetUserRoles == null)
            {
                return NotFound();
            }

            return aspNetUserRoles;
        }

        // PUT: api/AspNetUserRoles/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAspNetUserRoles(string id, AspNetUserRoles aspNetUserRoles)
        {
            if (id != aspNetUserRoles.UserId)
            {
                return BadRequest();
            }

            _context.Entry(aspNetUserRoles).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AspNetUserRolesExists(id))
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

        // POST: api/AspNetUserRoles
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<AspNetUserRoles>> PostAspNetUserRoles(AspNetUserRoles aspNetUserRoles)
        {
            _context.AspNetUserRoles.Add(aspNetUserRoles);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AspNetUserRolesExists(aspNetUserRoles.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAspNetUserRoles", new { id = aspNetUserRoles.UserId }, aspNetUserRoles);
        }

        // DELETE: api/AspNetUserRoles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AspNetUserRoles>> DeleteAspNetUserRoles(string id)
        {
            var aspNetUserRoles = await _context.AspNetUserRoles.FindAsync(id);
            if (aspNetUserRoles == null)
            {
                return NotFound();
            }

            _context.AspNetUserRoles.Remove(aspNetUserRoles);
            await _context.SaveChangesAsync();

            return aspNetUserRoles;
        }

        private bool AspNetUserRolesExists(string id)
        {
            return _context.AspNetUserRoles.Any(e => e.UserId == id);
        }
    }
}
