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
        void AddMessage(string content, string userId, string groupId);
        void SaveMessages(string groupId);
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

        public void AddMessage(string content, string userId, string groupId)
        {
            var message = new Message(content: content, fromId: userId, groupId: groupId, dateSent: DateTime.UtcNow, status: 0);
            _messages.Add(message);  
        }

        public void SaveMessages(string groupId)
        {
            _messageRepository.Add(_messages, groupId);
        }
    }
}