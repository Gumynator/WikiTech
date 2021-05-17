
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

namespace WikiTechWebApp.Controllers
{
    public class PropositionArticleController : Controller
    {

        static HttpClient client = new HttpClient();
        private readonly IWebHostEnvironment webhost;
        public PropositionArticleController(IWebHostEnvironment _webhost) 
        {
            webhost = _webhost;
            client = ConfigureHttpClient.configureHttpClient(client);
            client.DefaultRequestHeaders.Add("ApiKey", "61c08ad1-0823-4c38-9853-700675e3c8fc");
        }

        [Authorize]
        // GET: PropositionArticleController
        public ActionResult Index()
        {
            return View();
        }

        // GET: PropositionArticleController/Details/5
       /* public ActionResult Details(int id)
        {
            return View();
        }*/

       /* // GET: PropositionArticleController/Create
        public ActionResult Create()
        {
            return View();
        }
       */
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

        /* // GET: PropositionArticleController/Edit/5
         public ActionResult Edit(int id)
         {
             return View();
         }
        */
        /*
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
        */
        /*
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
        */

        /* [HttpPost]
         public async Task<string> uploadImg(IFormFile file)
         {
             string message;
             var saveimg = Path.Combine(webhost.WebRootPath, "images", file.FileName);
             string imgext = Path.GetExtension(file.FileName);

             //using var image = Image.Load(file.OpenReadStream());
             //image.Mutate(x => x.Resize(256, 256));

             try
             {

                 if (imgext == ".jpg" || imgext == ".png")
                 {
                     using (var uploadimg = new FileStream(saveimg, FileMode.Create))
                     {

                         await file.CopyToAsync(uploadimg);
                         message = "The selected file" + file.FileName + " est sauvé";
                     }

                 }
                 else
                 {
                     message = "seule les extension JPG et PNG sont supportée";
                 }

                 return "filename : " + saveimg + " le message :" + message;

             }
             catch (ExceptionImg e)
             {

                 return e.getMessage();
             }



         }
        */

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
       
    }
}
