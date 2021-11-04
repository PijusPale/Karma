using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Karma.Repositories;

namespace Karma.Services
{
    public class ChatHub : Hub
    {
        private List<MessageService> _messageServices {get; set; }

        private readonly IMessageRepository _messageRepository;

        public ChatHub(IMessageRepository MessageRepository) : base()
        {
            _messageServices = new List<MessageService>();
            _messageRepository = MessageRepository;
        }

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
            //save message
            return Clients.Group(groupId).SendAsync("ReceiveGroupMessage", user, message);
        }

        public async Task AddToGroup(string groupId)
        {
            if(!_messageServices.Any(x => x.GroupId == groupId))
                _messageServices.Add(new MessageService(_messageRepository, groupId));
            
            await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
        }

        public async Task RemoveFromGroup(string groupId)
        {
            //save all messages to that file
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId);
        }
    }
}
