
//Auteur    : Loris habegger
//Date      : 26.05.2021
//Fichier   : errorController.cs (controller d'erreur comme erreur 404)


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
                    ViewBag.ErrorMessage = "Ouuuups, la ressource n'a pas été trouvée | error 404";
                    break;

            }

            return View("NotFound");
        }
    }
}
