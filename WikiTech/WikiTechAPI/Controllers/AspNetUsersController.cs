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
    public class AspNetUsersController : ControllerBase
    {
        private readonly WikiTechDBContext _context;

        public AspNetUsersController(WikiTechDBContext context)
        {
            _context = context;
        }

        // GET: api/AspNetUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AspNetUsers>>> GetAspNetUsers()
        {
            return await _context.AspNetUsers.ToListAsync();
        }



        // GET: api/AspNetUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AspNetUsers>> GetAspNetUsers(string id)
        {
            var aspNetUsers = await _context.AspNetUsers.FindAsync(id);

            if (aspNetUsers == null)
            {
                return NotFound();
            }

            return aspNetUsers;
        }




        // PUT: api/AspNetUsers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Route("AspNetUsers/{id}")]
        public async Task<IActionResult> PutAspNetUsers(string id, AspNetUsers aspNetUsers)
        {
            if (id != aspNetUsers.Id)
            {
                return BadRequest();
            }

            _context.Entry(aspNetUsers).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AspNetUsersExists(id))
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

        //used in functionAPI
        //from body to force search parameter in the body and not in the URL
        [HttpPut("{id}")]
        [Route("addpoint/{id}")]
        public async Task<IActionResult> AddPointAspnNetUser (string id, [FromBody] int nbPointToAdd)
        {
            var currentAspNetUser = await _context.AspNetUsers.FindAsync(id);
            currentAspNetUser.NbpointAspnetuser += nbPointToAdd;

            Grade currentGrade = _context.Grade.Where(p => currentAspNetUser.NbpointAspnetuser >= p.MinpointGrade).OrderByDescending(p => p.MinpointGrade).FirstOrDefault();

            if (currentAspNetUser.IdGrade != currentGrade.IdGrade)
            {
                currentAspNetUser.IdGrade = currentGrade.IdGrade;
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        //used in functionAPI
        //from body to force search parameter in the body and not in the URL
        [HttpPut("{id}")]
        [Route("addcredit/{id}")]
        public async Task<IActionResult> AddCreditAspnNetUser(string id, int Credit)
        {
            var currentAspNetUser = await _context.AspNetUsers.FindAsync(id);
            currentAspNetUser.CreditAspnetuser += Credit;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/AspNetUsers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<AspNetUsers>> PostAspNetUsers(AspNetUsers aspNetUsers)
        {
            _context.AspNetUsers.Add(aspNetUsers);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AspNetUsersExists(aspNetUsers.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAspNetUsers", new { id = aspNetUsers.Id }, aspNetUsers);
        }

        // DELETE: api/AspNetUsers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AspNetUsers>> DeleteAspNetUsers(string id)
        {
            var aspNetUsers = await _context.AspNetUsers.FindAsync(id);
            if (aspNetUsers == null)
            {
                return NotFound();
            }

            _context.AspNetUsers.Remove(aspNetUsers);
            await _context.SaveChangesAsync();

            return aspNetUsers;
        }

        private bool AspNetUsersExists(string id)
        {
            return _context.AspNetUsers.Any(e => e.Id == id);
        }
    }
}
