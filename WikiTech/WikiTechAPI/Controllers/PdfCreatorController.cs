//Auteur    : Pancini Marco
//Date      : 24.05.2021
//Fichier   : PdfCreatorController.cs
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WikiTechAPI.Models;
using WikiTechAPI.Utility;

namespace WikiTechAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfCreatorController : ControllerBase
    {
        private IConverter _converter;
        private IWebHostEnvironment _hostingEnvironment;
        public PdfCreatorController(IConverter converter, IWebHostEnvironment env)
        {
            _converter = converter;
            _hostingEnvironment = env;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Auteur : Pancini Marco
        //Création : 25.04.2021
        //Modification : 30.05.2021
        //Description : Function qui permet la génération, le stockage dans le wwwroot, et le download du pdf d'une facture
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpPost("CreatePdfFacture/")]
        public IActionResult CreatePdfFacture(Facture PdfFacture)
        {
            var webRoothPath = _hostingEnvironment.WebRootPath;
            var IdUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "PDF Report",
                //Out = @"C:\PDFCreator\Employee_Report.pdf"
                Out = webRoothPath+"/pdfFactures/"+PdfFacture.IdFacture+".pdf"

            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = TemplateGenerator.GetHTMLFactureString(PdfFacture),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "PdfStyles.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var file = _converter.Convert(pdf);

            //var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            //using (var fileStream = new FileStream(Path.Combine(uploads, "test"), FileMode.Create))
            //{
            //    await pdf.CopyToAsync(fileStream);
            //}

            //return File(file, "application/pdf", PdfFacture.IdFacture + ".pdf");
            //return File(file, "application/pdf");
            return Ok("Successfully created PDF document.");
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Auteur : Pancini Marco
        //Création : 26.04.2021
        //Modification : 30.05.2021
        //Description : Function qui permet la génération, le stockage dans le wwwroot, et le download du pdf d'un don
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpPost("CreatePdfDon/")]
        public IActionResult CreatePdfDon(Don PdfDon)
        {
            var webRoothPath = _hostingEnvironment.WebRootPath;
            var IdUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "PDF Report",
                //Out = @"C:\PDFCreator\Employee_Report.pdf"
                Out = webRoothPath + "/pdfDons/" + PdfDon.IdDon + ".pdf"

            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = TemplateGenerator.GetHTMLDonString(PdfDon),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "PdfStyles.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var file = _converter.Convert(pdf);

            //var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            //using (var fileStream = new FileStream(Path.Combine(uploads, "test"), FileMode.Create))
            //{
            //    await pdf.CopyToAsync(fileStream);
            //}

            //return File(file, "application/pdf", PdfFacture.IdFacture + ".pdf");
            //return File(file, "application/pdf");
            return Ok("Successfully created PDF document.");
        }
    }
}
