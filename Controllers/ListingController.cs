using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Karma.Models;
using Karma.Repositories;

namespace Karma.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ListingController : ControllerBase
    {
        private readonly ILogger<ListingController> _logger;

        private readonly IListingRepository _listingRepository;

        public ListingController(ILogger<ListingController> logger, IListingRepository listingRepository)
        {
            _logger = logger;
            _listingRepository = listingRepository;
        }

        [HttpPost]
        public void Post(Listing listing) {
            listing.DatePublished = DateTime.UtcNow; //temp fix for curr date with form submit
            _listingRepository.AddListing(listing);
        }

        [HttpGet]
        public IEnumerable<Listing> GetAllListings()
        {
            return _listingRepository.GetAllListings();
        }

        [HttpGet("{id}")]
        public Listing GetListingById(string id) 
        {
            return _listingRepository.GetListingById(id);
        }

        [HttpDelete("{id}")]
        public void DeleteListing(string id) {
            _listingRepository.DeleteListingById(id);
        }
    }
}