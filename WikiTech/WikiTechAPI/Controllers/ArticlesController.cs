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
using WikiTechAPI.Utility;

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
        public int GetArticleNbTotalBySearch(List<Article> listArticle)
        {
            return listArticle.Count();
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

        public int GetPageNbTotalBySearch(List<Article> listArticle)
        {

            int nbtot = listArticle.Count();
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
        [HttpGet("testing/{nbPage}/{chainetest}/{idTag}")]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticletesting(int nbPage, string chainetest, int idTag) //="" have by default
        {

            string sortorder = chainetest.Substring(0, 4); //ordre de sorting
            string searchString = chainetest.Substring(4); //recherche
            int nbPerPage = NB_PER_PAGE;

            List<Article> ListeArticle;
            List<Article> rowArticles;


            if (searchString.Length > 0)
            {
                //*************ARTICLES+RECHERCHE
                //check si un tag est setté
                if (idTag != 0)
                {
                    ListeArticle = await _context.Article
                        .Where(d => d.IsactiveArticle == true)
                        .Where(s => s.TitreArticle.Contains(searchString))
                        .Include(p => p.IdNavigation)
                        .Include(p => p.Referencer)
                        .Where(p => p.Referencer.Any(x => x.IdTag == idTag))
                        .ToListAsync();
                }
                else
                {
                    ListeArticle = await _context.Article.Where(d => d.IsactiveArticle == true).Where(s => s.TitreArticle.Contains(searchString)).Include(p => p.IdNavigation).ToListAsync();
                }
                //*************ARTICLES+RECHERCHE
            }
            else
            {
                //*************ARTICLES
                if (idTag != 0)
                {
                    ListeArticle = await _context.Article
                        .Where(d => d.IsactiveArticle == true)
                        .Include(p => p.IdNavigation)
                        .Include(p => p.Referencer)
                        .Where(p => p.Referencer.Any(x => x.IdTag == idTag))
                        .ToListAsync();
                }
                else
                {
                    ListeArticle = await _context.Article.Where(d => d.IsactiveArticle == true).Include(p => p.IdNavigation).ToListAsync();
                }
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

            //*************PAGINATION
            if (nbPage > GetPageNbTotalBySearch(ListeArticle))
            {
                nbPage = GetPageNbTotalBySearch(ListeArticle);
            }
            if (nbPage < 1)
            {
                nbPage = 1;
            }

            if (nbPage * nbPerPage > GetArticleNbTotalBySearch(ListeArticle))
            {
                nbPerPage -= nbPage * nbPerPage - GetArticleNbTotalBySearch(ListeArticle);
            }

            rowArticles = ListeArticle.GetRange((nbPage - 1) * nbPerPage, nbPerPage);
            //*************PAGINATION

            return rowArticles;
        }


        // PUT: api/Articles/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArticle(string id, [FromBody]Article article)
        {
           
            _context.Entry(article).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                
                    return NotFound();
                
            }

            Logwritter log = new Logwritter("ArticleID : " + article.IdArticle + " l'etat a été modifié par " + id);
            log.writeLog();

            return NoContent();
        }

        // POST: api/Articles
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("{idactionneur}")]
        public async Task<ActionResult<Article>> PostArticle(String idactionneur,[FromBody]Article article)
        {
            _context.Article.Add(article);
            await _context.SaveChangesAsync();

            Logwritter log = new Logwritter("ArticleID : " + article.IdArticle + " est ajouté par " + idactionneur);
            log.writeLog();

            return CreatedAtAction("GetArticle", new { id = article.IdArticle }, article);
        }

        // DELETE: api/Articles/5
        [HttpDelete("{id}/{idactionneur}")]
        public async Task<ActionResult<Article>> DeleteArticle(int id, string idactionneur)
        {
            var article = await _context.Article.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            _context.Article.Remove(article);
            await _context.SaveChangesAsync();

            Logwritter log = new Logwritter("ArticleID : " + id + " est supprimé par " + idactionneur);
            log.writeLog();

            return article;
        }

        private bool ArticleExists(int id)
        {
            return _context.Article.Any(e => e.IdArticle == id);
        }



        [HttpPost("{id}/disable")]
        public async Task<ActionResult<Article>> DisableArticle(int id, [FromBody]string idactionneur)
        {

            Article articleToModify = _context.Article.Find(id);


            if (articleToModify == null)
            {
                return NotFound();
            }

            articleToModify.IsactiveArticle = false;
            await _context.SaveChangesAsync();

            Logwritter log = new Logwritter("ArticleID : " + id + " est desactivé par " + idactionneur);
            log.writeLog();

            return articleToModify;
        }

        [HttpPost("{id}/enable")]
        public async Task<ActionResult<Article>> enableArticle(int id, [FromBody]string idactionneur)
        {

            Article articleToModify = _context.Article.Find(id);


            if (articleToModify == null)
            {
                return NotFound();
            }

            articleToModify.IsactiveArticle = true;
            await _context.SaveChangesAsync();

            Logwritter log = new Logwritter("ArticleID : " + id + " est activé par " + idactionneur);
            log.writeLog();

            return articleToModify;
        }

    }
}
