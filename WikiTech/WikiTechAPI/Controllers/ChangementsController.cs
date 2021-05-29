//Auteur    : Loris habegger
//Date      : 18.05.2021
//Fichier   : ChangementController.cs

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
    public class ChangementsController : ControllerBase
    {
        private readonly WikiTechDBContext _context;

        public ChangementsController(WikiTechDBContext context)
        {
            _context = context;
        }

        // GET: api/Changements
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Changement>>> GetChangement()
        {
            return await _context.Changement.Include(p => p.IdNavigation).ToListAsync();
        }

        // GET: api/Changements/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Changement>> GetChangement(int id)
        {
            var changement = await _context.Changement.Include(p => p.IdNavigation).FirstOrDefaultAsync(i => i.IdChangement == id);

            if (changement == null)
            {
                return NotFound();
            }

            return changement;
        }

        //get change with no approbation date
        [HttpGet]
        [Route("nodate")]
        public async Task<ActionResult<IEnumerable<Changement>>> GetchangeNoDate()
        {

            return await _context.Changement.Where(d => d.DatepublicationChangement == null).Include(p => p.IdNavigation).ToListAsync();
        }

        //get change with date only
        [HttpGet]
        [Route("withdate")]
        public async Task<ActionResult<IEnumerable<Changement>>> GetchangeWithDate()
        {

            return await _context.Changement.Where(d => d.DatepublicationChangement != null).Include(p => p.IdNavigation).ToListAsync();
        }

        // PUT: api/Changements/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChangement(int id, [FromBody] Changement changement)
        {
            if (id != changement.IdChangement)
            {
                return BadRequest();
            }

            _context.Entry(changement).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                Article articleToModify = _context.Article.Find(changement.IdArticle);

                articleToModify.TitreArticle = changement.TitreChangement;
                articleToModify.TextArticle = changement.TextChangement;
                articleToModify.DescriptionArticle = changement.DescriptionChangement;

                await _context.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChangementExists(id))
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

        // POST: api/Changements
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Changement>> PostChangement(Changement changement)
        {
            _context.Changement.Add(changement);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChangement", new { id = changement.IdChangement }, changement);
        }

        // DELETE: api/Changements/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Changement>> DeleteChangement(int id)
        {
            var changement = await _context.Changement.FindAsync(id);
            if (changement == null)
            {
                return NotFound();
            }

            _context.Changement.Remove(changement);
            await _context.SaveChangesAsync();

            return changement;
        }

        private bool ChangementExists(int id)
        {
            return _context.Changement.Any(e => e.IdChangement == id);
        }
    }
}
