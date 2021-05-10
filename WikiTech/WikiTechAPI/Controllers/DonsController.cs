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
    public class DonsController : ControllerBase
    {
        private readonly WikiTechDBContext _context;

        public DonsController(WikiTechDBContext context)
        {
            _context = context;
        }




        // GET: api/Dons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Don>>> GetDon()
        {
            return await _context.Don.ToListAsync();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Don>>> GetDonByUserId(int id)
        {

            return await _context.Don.ToListAsync();
        }

        //// GET: api/Dons/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Don>> GetDon(int id)
        //{
        //    var don = await _context.Don.FindAsync(id);

        //    if (don == null)
        //    {
        //        return NotFound();
        //    }

        //    return don;
        //}

        // PUT: api/Dons/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDon(int id, Don don)
        {
            if (id != don.IdDon)
            {
                return BadRequest();
            }

            _context.Entry(don).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DonExists(id))
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

        // POST: api/Dons
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Don>> PostDon(Don don)
        {
            _context.Don.Add(don);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DonExists(don.IdDon))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDon", new { id = don.IdDon }, don);
        }

        // DELETE: api/Dons/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Don>> DeleteDon(int id)
        {
            var don = await _context.Don.FindAsync(id);
            if (don == null)
            {
                return NotFound();
            }

            _context.Don.Remove(don);
            await _context.SaveChangesAsync();

            return don;
        }

        private bool DonExists(int id)
        {
            return _context.Don.Any(e => e.IdDon == id);
        }
    }
}
