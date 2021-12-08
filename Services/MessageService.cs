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
            var messages = new List<Message>();
            foreach (var convo in conversations)
            {
                var message = _messageRepository.GetByLimit(convo.GroupId, 1);
                messages.AddRange(message);
            }
            var query = from c in conversations
                        join l in listings
                        on c.ListingId equals l.Id
                        select new  {
                                    ListingName = l.Name,
                                    ListingImg = l.ImagePath,
                                    GroupId = c.GroupId,
                                    ListingId = l.Id,
                                    UserOneId = c.UserOneId,
                                    UserTwoId = c.UserTwoId,
                                    };
            var result = (from q in query
                        select new ConversationDto {
                        ListingName = q.ListingName,
                        LastSender = "",
                        LastMessage = "",
                        UserOneId = q.UserOneId,
                        UserTwoId = q.UserTwoId,
                        UserOneName = _userService.GetUserById(q.UserOneId).FirstName,
                        UserTwoName = _userService.GetUserById(q.UserTwoId).FirstName,
                        ListingImg = q.ListingImg,
                        GroupId = q.GroupId,
                        ListingId = q.ListingId
                        }).ToList();
            
            foreach (var item in result)
            {
                var msg = messages.FirstOrDefault(x => x.GroupId == item.GroupId);
                if(msg != null)
                {
                    var user = _userService.GetUserById(msg.FromId);
                    item.LastSender = user.FirstName;
                    item.LastMessage = msg.Content;
                    item.DateSent = msg.DateSent;
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