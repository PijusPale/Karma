using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using Karma.Models;

namespace Karma.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : Controller
    {
        private readonly ILogger<ListingController> _logger;


        public ImageController(ILogger<ListingController> logger)
        {
            _logger = logger;
        }

        
        [HttpPost]
        public ActionResult Post([FromForm] FileModel file)
        {   
            try
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", file.Name);
                
                using (Stream stream = new FileStream(path, FileMode.Create))
                {
                    file.FormFile.CopyTo(stream);
                }
                
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ImageController posting file");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
