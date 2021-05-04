using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace WikiTechWebApp.Controllers
{
    public class ImgController : Controller
    {
        private readonly IWebHostEnvironment webhost;

        public ImgController(IWebHostEnvironment _webhost)
        {
            webhost = _webhost;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile imgfile)
        {
            var saveimg = Path.Combine(webhost.WebRootPath, "Images", imgfile.FileName);
            string imgext = Path.GetExtension(imgfile.FileName);

            if (imgext == ".jpg" || imgext == ".png")
            {
                using (var uploadimg = new FileStream(saveimg, FileMode.Create))
                {

                    await imgfile.CopyToAsync(uploadimg);
                    ViewData["Message"] = "The selected file" + imgfile.FileName + " est sauvé";
                }

            }
            else
            {
                ViewData["Message"] = "seule les extension JPG et PNG sont supportée";
            }
            return View();
        
        }
    }
}
