
//Auteur    : Loris habegger
//Date      : 06.05.2021
//Fichier   : PropositionArticleController.cs

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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using WikiTechWebApp.Exceptions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Dynamic;

namespace WikiTechWebApp.Controllers
{
    public class PropositionArticleController : Controller
    {

        static HttpClient client = new HttpClient();
        private readonly IWebHostEnvironment webhost; //utilisé pou l'enregistrement de l'image
        public PropositionArticleController(IWebHostEnvironment _webhost) 
        {
            webhost = _webhost;
            client = ConfigureHttpClient.configureHttpClient(client);
        }

        [Authorize]
        // GET: PropositionArticleController
        public ActionResult Index()
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

            //currentArticle.DatepublicationArticle = DateTime.Now; //à changer car elle sera attribuée lors de la validation quand il y en aura une

            currentArticle.Id = IdUser;
            currentArticle.IdSection = int.Parse(Request.Form["rdsection"]); // récupération de l'id de la section selectionnée

            if (ModelState.IsValid)
            {
                //Article article = await FunctionArticles.AddArticle(currentArticle); to passe trought another file
                try
                {
                    Article resultarticle;
                    StringContent content = new StringContent(JsonConvert.SerializeObject(currentArticle), Encoding.UTF8, "application/json");

                    using var response = await client.PostAsync(ConfigureHttpClient.apiUrl + "Articles", content);
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    resultarticle = JsonConvert.DeserializeObject<Article>(apiResponse);


                    List<Referencer> resultReferences = new List<Referencer>();

                    resultReferences = await FunctionAPI.AddTagToArticle(idTags, resultarticle.IdArticle);


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


        [HttpPost]
        [RequestSizeLimit(2097152)] //2MB
        public async Task<string> uploadImg(IFormFile file)
        {

            if (file == null)
            {
                Console.WriteLine("image too large");

                ViewBag.ErrorMessage = string.Format("image is Too large");

                return "Image too large";
            }
            else
            {

                try
                {
                    //path to save the file
                    var saveimg = Path.Combine(webhost.WebRootPath, "images", file.FileName);

                    //convert file to img for the resizing
                    var image = Image.Load(file.OpenReadStream());
                    var size = image.Size();

                    double currentLargeur = image.Width;
                    double currentHauteur = image.Height;

                    //resizing dimension
                    while (currentLargeur > 800 || currentHauteur > 800)
                    {
                        currentLargeur = currentLargeur / 1.25;
                        currentHauteur = currentHauteur / 1.25;

                    }

                    int newLargeur = (int)currentLargeur;
                    int newHauteur = (int)currentHauteur;

                    //resize with nwe value
                    image.Mutate(x => x.Resize(newLargeur, newHauteur));


                    image.Save(saveimg);
                    return saveimg;
                }
                catch (ExceptionImg e)
                {

                    return e.getMessage();
                }

            }

        }


        [Authorize]
        // GET: PropositionArticleController
        public ActionResult Approbation()
        {

            IEnumerable<Article> artList;
            IEnumerable<Changement> changementliste;

            dynamic dynamicmodel = new ExpandoObject();

            try
            {

                HttpResponseMessage response = client.GetAsync("Articles/nodate").Result;
                artList = response.Content.ReadAsAsync<IEnumerable<Article>>().Result;

                dynamicmodel.Article = artList;

                HttpResponseMessage responsechangement = client.GetAsync("Changements/nodate").Result;
                changementliste = responsechangement.Content.ReadAsAsync<IEnumerable<Changement>>().Result;

                dynamicmodel.Changement = changementliste;


                return View(dynamicmodel);

            }
            catch (ExceptionLiaisonApi e)
            {
                Console.WriteLine(e.getMessage());
                return Redirect("/Home/Index");
            }
            return View();
        }

        [Authorize]
        // GET: the article detail for decision
        public ActionResult approbationArticleDetail(int id)
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


        [Authorize]
        [HttpPost]
        public async Task<ActionResult> validerArticle([Bind("IdArticle,TitreArticle,DescriptionArticle,TextArticle,IdSection,Referencer,IsqualityArticle")] Article _article)
        {

            Article currentArticle = _article;


            currentArticle.DatepublicationArticle = DateTime.Now; //^Date de la validation

            currentArticle.IdArticle = Int32.Parse(Request.Form["IdArticle"]);
            currentArticle.IdSection = Int32.Parse(Request.Form["IdSection"]); 
            currentArticle.Id = Request.Form["IdAuteur"];


            try
                {
                    Article resultarticle;

                    using var response = await client.PutAsJsonAsync(ConfigureHttpClient.apiUrl + "Articles/" + currentArticle.IdArticle, currentArticle);
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    resultarticle = JsonConvert.DeserializeObject<Article>(apiResponse);


                    return Redirect("/Articles/" + currentArticle.IdArticle);

                }
                catch (ExceptionLiaisonApi e)
                {
                    Console.WriteLine(e.getMessage());
                    return RedirectToAction(nameof(Index));
                }
        }

        [Authorize]
        public async Task<ActionResult> supprimerArticle(int id)
        {

            Article currentArticle = new Article();
            currentArticle.IdArticle = Int32.Parse(Request.Form["IdArticle"]);


            return RedirectToAction(nameof(Index));



        }


    }
}
