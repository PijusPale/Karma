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
using System.Threading.Tasks;

namespace Karma.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ListingController : ControllerBase
    {
        private readonly ILogger<ListingController> _logger;

        private readonly IListingRepository _listingRepository;

        private readonly IUserService _userService;

        public ListingController(ILogger<ListingController> logger, IListingRepository listingRepository, IUserService userService)
        {
            _logger = logger;
            _listingRepository = listingRepository;
            _userService = userService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> PostAsync(Listing listing)
        {
            listing.DatePublished = DateTime.UtcNow; //temp fix for curr date with form submit

            string userId = this.TryGetUserId();
            if (userId == null)
                return Unauthorized();

            listing.OwnerId = userId;

            listing.isReserved = false;
            await _listingRepository.AddAsync(listing);
            return Ok();
        }

        [HttpPost("id={id}/reserve={reserve}/for={receiverId}")]
        [Authorize]
        public async Task<ActionResult> ReserveListingAsync(string id, bool reserve, string receiverId)
        {
            string userId = this.TryGetUserId();
            var listing = await _listingRepository.GetByIdAsync(id);
            if(userId != listing.OwnerId)
                return Unauthorized();

            listing.isReserved = reserve;
            listing.recipientId = receiverId;
            await _listingRepository.UpdateAsync(listing);

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Listing>>> GetAllListingsAsync()
        {
            return Ok(await _listingRepository.GetAllAsync());
        }

        [HttpGet("userId={id}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Listing>>> GetListingsOfUserAsync(string id)
        {   
            string userId = this.TryGetUserId();
            if(userId == null || id != userId)
                return Unauthorized();

            return (await _listingRepository.GetAllUserListingsAsync(id)).ToList();
        }

        [HttpGet("requesteeId={id}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Listing>>> GetRequestedListingsOfUserAsync(string id)
        {
            string userId = this.TryGetUserId();
            if (id != userId)
                return Unauthorized();

            return (await _listingRepository.GetRequestedListingsAsync(id)).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Listing>> GetListingByIdAsync(string id)
        {
            var listing =  (await _listingRepository.GetByIdAsync(id));
            return listing != null ? Ok(listing) : NotFound();
        }

        [HttpGet("request/{id}")]
        [Authorize]
        public async Task<ActionResult> RequestListingAsync(string id)
        {
            string userId = this.TryGetUserId();
            if (userId == null)
                return Unauthorized();

            var listing = await _listingRepository.GetByIdAsync(id);
            if (listing == null)
                return NotFound();

            var user = _userService.GetUserById(userId);
            if (listing.OwnerId == userId)
                return Forbid();
            
            if(listing.RequestedUserIDs.Contains(userId) || user.RequestedListings.Contains(id))
                return Conflict();

            listing.RequestedUserIDs.Add(userId);
            await _listingRepository.UpdateAsync(listing);

            user.RequestedListings.Add(listing.Id);
            
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteListingAsync(string id)
        {
            string userId = this.TryGetUserId();
            var listing = await _listingRepository.GetByIdAsync(id);
            if (listing == null)
                return NotFound();
            if (userId == null || listing.OwnerId != userId)
                return Unauthorized();
            await _listingRepository.DeleteByIdAsync(id);
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> UpdateListingAsync(string id, [FromBody] Listing listing)
        {

            var old = await _listingRepository.GetByIdAsync(listing.Id);
            if (old == null) return NotFound();

            string userId = this.TryGetUserId();
            if (userId == null || old.OwnerId != userId) return Unauthorized();

            listing.DatePublished = DateTime.UtcNow; //temp fix for curr date with form submit
            listing.RequestedUserIDs = old.RequestedUserIDs; // temp fix for saving old requests
            listing.OwnerId = userId;
            await _listingRepository.UpdateAsync(listing);
            return Ok();
        }
    }
}