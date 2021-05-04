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
using System.Security.Claims;

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

            var idTags = Request.Form["tags"].ToList(); //.count compte les éléments à l'intérrieur

            var IdUser = User.FindFirstValue(ClaimTypes.NameIdentifier); // récupération de l'ID de l'utilisateur courrant

            currentArticle.Id = IdUser;
            currentArticle.IdSection = int.Parse(Request.Form["rdsection"]); // récupération de l'id de la section selectionnée

            if (ModelState.IsValid)
            {
                //Article article = await FunctionArticles.AddArticle(currentArticle); to passe trought another file

                Article resultarticle;
                StringContent content = new StringContent(JsonConvert.SerializeObject(currentArticle), Encoding.UTF8, "application/json");

                using var response = await client.PostAsync(ConfigureHttpClient.apiUrl + "Articles", content);
                string apiResponse = await response.Content.ReadAsStringAsync();

                resultarticle = JsonConvert.DeserializeObject<Article>(apiResponse);


                List<Referencer> resultReferences = new List<Referencer>();

                resultReferences = await FunctionAPI.AddTagToArticle(idTags, resultarticle.IdArticle);


                return Redirect("/Articles/Index");
            }
            else
            {

                return RedirectToAction(nameof(Index));
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
