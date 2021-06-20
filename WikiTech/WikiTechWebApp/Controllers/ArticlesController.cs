
//Auteur    : Loris habegger
//Date      : 01.05.2021
//Fichier   : ArticlesController.cs


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WikiTechAPI.Models;
using WikiTechAPI.ViewModels;
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
       public IActionResult Index()
        {

            IEnumerable<Article> artList;

            try
            {

                HttpResponseMessage response = client.GetAsync("Articles/withdate").Result;
                artList = response.Content.ReadAsAsync<IEnumerable<Article>>().Result;
                return View(artList);

            }
            catch (ExceptionLiaisonApi e)
            {
                Console.WriteLine(e.getMessage());
                return Redirect("/Home/Index");
            }
            

        }

        // GET: ArticlesController to restor
        public async Task<IActionResult> DiscussionAsync(int id)
        {

            IEnumerable<MessageByArticle> listMessage;
            HttpResponseMessage getMessages = await client.GetAsync("Messages/GetMessageByIdArticle/" + id);
            listMessage = getMessages.Content.ReadAsAsync<IEnumerable<MessageByArticle>>().Result;
            ViewBag.idArticle = id;


            return View(listMessage);
        }

        public ViewResult WriteMessage(int id)
        {

            ViewBag.idArticle = id;


            return View();
        }

        public async Task<IActionResult> SendMessageAsync(int idArticle, string userMessage)
        {
            string IdUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Message message = new Message();
            message.CorpsMessage = userMessage;
            message.DateMessage = DateTime.Today;
            message.Id = IdUser;
            message.IdArticle = idArticle;
            HttpResponseMessage posdtMessage = await client.PostAsJsonAsync("Messages/PostMessage/", message);

            return View();
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
                ViewBag.id = idArt;

                dynamicmodel.Article = article;

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

        /*
        protected void Like(object sender, EventArgs e)
        {
            Console.Write("like");
        }

        protected void Dislike(object sender, EventArgs e)
        {
            Console.Write("dislike");
        }
        */

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

                    using var response = await client.PostAsync(ConfigureHttpClient.apiUrl + "Changements", content);
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



    }
}
