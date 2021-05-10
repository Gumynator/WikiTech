using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WikiTechAPI.Models;
using WikiTechWebApp.ApiFunctions;

namespace WikiTechWebApp.Controllers
{
    public class AspNetUsersController : Controller
    {
        //private readonly WikiTechDBContext _context;
        static HttpClient client = new HttpClient();
       
        public AspNetUsersController(IHttpContextAccessor httpContextAccessor)
        {
            
            client = ConfigureHttpClient.configureHttpClient(client);
            client.DefaultRequestHeaders.Add("ApiKey", "61c08ad1-0823-4c38-9853-700675e3c8fc");

            //_context = context;
        }

        // GET: AspNetUsers
        public async Task<IActionResult> Index()
        {
            //var wikiTechDBContext = _context.AspNetUsers.Include(a => a.IdAbonnementNavigation).Include(a => a.IdGenreNavigation).Include(a => a.IdGradeNavigation).Include(a => a.IdVilleNavigation);
            //return View(await wikiTechDBContext.ToListAsync());
            IEnumerable<AspNetUsers> userList;
            HttpResponseMessage response = client.GetAsync("AspNetUsers").Result;
            userList = response.Content.ReadAsAsync<IEnumerable<AspNetUsers>>().Result;

            return View(userList);
        }

        // GET: AspNetUsers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AspNetUsers aspNetUsers;
            HttpResponseMessage responseuser = client.GetAsync("AspNetUsers/" + id).Result;
            aspNetUsers = responseuser.Content.ReadAsAsync<AspNetUsers>().Result;
            if (aspNetUsers == null)
            {
                return NotFound();
            }

            return View(aspNetUsers);
        }

        // GET: AspNetUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AspNetUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdGrade,IdAbonnement,IdVille,IdGenre,NomAspnetuser,PrenomAspnetuser,UserName,IsactiveAspnetuser,IsprivateAspnetuser,AdresseAspnetuser,DatecreationAspnetuser,NbpointAspnetuser,NumcarteAspnetuser,CvvcarteAspnetuser,ExpirationcarteAspnetuser,CreditAspnetuser,IbanAspnetuser,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] AspNetUsers aspNetUsers)
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

        // GET: AspNetUsers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AspNetUsers aspNetUsers;
            HttpResponseMessage responseuser = client.GetAsync("AspNetUsers/" + id).Result;
            aspNetUsers = responseuser.Content.ReadAsAsync<AspNetUsers>().Result;

            if (aspNetUsers == null)
            {
                return NotFound();
            }
            return View();
        }

        // POST: AspNetUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    
   
        public async Task<IActionResult> Edit(string id, [Bind("Id,IdGrade,IdAbonnement,IdVille,IdGenre,NomAspnetuser,PrenomAspnetuser,UserName,IsactiveAspnetuser,IsprivateAspnetuser,AdresseAspnetuser,DatecreationAspnetuser,NbpointAspnetuser,NumcarteAspnetuser,CvvcarteAspnetuser,ExpirationcarteAspnetuser,CreditAspnetuser,IbanAspnetuser,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] AspNetUsers aspNetUsers)
        {
            if (id != aspNetUsers.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                   
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AspNetUsersExists(aspNetUsers.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
           
            return View(aspNetUsers);
        }


        public async Task<IActionResult> EditActive(string id)
        {
          
            if (ModelState.IsValid)
            {
                try
                {
                    HttpResponseMessage responseuser = client.GetAsync("AspNetUsers/" + id).Result;
                    var user = await FunctionAPI.GetUserByIdAsync(client, id);
                    if (user.LockoutEnd == null)
                    {
                        user.LockoutEnd = DateTime.MaxValue;
                    }
                    else
                    {
                        user.LockoutEnd = null;
                    }
                    
                    
                    HttpResponseMessage response = await client.PutAsJsonAsync("AspNetUsers/" + id, user);
                    if (response.IsSuccessStatusCode)
                    {

                        user = await response.Content.ReadAsAsync<AspNetUsers>();
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                   
                }
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: AspNetUsers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            AspNetUsers aspNetUsers;
            HttpResponseMessage responseuser = client.GetAsync("AspNetUsers/" + id).Result;
            aspNetUsers = responseuser.Content.ReadAsAsync<AspNetUsers>().Result;

            if (aspNetUsers == null)
            {
                return NotFound();
            }

            return View(aspNetUsers);
        }

        // POST: AspNetUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            AspNetUsers aspNetUsers;
            HttpResponseMessage responseuser = client.GetAsync("AspNetUsers/" + id).Result;
            aspNetUsers = responseuser.Content.ReadAsAsync<AspNetUsers>().Result;
            return RedirectToAction(nameof(Index));
        }

        private bool AspNetUsersExists(string id)
        {
            bool result =false;
            AspNetUsers aspNetUsers;
            HttpResponseMessage responseuser = client.GetAsync("AspNetUsers/" + id).Result;
            if (responseuser != null)
            {
                result = true;
            }
            return result;
        }
        

    }
}
