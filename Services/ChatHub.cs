using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using Karma.Repositories;
using System.IO;

namespace Karma.Services
{
    public class ChatHub : Hub
    {
        private IMessageService _messageService = new MessageService(new MessageRepository(Path.Combine("data", "messages")));
        public Task SendMessage(string user, string message)
        {
            return Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public Task SendMessageToCaller(string user, string message)
        {
            return Clients.Caller.SendAsync("ReceiveMessageToCaller", user, message);
        }

        public Task SendGroupMessage(string groupId, string user, string message)
        {
            _messageService.AddMessage(message, Context.ConnectionId, groupId);
            _messageService.SaveMessages(groupId);

            return Clients.Group(groupId).SendAsync("ReceiveGroupMessage", user, message);
        }

        public async Task AddToGroup(string groupId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
        }

        public async Task RemoveFromGroup(string groupId)
        {
            _messageService.SaveMessages(groupId);

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId);
        }
    }
}
