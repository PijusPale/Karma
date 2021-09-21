using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Karma.Models;

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

        [HttpPost]
        public void Post(Listing listing) {
            string jsonString = System.IO.File.ReadAllText(FilePath);
            List<Listing> listings = JsonSerializer.Deserialize<List<Listing>>(jsonString);
            listings.Add(listing);
            string newJsonString = JsonSerializer.Serialize(listings);
            System.IO.File.WriteAllText(FilePath, newJsonString);
        }

        [HttpGet]
        public IEnumerable<Listing> GetAllListings()
        {
            string jsonString = System.IO.File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<Listing>>(jsonString);
        }

        [HttpGet("{id}")]
        public Listing GetListingById(string id) 
        {
            string jsonString = System.IO.File.ReadAllText(FilePath);
            List<Listing> listings = JsonSerializer.Deserialize<List<Listing>>(jsonString);
            return listings.FirstOrDefault(x => x.Id == id);
        }
    }
}