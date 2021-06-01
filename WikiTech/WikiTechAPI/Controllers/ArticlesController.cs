//Auteur    : Loris habegger
//Date      : 05.05.2021
//Fichier   : ArticlesController.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WikiTechAPI.Models;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using System.Text.Json.Serialization;

namespace WikiTechAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly WikiTechDBContext _context;

        public ArticlesController(WikiTechDBContext context)
        {
            _context = context;
        }

        // GET: api/Articles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticle()
        {
            //return await _context.Article.ToListAsync();

            
            return await _context.Article.Include(p => p.IdNavigation).ToListAsync();
        }

        // GET: api/Articles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Article>> GetArticle(int id)
        {

            var article = await _context.Article.Include(p => p.IdNavigation).Include(r => r.Referencer).Include(c => c.Changement).FirstOrDefaultAsync(i => i.IdArticle == id);


            if (article == null)
            {
                return NotFound();
            }

            return article;
        }

        //get article with no approbation date
        [HttpGet]
        [Route("toactive")]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticleNoActivated()
        {

            return await _context.Article.Where(d => d.IsactiveArticle == false).Include(p => p.IdNavigation).ToListAsync();
        }

        [HttpGet("_id")]
        [Route("byuser")]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticlebyUser(string _id)
        {
     
            return await _context.Article.Where(d => d.Id == _id).ToListAsync();
        }
       
        //get article with date only
        [HttpGet]
        [Route("beactive")]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticleIsActive()
        {
            return await _context.Article.Where(d => d.IsactiveArticle == true).Include(p => p.IdNavigation).ToListAsync();
        }


        // PUT: api/Articles/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArticle(int id, [FromBody]Article article)
        {
            if (id != article.IdArticle)
            {
                return BadRequest();
            }

            _context.Entry(article).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(id))
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

        // POST: api/Articles
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Article>> PostArticle(Article article)
        {
            _context.Article.Add(article);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArticle", new { id = article.IdArticle }, article);
        }

        // DELETE: api/Articles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Article>> DeleteArticle(int id)
        {
            var article = await _context.Article.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            _context.Article.Remove(article);
            await _context.SaveChangesAsync();

            return article;
        }

        private bool ArticleExists(int id)
        {
            return _context.Article.Any(e => e.IdArticle == id);
        }


        [HttpPut("{id}")]
        [Route("disable")]
        public async Task<ActionResult<Article>> DisableArticle(int id, [FromBody]Article article)
        {

            Article articleToModify = _context.Article.Find(article.IdArticle);


            if (article == null)
            {
                return NotFound();
            }

            articleToModify.IsactiveArticle = false;
            await _context.SaveChangesAsync();

            return article;
        }

    }
}
