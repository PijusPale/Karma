using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Authorization;

namespace Karma.Services
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IMessageService _messageService;

        private readonly IUserService _userService;

        private static List<int> usersOnline = new List<int>();

        public ChatHub(IMessageService messageService, IUserService userService)
        {
            _messageService = messageService;
            _userService = userService;
        }

        public async override Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            if (Context.UserIdentifier != null)
                usersOnline.Add(int.Parse(Context.UserIdentifier));
        }

        public async override Task OnDisconnectedAsync(Exception ex)
        {
            await base.OnDisconnectedAsync(ex);
            if (Context.UserIdentifier != null)
            {
                var userId = int.Parse(Context.UserIdentifier);
                usersOnline.Remove(userId);
                var user = _userService.GetUserById(userId);
                user.LastActive = DateTime.UtcNow;
                _userService.Update(user);
            }
        }

        public Task SendMessageAsync(string user, string message)
        {
            return Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public Task SendMessageToCallerAsync(string user, string message)
        {
            return Clients.Caller.SendAsync("ReceiveMessageToCaller", user, message);
        }

        public Task LastActiveRequest(int userId)
        {
            if (usersOnline.Contains(userId))
            {
                return Clients.Caller.SendAsync("GetLastActive", DateTime.UtcNow);
            }
            else
            {
                var user = _userService.GetUserById(userId);
                return Clients.Caller.SendAsync("GetLastActive", user.LastActive);
            }
        }

        public Task SendGroupMessageAsync(string groupId, int user, string message)
        {
            _messageService.AddMessage(message, Context.UserIdentifier, groupId);
            _messageService.SaveMessages(groupId);

            return Clients.Group(groupId).SendAsync("ReceiveGroupMessage", user, message);
        }

        public Task SendTypingNotificationAsync(string groupId, int userId, bool isTyping)
        {
            return Clients.Group(groupId).SendAsync("ReceiveTypingNotification", userId, isTyping);
        }

        public async Task AddToGroupAsync(string groupId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
        }

        public async Task RemoveFromGroupAsync(string groupId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId);
        }
    }
}
