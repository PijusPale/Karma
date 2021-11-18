using Karma.Services;
using Karma.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Karma.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly ILogger<MessagesController> _logger;

        private readonly IMessageService _messageService;

        private readonly IListingRepository _listingRepository;

        public MessagesController(ILogger<MessagesController> logger, IMessageService messageService, IListingRepository listingRepository)
        {
            _logger = logger;
            _messageService = messageService;
            _listingRepository = listingRepository;
        }

        [HttpGet("listingId={listingId}/groupId={groupId}&limit={limit}")]
        [Authorize]
        public IActionResult GetMessages(string listingId, string groupId, int limit)
        {
            string userId = this.TryGetUserId();
            var listing = _listingRepository.GetById(int.Parse(listingId));
            if(listing == null)
                return NoContent();

            if (userId == null || (!listing.OwnerId.Equals(userId) && !listing.recipientId.Equals(userId)))
                return Unauthorized();

            var result = _messageService.GetByLimit(groupId, limit).ToList();
            if(result.Count == 0)
                return NoContent();
    
            return Ok(result);
        }

        [HttpGet("listingId={listingId}/groupId={groupId}&limit={limit}/sinceId={lastMessageId}")]
        public IActionResult GetMessages(string listingId, string groupId, int limit, string lastMessageId)
        {
            string userId = this.TryGetUserId();
            var listing = _listingRepository.GetById(int.Parse(listingId));
            if(listing == null)
                return NoContent();

            if (userId == null || (!listing.OwnerId.Equals(userId) && !listing.recipientId.Equals(userId)))
                return Unauthorized();
   
            var result = _messageService.GetByLimit(groupId, limit, lastMessageId).ToList();
            if(result.Count == 0)
                return NoContent();

            return Ok(result);
        }
    }
}
