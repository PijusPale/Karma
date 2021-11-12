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
        //TODO: Find a better way to do DI
        private IMessageService _messageService = new MessageService(new MessageRepository(Path.Combine("data", "messages")));
       
        public Task SendMessageAsync(string user, string message)
        {
            return Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public Task SendMessageToCallerAsync(string user, string message)
        {
            return Clients.Caller.SendAsync("ReceiveMessageToCaller", user, message);
        }

        public Task SendGroupMessageAsync(string groupId, string user, string message)
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
