﻿using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace Shopping.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {

        public string uniqueFileName = null;

        [HttpPost]
        public IActionResult Upload()
        {

            var file = Request.Form.Files[0];
            var folderName = Path.Combine("StaticFiles", "Images");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            if (file.Length > 0)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

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

    }

}
      