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


        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Auteur : Pancini Marco
        //Création : 07.05.2021
        //Modification : 10.05.2021
        //Description : Fonction qui permet de récupérer les dons d'un utilisateur
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpGet("GetDonsByUserId/{userID}")]
        public async Task<ActionResult<IEnumerable<Don>>> GetDonsByUserId(string userID)
        {
            var query = (from don in await _context.Don.ToListAsync()
                         where userID.Equals(don.Id)
                         select don).ToList();
            if (query == null)
            {
                return NotFound();
            }

            return query;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Auteur : Pancini Marco
        //Création : 25.05.2021
        //Modification : 29.05.2021
        //Description : Fonction qui permet de récupérer les 20 premier dons par valeur décroissante
        //ainsi que le nom/prenom de l'utilisateur concerné
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpGet("GetAllDons/")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllDons()
        {
            var query = (from don in await _context.Don.ToListAsync()
                         join user in _context.AspNetUsers
                         on don.Id equals user.Id
                         orderby don.MontantDon descending
                         select new { MontantDon = don.MontantDon,
                             DateDon = don.DateDon,
                             NomAspnetuser = user.NomAspnetuser+ user.PrenomAspnetuser }).Take(20).ToList();
            if (query == null)
            {
                return NotFound();
            }

            return query;
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

        // GET: api/Dons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Don>> GetDon(int id)
        {
            var don = await _context.Don.FindAsync(id);

            if (don == null)
            {
                return NotFound();
            }

            return don;
        }

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


        //// POST: api/Dons
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for
        //// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPost]
        //public async Task<ActionResult<Don>> PostDon(Don don)
        //{
        //    _context.Don.Add(don);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetDon", new { id = don.IdDon }, don);
        //}

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
            catch (DbUpdateException e)
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
