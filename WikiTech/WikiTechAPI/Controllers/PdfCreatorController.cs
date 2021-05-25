using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wkhtmltopdf.NetCore;

namespace WikiTechAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfCreatorController : ControllerBase
    {
        readonly IGeneratePdf generatePdf;
        public PdfCreatorController(IGeneratePdf generatePdf)
        {
            this.generatePdf = generatePdf;
        }
    }
}
