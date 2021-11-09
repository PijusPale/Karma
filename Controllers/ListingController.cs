using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Karma.Models;
using Karma.Repositories;
using Karma.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Linq;

namespace Karma.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ListingController : ControllerBase
    {
        private readonly ILogger<ListingController> _logger;

        private readonly IListingRepository _listingRepository;

        private readonly IUserService _userService;

        public event EventHandler nofitication;

        public ListingController(ILogger<ListingController> logger, IListingRepository listingRepository, IUserService userService)
        {
            _logger = logger;
            _listingRepository = listingRepository;
            _userService = userService;
        }

        [HttpPost]
        [Authorize]
        public ActionResult Post(Listing listing)
        {
            listing.DatePublished = DateTime.UtcNow; //temp fix for curr date with form submit

            string userId = this.TryGetUserId();
            if (userId == null)
                return Unauthorized();

            listing.OwnerId = userId;

            listing.isReserved = false;
            _listingRepository.Add(listing);
            return Ok();
        }

        [HttpPost("id={id}/reserve={reserve}/for={receiverId}")]
        [Authorize]
        public ActionResult ReserveListing(string id, bool reserve, string receiverId)
        {
            string userId = this.TryGetUserId();
            var listing = _listingRepository.GetById(id);
            if(userId != listing.OwnerId)
                return Unauthorized();

            listing.isReserved = reserve;
            listing.recipientId = receiverId;
            _listingRepository.Update(listing);

            return Ok();
        }

        [HttpGet]
        public IEnumerable<Listing> GetAllListings()
        {
            return _listingRepository.GetAll();
        }

        [HttpGet("userId={id}")]
        [Authorize]
        public ActionResult<IEnumerable<Listing>> GetListingsOfUser(string id)
        {   
            string userId = this.TryGetUserId();
            if(userId == null || id != userId)
                return Unauthorized();

            return _listingRepository.GetAllUserListings(id).ToList();
        }

        [HttpGet("requesteeId={id}")]
        [Authorize]
        public ActionResult<IEnumerable<Listing>> GetRequestedListingsOfUser(string id)
        {
            string userId = this.TryGetUserId();
            if (id != userId)
                return Unauthorized();

            return _listingRepository.GetRequestedListings(id).ToList();
        }

        [HttpGet("{id}")]
        public IActionResult GetListingById(string id)
        {
            var listing =  _listingRepository.GetById(id);
            return listing != null ? Ok(listing) : NotFound();
        }

        [HttpGet("request/{id}")]
        [Authorize]
        public IActionResult RequestListing(string id)
        {
            string userId = this.TryGetUserId();
            if (userId == null)
                return Unauthorized();

            var listing = _listingRepository.GetById(id);
            var user = _userService.GetUserById(userId);
            if (listing.OwnerId == userId)
                return Forbid();
            
            if(listing.RequestedUserIDs.Contains(userId) || user.RequestedListings.Contains(id))
                return Conflict();

            listing.RequestedUserIDs.Add(userId);
            _listingRepository.Update(listing);

            user.RequestedListings.Add(listing.Id);
            
            return Ok();
        }

        protected virtual void onNofitication()
        {
            if (nofitication != null) nofitication(this);
        }

        private static void nofiticationHandler (object sender)
        {
            Console.WriteLine("Nofitication event works!!!");
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteListing(string id)
        {
            string userId = this.TryGetUserId();
            var listing = _listingRepository.GetById(id);
            if (userId == null || listing.OwnerId != userId)
                return Unauthorized();
            _listingRepository.DeleteById(id);
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult UpdateListing(string id, [FromBody] Listing listing)
        {

            var old = _listingRepository.GetById(listing.Id);
            if (old == null) return NotFound();

            string userId = this.TryGetUserId();
            if (userId == null || old.OwnerId != userId) return Unauthorized();

            listing.DatePublished = DateTime.UtcNow; //temp fix for curr date with form submit
            listing.RequestedUserIDs = old.RequestedUserIDs; // temp fix for saving old requests
            listing.OwnerId = userId;
            _listingRepository.Update(listing);
            return Ok();
        }
    }
}