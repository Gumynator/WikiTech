using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
//Auteur    : Loris habegger
//Date      : 15.06.2021
//Fichier   : ApiLogController.cs (retourne les logs à la vue)

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikiTechAPI.Utility;

namespace WikiTechAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiLogController : ControllerBase
    {
        // GET: api/ApiLog
        [HttpGet]
        public async Task<ActionResult<List<String>>> GetLogs()
        {

            List<String> allLogs = Logwritter.readLog();

            return allLogs;
        }

    }
}
