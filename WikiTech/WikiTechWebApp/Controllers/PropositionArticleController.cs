
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
using Microsoft.AspNetCore.Identity.UI.Services;
using WikiTechAPI.Utility;

namespace WikiTechWebApp.Controllers
{
    public class PropositionArticleController : Controller
    {

        const int POINT_VALIDEUR = 2;
        const int POINT_CREATEUR = 5;

        static HttpClient client = new HttpClient();
        static IEmailSender _sender;
        private readonly IWebHostEnvironment webhost; //utilisé pou l'enregistrement de l'image
        public PropositionArticleController(IWebHostEnvironment _webhost, IEmailSender sender) 
        {
            _sender = sender;
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

                    using var response = await client.PostAsync(ConfigureHttpClient.apiUrl + "Articles" + "/" + IdUser, content);
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    resultarticle = JsonConvert.DeserializeObject<Article>(apiResponse);


                    List<Referencer> resultReferences = new List<Referencer>();

                    resultReferences = await FunctionAPI.AddTagToArticle(idTags, resultarticle.IdArticle);

                    Logwritter log = new Logwritter("Nouvelle article proposé et en attente de validation");
                    log.writeLog();


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

                    Logwritter log = new Logwritter("Image Uploadée");
                    log.writeLog();


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

                HttpResponseMessage response = client.GetAsync("Articles/toactive").Result;
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
            currentArticle.IsactiveArticle = true;

           string valideurArticle = User.FindFirstValue(ClaimTypes.NameIdentifier);


            try
                {
                    Article resultarticle;

                    using var response = await client.PutAsJsonAsync(ConfigureHttpClient.apiUrl + "Articles/" + valideurArticle, currentArticle);
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    resultarticle = JsonConvert.DeserializeObject<Article>(apiResponse);

                    var usernameq = await FunctionAPI.GetUserByIdAsync(client, currentArticle.Id);

                    //envoie du mail d'état à l'auteur de l'article
                    await _sender.SendEmailAsync(usernameq.Email, "Article accepté", "Bonjour, votre article ayant pour titre : " + currentArticle.TitreArticle + " a été validé. Vous pourrez le consulter en ligne");

                    //fonction d'ajout de point pour le valideur
                    FunctionAPI.IncreasePointForUser(client, valideurArticle, POINT_VALIDEUR);
                    //fonction d'ajout de point pour l'auteur
                    FunctionAPI.IncreasePointForUser(client, currentArticle.Id, POINT_CREATEUR);

                return Redirect("/Articles/Details/" + currentArticle.IdArticle);

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

            Article article;
            string valideurArticle = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                //get article
                HttpResponseMessage responsearticle = client.GetAsync("Articles/" + id).Result;
                article = responsearticle.Content.ReadAsAsync<Article>().Result;

                //delete the article
                using var response = await client.DeleteAsync(ConfigureHttpClient.apiUrl + "Articles/" + id + "/" + valideurArticle);
                string apiResponse = await response.Content.ReadAsStringAsync();


                var usernameq = await FunctionAPI.GetUserByIdAsync(client, article.Id);

                await _sender.SendEmailAsync(usernameq.Email, "Article pas accepté", "Bonjour, votre article ayant pour titre: " + article.TitreArticle + " n'est pas validé. il ne respecte probablement pas la politique de wikitech");

                //fonction d'ajout de point pour le valideur
                FunctionAPI.IncreasePointForUser(client, valideurArticle, POINT_VALIDEUR);

                //Reference (tags) is deleting by cascade

                return Redirect("/Home/Index");

            }
            catch (ExceptionLiaisonApi e)
            {
                Console.WriteLine(e.getMessage());
                return Redirect("/Home/Index");
            }
        }

        [Authorize]
        // GET: the article detail for decision
        public ActionResult approbationChangementDetail(int id)
        {

            Article article;
            Changement changement;
            dynamic dynamicmodel = new ExpandoObject();

            try
            {

                HttpResponseMessage responsechangement = client.GetAsync("Changements/" + id).Result;
                changement = responsechangement.Content.ReadAsAsync<Changement>().Result;

                dynamicmodel.Changement = changement;

                HttpResponseMessage responsearticle = client.GetAsync("Articles/" + changement.IdArticle).Result;
                article = responsearticle.Content.ReadAsAsync<Article>().Result;

                dynamicmodel.Article = article;

                return View(dynamicmodel);

            }
            catch (ExceptionLiaisonApi e)
            {
                Console.WriteLine(e.getMessage());
                return Redirect("/Home/Index");
            }

        }


        [Authorize]
        [HttpPost]
        public async Task<ActionResult> validerChangement()
        {

            Changement currentchangement = new Changement();

            currentchangement.DatepublicationChangement = DateTime.Now; //Date de la validation
            currentchangement.IdChangement = Int32.Parse(Request.Form["IdChangement"]);
            currentchangement.IdArticle = Int32.Parse(Request.Form["IdArticle"]);
            currentchangement.TextChangement = Request.Form["TextChangement"];
            currentchangement.DescriptionChangement = Request.Form["DescriptionChangement"];
            currentchangement.TitreChangement = Request.Form["TitreChangement"];
            currentchangement.Id = Request.Form["IdAuteurChangement"];
            currentchangement.ResumeChangement = Request.Form["resumeChange"];

            string valideurChangement = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                Changement resultatChangement;

                using var response = await client.PutAsJsonAsync(ConfigureHttpClient.apiUrl + "Changements/" + valideurChangement, currentchangement);
                string apiResponse = await response.Content.ReadAsStringAsync();

                resultatChangement = JsonConvert.DeserializeObject<Changement>(apiResponse);

                var usernameq = await FunctionAPI.GetUserByIdAsync(client, currentchangement.Id);

                //envoie du mail d'état à l'auteur du changement
                await _sender.SendEmailAsync(usernameq.Email, "Changement accepté", "Bonjour, votre changement ayant pour titre : " + currentchangement.TitreChangement + " a été validé. Vous pourrez le consulter en ligne");

                //fonction d'ajout de point pour le valideur
                FunctionAPI.IncreasePointForUser(client, valideurChangement, POINT_VALIDEUR); 
                //fonction d'ajout de point pour l'auteur
                FunctionAPI.IncreasePointForUser(client, currentchangement.Id, POINT_CREATEUR);

                return Redirect("/Articles/Details/" + currentchangement.IdArticle);

            }
            catch (ExceptionLiaisonApi e)
            {
                Console.WriteLine(e.getMessage());
                return RedirectToAction(nameof(Index));
            }
        }

        [Authorize]
        public async Task<ActionResult> supprimerChangement(int id)
        {

            Changement currentchangement = new Changement();
            currentchangement.IdChangement = id;
            string valideurChangement = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                //get Changement
                HttpResponseMessage responsearticle = client.GetAsync("Changements/" + currentchangement.IdChangement).Result;
                currentchangement = responsearticle.Content.ReadAsAsync<Changement>().Result;

                //delete the Changement
                using var response = await client.DeleteAsync(ConfigureHttpClient.apiUrl + "Changements/" + currentchangement.IdChangement + "/" + valideurChangement);
                string apiResponse = await response.Content.ReadAsStringAsync();


                var usernameq = await FunctionAPI.GetUserByIdAsync(client, currentchangement.Id);

                await _sender.SendEmailAsync(usernameq.Email, "Changement pas accepté", "Bonjour, votre Changement ayant pour titre: " + currentchangement.TitreChangement + " n'est pas validé. il ne respecte probablement pas la politique de wikitech");

                //fonction d'ajout de point pour le valideur
                FunctionAPI.IncreasePointForUser(client, valideurChangement, POINT_VALIDEUR);

                //Reference (tags) is deleting by cascade

                return Redirect("/Home/Index");

            }
            catch (ExceptionLiaisonApi e)
            {
                Console.WriteLine(e.getMessage());
                return Redirect("/Home/Index");
            }
        }


    }
}
