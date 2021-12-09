using System;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using Karma.Models;
using Karma.Repositories;

namespace Karma.Services
{
    public interface IMessageService
    {
        IEnumerable<Message> GetAll(string groupId);
        IEnumerable<Message> GetByLimit(string groupId, int limit);
        IEnumerable<Message> GetByLimit(string groupId, int limit, string lastMessageId);
        void AddMessage(string content, string userId, string groupId);
        void SaveMessages(string groupId);
        void CreateConversation(int userOneId, int userTwoId, int listingId);
        IEnumerable<ConversationDto> GetConversations(int userId); 
        Conversation GetConversation(string groupId);
    }

    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        private readonly IListingRepository _listingRepository;

        private readonly IUserService _userService;

        private List<Message> _messages;

        public MessageService(IMessageRepository messageRepository, IListingRepository listingRepository, IUserService userService)
        {
            _messageRepository = messageRepository;
            _listingRepository = listingRepository;
            _userService = userService;
            _messages = new List<Message>();
        }

        public IEnumerable<Message> GetAll(string groupId)
        {
            return _messageRepository.GetAllByGroup(groupId);
        }

        public IEnumerable<Message> GetByLimit(string groupId, int limit)
        {
            return _messageRepository.GetByLimit(groupId, limit);
        }

        public IEnumerable<Message> GetByLimit(string groupId, int limit, string lastMessageId)
        {
            return _messageRepository.GetByLimit(groupId, limit, lastMessageId);
        }

        public void AddMessage(string content, string userId, string groupId)
        {
            var message = new Message(content: content, fromId: int.Parse(userId), groupId: groupId, dateSent: DateTime.UtcNow, status: 0);
            _messages.Add(message);  
        }

        public void SaveMessages(string groupId)
        {
            _messageRepository.Add(_messages, groupId);
        }

        public void CreateConversation(int userOneId, int userTwoId, int listingId)
        {
            var groupId = Guid.NewGuid().ToString("N");
            _messageRepository.CreateConversation(userOneId, userTwoId, listingId, groupId);
        }

        public IEnumerable<ConversationDto> GetConversations(int userId)
        {
            var conversations = _messageRepository.GetConversations(userId);
            var listings = _listingRepository.GetListingsByIDs(
                (from convo in conversations
                select convo.ListingId)
                .ToList());
            var result = (from c in conversations
                        join l in listings
                        on c.ListingId equals l.Id
                        select new ConversationDto {
                        ListingName = l.Name,
                        LastSender = "",
                        LastMessage = "",
                        UserOneId = c.UserOneId,
                        UserTwoId = c.UserTwoId,
                        UserOneName = _userService.GetUserById(c.UserOneId).FirstName,
                        UserTwoName = _userService.GetUserById(c.UserTwoId).FirstName,
                        ListingImg = l.ImagePath,
                        GroupId = c.GroupId,
                        ListingId = l.Id
                        }).ToList();
            
            foreach (var item in result)
            {
                var message = _messageRepository.GetByLimit(item.GroupId, 1).FirstOrDefault();
                if(message != null)
                {
                    var user = _userService.GetUserById(message.FromId);
                    item.LastSender = user.FirstName;
                    item.LastMessage = message.Content;
                    item.DateSent = message.DateSent;
                }
            }
            return result.OrderByDescending(c => c.DateSent);     
        }

        public Conversation GetConversation(string groupId)
        {
            return _messageRepository.GetConversation(groupId);
        }
    }
}