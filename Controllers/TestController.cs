using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
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
        public string Get(string data)
        { 
        Console.WriteLine("HTTP Post");
        Console.WriteLine(data);
            if(data != null){
                Console.WriteLine("ok");
                return "success";
            }
            else
                return "failed";
        }
    }
}