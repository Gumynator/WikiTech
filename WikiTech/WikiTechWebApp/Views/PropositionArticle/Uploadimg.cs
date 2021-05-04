using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace WikiTechWebApp.Views.PropositionArticle
{
    
    public class Uploadimg
    {

        private readonly IWebHostEnvironment webhost;

        public Uploadimg(IWebHostEnvironment _webhost)
        {
            webhost = _webhost;

        }

        [HttpPost]
        public async Task<string> uploadImg(IFormFile imgfile)
        {
            string message;
            var saveimg = Path.Combine(webhost.WebRootPath, "Images", imgfile.FileName);
            string imgext = Path.GetExtension(imgfile.FileName);

            if (imgext == ".jpg" || imgext == ".png")
            {
                using (var uploadimg = new FileStream(saveimg, FileMode.Create))
                {

                    await imgfile.CopyToAsync(uploadimg);
                    message = "The selected file" + imgfile.FileName + " est sauvé";
                }

            }
            else
            {
                message = "seule les extension JPG et PNG sont supportée";
            }
            return "filename : " + saveimg + " le message :" + message;

        }


    }
}
