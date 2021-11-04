using System;
using System.Collections.Generic;
using Karma.Models;
using Karma.Repositories;

namespace Karma.Services
{
    public interface IMessageService
    {
        IEnumerable<Message> GetAll();
        void AddMessage(string content, string connectionId);
        void SaveMessages();
    }
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        private List<Message> _messages;

        public string GroupId { get; }

        public MessageService(IMessageRepository messageRepository, string groupId)
        {
            _messageRepository = messageRepository;
            GroupId = groupId;
        }

        public IEnumerable<Message> GetAll()
        {
            return _messages;
        }

        public void AddMessage(string content, string connectionId)
        {
            //get user id from connection id
            var message = new Message(content: content, fromId: userId, groupId: GroupId, dateSent: DateTime.UtcNow, status: 0);
            _messages.Add(message);  
        }

        public void SaveMessages()
        {
            _messageRepository.Add(_messages, GroupId);
        }
    }
}