using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Karma.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
    private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public ActionResult PostListing([FromBody]Listing listing)
        { 
        Console.WriteLine("HTTP Post");
        Console.WriteLine(listing);
            if (listing != null)
            {
                Console.WriteLine("ok");
                return Ok();
            }
            else
                return BadRequest("Not good");
        }
    }
}