using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using Karma.Repositories;
using System.IO;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Karma.Services
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IMessageService _messageService;
        
        public ChatHub(IMessageService messageService)
        {
            _messageService = messageService;
        }
    
        public Task SendMessageAsync(string user, string message)
        {
            return Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public Task SendMessageToCallerAsync(string user, string message)
        {
            return Clients.Caller.SendAsync("ReceiveMessageToCaller", user, message);
        }

        public Task SendGroupMessageAsync(string groupId, int user, string message)
        {   
            _messageService.AddMessage(message, Context.UserIdentifier, groupId);
            _messageService.SaveMessages(groupId);

            return Clients.Group(groupId).SendAsync("ReceiveGroupMessage", user, message);
        }

        public async Task AddToGroupAsync(string groupId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
        }

        public async Task RemoveFromGroupAsync(string groupId)
        {
            _messageService.SaveMessages(groupId);

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId);
        }
    }
}
