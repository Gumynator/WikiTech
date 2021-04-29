using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikiTechWebApp.Controllers
{
    public class PropositionArticleController : Controller
    {
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
