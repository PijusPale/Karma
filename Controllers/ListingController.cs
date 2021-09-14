using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Karma.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ListingController : ControllerBase
    {
        private static readonly string[] Names = new[]
        {
            "Noah", "John", "James", "William", "Daniel", "Charlotte", "Kenny", "Bacon"
        };

        private static readonly string[] Locations = new[]
        {
            "Lithuania", "Latvia", "Poland", "Germany"
        };

        private readonly ILogger<ListingController> _logger;

        public ListingController(ILogger<ListingController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Listing> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new Listing
            {
                Id = "RN4564654",
                Name = Names[rng.Next(Names.Length)],
                Description = "tis a description",
                Quantity = rng.Next(1,5),
                Location = Locations[rng.Next(Locations.Length)],
                ImagePath = "https://i.pinimg.com/474x/28/7b/9a/287b9a35afe88d4c52eeb83fedaeabdf.jpg",
                DatePublished = DateTime.Now
                
            })
            .ToArray();
        }
    }
}