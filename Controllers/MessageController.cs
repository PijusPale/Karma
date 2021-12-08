using Karma.Services;
using Karma.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Karma.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly ILogger<MessageController> _logger;

        private readonly IMessageService _messageService;

        private readonly IListingRepository _listingRepository;

        public MessageController(ILogger<MessageController> logger, IMessageService messageService, IListingRepository listingRepository)
        {
            _logger = logger;
            _messageService = messageService;
            _listingRepository = listingRepository;
        }

        [HttpGet("groupId={groupId}&limit={limit}")]
        [Authorize]
        public IActionResult GetMessages(string groupId, int limit)
        {
            var userId = this.TryGetUserId();
            var conversation = _messageService.GetConversation(groupId);

            if (userId == null || conversation == null || !(conversation.UserOneId == userId) && !(conversation.UserTwoId == userId) )
                return Unauthorized();

            var result = _messageService.GetByLimit(groupId, limit).ToList();
            if(result.Count == 0)
                return NoContent();
    
            return Ok(result);
        }

        [Authorize]
        [HttpGet("groupId={groupId}&limit={limit}/sinceId={lastMessageId}")]
        public IActionResult GetMessages(string groupId, int limit, string lastMessageId)
        {
            var userId = this.TryGetUserId();
            var conversation = _messageService.GetConversation(groupId);

            if (userId == null || conversation == null || !(conversation.UserOneId == userId) && !(conversation.UserTwoId == userId) )
                return Unauthorized();
   
            var result = _messageService.GetByLimit(groupId, limit, lastMessageId).ToList();
            if(result.Count == 0)
                return NoContent();

            return Ok(result);
        }

        
        [HttpGet("conversations")]
        [Authorize]
        public IActionResult GetConversations()
        {
            var userId = this.TryGetUserId();
           
            if (userId == null)
                return Unauthorized();

            var result = _messageService.GetConversations((int) userId);

            return Ok(result);
        }
    }
}
