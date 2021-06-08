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
        private const int NB_PER_PAGE = 5;

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

            return await _context.Article.Where(d => d.DatepublicationArticle == null).Include(p => p.IdNavigation).ToListAsync();
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



        //get article with date only **PAGINTATION **
        [HttpGet("nbtot")]
        public int GetArticleNbTotal()
        {
            return _context.Article.Where(d => d.IsactiveArticle == true).Count();
        }

        //get article with date only **PAGINTATION **
        [HttpGet("nbtotbysearch")]
        public int GetArticleNbTotalBySearch(String search)
        {
            return _context.Article.Where(d => d.IsactiveArticle == true).Where(s => s.TitreArticle.Contains(search)).Count();
        }

        //get article with date only **PAGINTATION **
        [HttpGet("pagetot")]
        public int GetPageNbTotal()
        {

            int nbtot = _context.Article.Where(d => d.IsactiveArticle == true).Count();
            int nbpage;

            if (nbtot % NB_PER_PAGE > 0)
            {
                nbpage = nbtot / NB_PER_PAGE + 1;
            }
            else
            {
                nbpage = nbtot / NB_PER_PAGE;
            }

            return nbpage;
        }

        [HttpGet("pagetotbysearch")]
        public int GetPageNbTotalBySearch(String search)
        {

            int nbtot = _context.Article.Where(d => d.IsactiveArticle == true).Where(s => s.TitreArticle.Contains(search)).Count();
            int nbpage;

            if (nbtot % NB_PER_PAGE > 0)
            {
                nbpage = nbtot / NB_PER_PAGE + 1;
            }
            else
            {
                nbpage = nbtot / NB_PER_PAGE;
            }

            return nbpage;
        }


        //get article with date only **PAGINTATION **
        [HttpGet("test/{nbPage}")]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticletest(int nbPage)
        {
            int nbPerPage = NB_PER_PAGE;

            if (nbPage > GetPageNbTotal())
            {
                nbPage = GetPageNbTotal();
            }
            if (nbPage < 1)
            {
                nbPage = 1;
            }

            //return await _context.Article.Where(d => d.DatepublicationArticle != null).Include(p => p.IdNavigation).ToListAsync();
            List<Article> rowArticles = await _context.Article.Where(d => d.IsactiveArticle == true).Include(p => p.IdNavigation).ToListAsync();

            if (nbPage * nbPerPage > GetArticleNbTotal())
            {
                nbPerPage -= nbPage * nbPerPage - GetArticleNbTotal();
            }

            List<Article> subListArticles = rowArticles.GetRange((nbPage - 1) * nbPerPage, nbPerPage);


            return subListArticles;
        }

        //get article with date only **Searching by titre**
        [HttpGet("testing/{nbPage}/{chainetest}")]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticletesting(int nbPage, string chainetest = "") //="" have by default
        {

            string sortorder = chainetest.Substring(0, 4); //ordre de sorting
            string searchString = chainetest.Substring(4); //recherche
            int nbPerPage = NB_PER_PAGE;

            List<Article> ListeArticle;
            List<Article> rowArticles;


            if (searchString.Length > 0)
            {
                //*************ARTICLES+RECHERCHE
                ListeArticle = await _context.Article.Where(d => d.IsactiveArticle == true).Where(s => s.TitreArticle.Contains(searchString)).Include(p => p.IdNavigation).ToListAsync();
                //*************ARTICLES+RECHERCHE
            }
            else
            {
                //*************ARTICLES
                ListeArticle = await _context.Article.Where(d => d.IsactiveArticle == true).Include(p => p.IdNavigation).ToListAsync();
                //*************ARTICLES
            }


            //*************SORTAGE
            IEnumerable<Article> intermediateListeArticle = ListeArticle;

            switch (sortorder)
            {
                case "ztoa":
                    intermediateListeArticle = intermediateListeArticle.OrderByDescending(s => s.TitreArticle);
                    break;
                case "atoz":
                    intermediateListeArticle = intermediateListeArticle.OrderBy(s => s.TitreArticle);
                    break;
                case "desc":
                    intermediateListeArticle = intermediateListeArticle.OrderByDescending(s => s.DatepublicationArticle);
                    break;
                case "dasc":
                    intermediateListeArticle = intermediateListeArticle.OrderBy(s => s.DatepublicationArticle);
                    break;
                default:
                    intermediateListeArticle = intermediateListeArticle.OrderBy(s => s.TitreArticle);
                    break;
            }

            ListeArticle = intermediateListeArticle.ToList();
            //*************SORTAGE


            if (searchString.Length > 0)
            {
                //*************PAGINATION+SEARCH
                if (nbPage > GetPageNbTotalBySearch(searchString))
                {
                    nbPage = GetPageNbTotalBySearch(searchString);
                }
                if (nbPage < 1)
                {
                    nbPage = 1;
                }

                if (nbPage * nbPerPage > GetArticleNbTotalBySearch(searchString))
                {
                    nbPerPage -= nbPage * nbPerPage - GetArticleNbTotalBySearch(searchString);
                }

                rowArticles = ListeArticle.GetRange((nbPage - 1) * nbPerPage, nbPerPage);

                //*************PAGINATION+SEARCH
            }
            else
            {
                //*************PAGINATION
                if (nbPage > GetPageNbTotal())
                {
                    nbPage = GetPageNbTotal();
                }
                if (nbPage < 1)
                {
                    nbPage = 1;
                }

                if (nbPage * nbPerPage > GetArticleNbTotal())
                {
                    nbPerPage -= nbPage * nbPerPage - GetArticleNbTotal();
                }

                rowArticles = ListeArticle.GetRange((nbPage - 1) * nbPerPage, nbPerPage);
            }


            return rowArticles;
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



        [HttpPost("{id}/disable")]
        public async Task<ActionResult<Article>> DisableArticle(int id, [FromBody] int _id)
        {

            Article articleToModify = _context.Article.Find(id);


            if (articleToModify == null)
            {
                return NotFound();
            }

            articleToModify.IsactiveArticle = false;
            await _context.SaveChangesAsync();

            return articleToModify;
        }

        [HttpPost("{id}/enable")]
        public async Task<ActionResult<Article>> enableArticle(int id, [FromBody] int _id)
        {

            Article articleToModify = _context.Article.Find(id);


            if (articleToModify == null)
            {
                return NotFound();
            }

            articleToModify.IsactiveArticle = true;
            await _context.SaveChangesAsync();

            return articleToModify;
        }

    }
}
