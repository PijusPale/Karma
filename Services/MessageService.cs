using System;
using System.Collections.Generic;
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
        IEnumerable<ConversationDto> GetConversations(string userId); 
    }

    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        private List<Message> _messages;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
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
            var random = new Random();
            string messageId = random.Next(9999).ToString();
            var message = new Message(id: messageId, content: content, fromId: userId, groupId: groupId, dateSent: DateTime.UtcNow, status: 0);
            _messages.Add(message);  
        }

        public void SaveMessages(string groupId)
        {
            _messageRepository.Add(_messages, groupId);
        }

        public IEnumerable<ConversationDto> GetConversations(string userId)
        {
        }
    }
}