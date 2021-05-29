using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using WikiTechAPI.Models;
using WikiTechWebApp.ApiFunctions;
using WikiTechWebApp.Services;
using WikiTechWebApp.Exceptions;
using System.Dynamic;

namespace WikiTechWebApp.Controllers
{
    public class PostulationsController : Controller
    {
        static HttpClient client = new HttpClient();
        static IEmailSender _sender;
        public PostulationsController(IHttpContextAccessor httpContextAccessor,IEmailSender sender)
        {
            client = ConfigureHttpClient.configureHttpClient(client);
            //client.DefaultRequestHeaders.Add("ApiKey", "61c08ad1-0823-4c38-9853-700675e3c8fc");
            _sender = sender;
        }
        // GET: PostulationsController
        public ActionResult Index()
        {
            return View();
        }

        // GET: PostulationsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [Authorize]
        // GET: PostulationsController/Create
        public async Task<IActionResult> CreatePostulation(Postulation postulation)
        {
           
            var IdUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (IdUser==null)
            {
                return RedirectToAction("Index", "Home");
            }
            postulation.Id = IdUser;
            postulation.DatePostulation=DateTime.Today;
            var usernameq = await FunctionAPI.GetUserByIdAsync(client, IdUser);
            //var postulation = new Postulation(){DatePostulation = DateTime.Today,Id = id,,,};
            var postTask = client.PostAsJsonAsync<Postulation>("Postulations", postulation);
            postTask.Wait();

            var result = postTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<Postulation>();
                readTask.Wait();

                await _sender.SendEmailAsync(usernameq.Email, "Postulation confirmée", "Bonjour, nous vous confirmons la bonne réception de votre postulation");

                var insertedPostulation = readTask.Result;
            }
            else
            {
                Console.WriteLine(result.StatusCode);
            }


            return View("ValidationPostulation");
        }

        // POST: PostulationsController/Create
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

        // GET: PostulationsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PostulationsController/Edit/5
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

        // GET: PostulationsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PostulationsController/Delete/5
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
        // GET: PropositionArticleController
        public ActionResult ListPostulations()
        {

            IEnumerable<Postulation> postulationList;
            dynamic dynamicmodel = new ExpandoObject();

            try
            {
                
                HttpResponseMessage response = client.GetAsync("Postulations").Result;
                postulationList = response.Content.ReadAsAsync<IEnumerable<Postulation>>().Result;
                dynamicmodel.Postulation = postulationList;

                return View(dynamicmodel);

            }
            catch (ExceptionLiaisonApi e)
            {
                Console.WriteLine(e.getMessage());
                return Redirect("/Home/Index");
            }
            return View();
        }
    }
}
