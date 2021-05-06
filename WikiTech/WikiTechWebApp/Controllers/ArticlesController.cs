
//Auteur    : Loris habegger
//Date      : 01.05.2021
//Fichier   : ArticlesController.cs


using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

        // GET: ArticlesController
        public IActionResult Index()
        {

            IEnumerable<Article> artList;

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
    }
}
