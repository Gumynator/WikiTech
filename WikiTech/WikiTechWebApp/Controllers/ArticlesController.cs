
//Auteur    : Loris habegger
//Date      : 01.05.2021
//Fichier   : ArticlesController.cs


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WikiTechAPI.Models;
using WikiTechWebApp.ApiFunctions;
using WikiTechWebApp.Exceptions;

namespace WikiTechWebApp.Controllers
{
    public class ArticlesController : Controller
    {

        static HttpClient client = new HttpClient();
    

        public ArticlesController()
        {
            client = ConfigureHttpClient.configureHttpClient(client);

        }


        // GET: ArticlesController to restor
        public IActionResult Index(string sortorder, string searchString, int currentPage, int idTag) //passage des super paramètre
        {
            if (currentPage == 0)
            {
                currentPage = 1;
            }
            if (sortorder == null)
            {
                sortorder = "atoz";

            }
            if (searchString == null)
            {
                searchString = "";

            }


            IEnumerable<Article> artList;

            //envoie des données à le vue
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentPage"] = currentPage;
            ViewData["sortorder"] = sortorder;
            ViewData["currentTag"] = idTag;

            string chainetest = sortorder + searchString;

            try
            {

                HttpResponseMessage response = client.GetAsync("Articles/testing/" + currentPage + "/" + chainetest + "/" + idTag).Result;
                artList = response.Content.ReadAsAsync<IEnumerable<Article>>().Result;

                HttpResponseMessage respage = client.GetAsync("Articles/nbtot").Result;
                int nbArtTotal = respage.Content.ReadAsAsync<int>().Result;

                ViewData["nbArtTotal"] = nbArtTotal;

             
                return View(artList);

            }
            catch (ExceptionLiaisonApi e)
            {
                Console.WriteLine(e.getMessage());
                return Redirect("/Home/Index");
            }

        }


        public IActionResult AllArticle()
        {
            IEnumerable<Article> artList;

            string IdUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Grade grade = null;
            if (IdUser != null)
            {
                var currentUser = FunctionAPI.GetUserByIdAsync(client, IdUser).Result;
                grade = FunctionAPI.GetGradesForUser(client, currentUser.IdGrade).Result;
            }

            if (IdUser == null || grade.NomGrade != "user sup")
            {
                return RedirectToAction("Unauthorize", "Home");
            }

            try
            {

                HttpResponseMessage response = client.GetAsync("Articles").Result;
                artList = response.Content.ReadAsAsync<IEnumerable<Article>>().Result;
                return View(artList);

            }
            catch (ExceptionLiaisonApi e)
            {
                Console.WriteLine(e.getMessage());
                return Redirect("/Home/Index");
            }

        }


        // GET: ArticleController/Details/5
        public ActionResult DetailsAsync(int id)
        {

            //get article with user of article
            Article article;
            Changement changement;
            dynamic dynamicmodel = new ExpandoObject(); //for return multiple 1 object with multiple object


            try
            {
                HttpResponseMessage responsearticle = client.GetAsync("Articles/" + id).Result;
                article = responsearticle.Content.ReadAsAsync<Article>().Result;
                var idArt = article.Id;
                var idArticle = article.IdArticle;
                ViewBag.id = idArt;

                dynamicmodel.Article = article;

                string IdUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
                

                Voir vu = new Voir();
                vu.Id = IdUser;
                vu.IdArticle = idArticle;
                vu.Isread = true;
                var postTask = client.PostAsJsonAsync<Voir>("Voirs", vu);
                postTask.Wait();
                var result = postTask.Result;


                var listeChangements = article.Changement;

                List<Changement> resultatChangement = new List<Changement>();

                foreach (var item in listeChangements)
                {


                    HttpResponseMessage responsechangement = client.GetAsync("Changements/" + item.IdChangement).Result;
                    changement = responsechangement.Content.ReadAsAsync<Changement>().Result;

                    resultatChangement.Add(changement);

                }



                dynamicmodel.Changement = resultatChangement;


                return View(dynamicmodel);

            }
            catch (ExceptionLiaisonApi e)
            {
                Console.WriteLine(e.getMessage());
                return Redirect("/Home/Index");
            }

        }

        // GET: ArticleController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ArticleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ArticleController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ArticleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ArticleController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ArticleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        [Authorize]
        // GET: the article detail for decision
        public ActionResult PropositionModification(int id)
        {

            Article article;

            try
            {
                HttpResponseMessage responsearticle = client.GetAsync("Articles/" + id).Result;
                article = responsearticle.Content.ReadAsAsync<Article>().Result;


                return View(article);
                

            }
            catch (ExceptionLiaisonApi e)
            {
                Console.WriteLine(e.getMessage());
                return Redirect("/Home/Index");
            }
        }

        [Authorize]
        [HttpPost]
        // GET: the article detail for decision
        public async Task<ActionResult> sendModificationAsync(int idArticle)
        {

            Changement currentchangement = new Changement();

            currentchangement.IdArticle = Int32.Parse(Request.Form["IdArticle"]);
            currentchangement.TextChangement = Request.Form["TextArticle"];
            currentchangement.DescriptionChangement = Request.Form["DescriptionArticle"];
            currentchangement.TitreChangement = Request.Form["TitreArticle"];
            currentchangement.Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            currentchangement.ResumeChangement = Request.Form["ResumeChangement"];

            if (ModelState.IsValid)
            {
                
                try
                {
                    Changement resultChangement;
                    StringContent content = new StringContent(JsonConvert.SerializeObject(currentchangement), Encoding.UTF8, "application/json");

                    using var response = await client.PostAsync(ConfigureHttpClient.apiUrl + "Changements" + "/" + currentchangement.Id, content);
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    resultChangement = JsonConvert.DeserializeObject<Changement>(apiResponse);


                    return Redirect("/Articles/Index");

                }
                catch (ExceptionLiaisonApi e)
                {
                    Console.WriteLine(e.getMessage());
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }


        [Authorize]
        // GET: the article detail for decision
        public async Task<ActionResult> disableArticleeeeee(int id)
        {

            Article article;
            string currentid = User.FindFirstValue(ClaimTypes.NameIdentifier);

            string IdUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Grade grade = null;
            if (IdUser != null)
            {
                var currentUser = FunctionAPI.GetUserByIdAsync(client, IdUser).Result;
                grade = FunctionAPI.GetGradesForUser(client, currentUser.IdGrade).Result;
            }

            if (IdUser == null || grade.NomGrade != "user sup")
            {
                return RedirectToAction("Unauthorize", "Home");
            }

            try
            {

                Article resultarticle;

                using var response = await client.PostAsJsonAsync(ConfigureHttpClient.apiUrl + "Articles/" +  id + "/disable", currentid);
                string apiResponse = await response.Content.ReadAsStringAsync();

                resultarticle = JsonConvert.DeserializeObject<Article>(apiResponse);


                return RedirectToAction(nameof(Index));


            }
            catch (ExceptionLiaisonApi e)
            {
                Console.WriteLine(e.getMessage());
                return RedirectToAction(nameof(Index));
            }
        }

        [Authorize]
        // GET: the article detail for decision
        public async Task<ActionResult> enableArticleeeee(int id)
        {
            string IdUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Grade grade = null;
            if (IdUser != null)
            {
                var currentUser = FunctionAPI.GetUserByIdAsync(client, IdUser).Result;
                grade = FunctionAPI.GetGradesForUser(client, currentUser.IdGrade).Result;
            }

            if (IdUser == null || grade.NomGrade != "user sup")
            {
                return RedirectToAction("Unauthorize", "Home");
            }
            Article article;
            string currentid = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {

                Article resultarticle;

                using var response = await client.PostAsJsonAsync(ConfigureHttpClient.apiUrl + "Articles/" + id + "/enable", currentid);
                string apiResponse = await response.Content.ReadAsStringAsync();

                resultarticle = JsonConvert.DeserializeObject<Article>(apiResponse);


                return RedirectToAction(nameof(Index));


            }
            catch (ExceptionLiaisonApi e)
            {
                Console.WriteLine(e.getMessage());
                return RedirectToAction(nameof(Index));
            }
        }
        
    }
}
