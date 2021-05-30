using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikiTechWebApp.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {

            switch (statusCode)
            {

                case 404:
                    ViewBag.ErrorMessage = "Erreur 404, la ressource n'est pas trouvée";
                    break;

            }

            return View("NotFound");
        }
    }
}
