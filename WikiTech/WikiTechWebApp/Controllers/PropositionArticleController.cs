using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
using WikiTechAPI.Models;
using WikiTechWebApp.ApiFunctions;
using Microsoft.AspNetCore.Hosting.Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Grpc.Core;
using Newtonsoft.Json;
using System.Text;

namespace WikiTechWebApp.Controllers
{
    public class PropositionArticleController : Controller
    {

        static HttpClient client = new HttpClient();

        public PropositionArticleController() 
        {

            client = ConfigureHttpClient.configureHttpClient(client);
        }

        //[Authorize]
        // GET: PropositionArticleController
        public ActionResult Index()
        {
            return View();
        }

        // GET: PropositionArticleController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PropositionArticleController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PropositionArticleController/Create
        [HttpPost("CreateProposition")]
        public async Task<ActionResult> Create([Bind("Id,TitreArticle,DescriptionArticle,TextArticle,IdSection,Referencer,IsqualityArticle")] Article _article)
        {




            Article currentArticle = _article;

            currentArticle.Id = "00221f02-bfdb-4607-9403-7168e260ea8a"; // sera récupéré quand loggé
            currentArticle.IdSection = int.Parse(Request.Form["rdsection"]);

            var listreference = currentArticle.Referencer.ToList();
            var Tags = Request.Form["tags"]; //.count compte les éléments à l'intérrieur

            if (ModelState.IsValid)
            {
                //Article article = await FunctionArticles.AddArticle(currentArticle); to passe trought another file

                Article resultarticle;

                StringContent content = new StringContent(JsonConvert.SerializeObject(currentArticle), Encoding.UTF8, "application/json");

                using var response = await client.PostAsync(ConfigureHttpClient.apiUrl + "Articles", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                resultarticle = JsonConvert.DeserializeObject<Article>(apiResponse);

                return Redirect(Url.Action("http://google.com"));
            }
            else
            {

                return RedirectToAction("Home/Index");
            }

        }

        // GET: PropositionArticleController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PropositionArticleController/Edit/5
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

        // GET: PropositionArticleController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PropositionArticleController/Delete/5
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

    }
}
