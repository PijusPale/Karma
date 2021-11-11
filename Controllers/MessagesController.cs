using Karma.Services;
using Karma.Models;
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

        public MessagesController(ILogger<MessagesController> logger, IMessageService messageService)
        {
            _logger = logger;
            _messageService = messageService;
        }

        [HttpGet("groupId={groupId}&limit={limit}")]
        [Authorize]
        public IActionResult GetMessages(string groupId, int limit)
        {
            string userId = this.TryGetUserId();
            if (userId == null)
                return Unauthorized();

            var result = _messageService.GetByLimit(groupId, limit).ToList();
            if(result.Count == 0)
                return NoContent();
    
            return Ok(result);
        }

        [HttpGet("groupId={groupId}&limit={limit}/sinceId={lastMessageId}")]
        public IActionResult GetMessages(string groupId, int limit, string lastMessageId)
        {
            string userId = this.TryGetUserId();
            if (userId == null)
                return Unauthorized();
   
            var result = _messageService.GetByLimit(groupId, limit, lastMessageId).ToList();
            if(result.Count == 0)
                return NoContent();

            return Ok(result);
        }
    }
}
