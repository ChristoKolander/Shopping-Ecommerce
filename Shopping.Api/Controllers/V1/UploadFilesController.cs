using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;

namespace Shopping.Api.Controllers
{

    [ApiVersion("1.0")]
    [Route("api/v{version:apiversion}/[controller]")]
    [ApiController]
    public class UploadFilesController : ControllerBase
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        
        public UploadFilesController(IWebHostEnvironment webHostEnvironment )
        {
            this.webHostEnvironment = webHostEnvironment;
        }

    [HttpPost("[action]")]
    public IActionResult Upload()
    {
                
        var file = Request.Form.Files[0];

          
        var folderName = Path.Combine("StaticFiles", "Images");
        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

           

            if (file.Length > 0)
        {

            string uniqueFileName = null;
            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

            //Pragim GUIDlösning
            uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;

            var fullPath = Path.Combine(pathToSave, uniqueFileName);
            
            var dbPath = Path.Combine(folderName, uniqueFileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return Ok(dbPath);
        }
        else
        {
            return BadRequest();
        }
    }
               

    [HttpPost("[action]")]
    public IActionResult UploadFiles(List<IFormFile> files)
    {


        if (files.Count == 0)
        {
            return BadRequest();
        }



        var folderName = Path.Combine("StaticFiles", "Images");

        string directoryPath = Path.Combine(this.webHostEnvironment.ContentRootPath, folderName);

            foreach (IFormFile file in files)
            {
           

                string filePath = Path.Combine(directoryPath, file.FileName);


                using (FileStream stream = new FileStream(filePath, FileMode.Create))

                {
                    file.CopyTo(stream);
                }
            }
                    return Ok("Upload Successful!");
        }
 
    }
}



