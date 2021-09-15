using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Karma.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ListingController : ControllerBase
    {
        private static readonly string FilePath = Path.Combine("data", "ListingsData.json");

        private readonly ILogger<ListingController> _logger;

        public ListingController(ILogger<ListingController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Listing> Get()
        {
            string jsonString = System.IO.File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<Listing[]>(jsonString);
        }
    }
}