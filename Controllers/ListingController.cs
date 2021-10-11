using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Karma.Models;
using Karma.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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
        [Authorize]
        public ActionResult Post(Listing listing) {
            var random = new Random();
            listing.Id = random.Next(9999).ToString(); // temp fix for id generation, later this should be assigned in DB.
            listing.DatePublished = DateTime.UtcNow; //temp fix for curr date with form submit
            
            string userId = User.FindFirst(ClaimTypes.Name)?.Value;
            listing.OwnerId = userId;
            _listingRepository.AddListing(listing);
            return StatusCode(StatusCodes.Status200OK);
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
        [Authorize]
        public void DeleteListing(string id) {
            _listingRepository.DeleteListingById(id);
        }
    }
}