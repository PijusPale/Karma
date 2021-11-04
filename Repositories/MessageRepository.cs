using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Karma.Models;

namespace Karma.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        protected string _filePath;

        public MessageRepository(string filePath)
        {
            _filePath = filePath;
        }

        public IEnumerable<Message> GetAllByGroup(string groupId)
        {
            var fullFilePath = Path.ChangeExtension(Path.Combine(_filePath, groupId), ".json");
            if(System.IO.File.Exists(fullFilePath))
            {
                string jsonString = System.IO.File.ReadAllText(fullFilePath);
                return JsonSerializer.Deserialize<List<Message>>(jsonString);
            }
            
            return Enumerable.Empty<Message>();
        }
        public void Add(List<Message> newMessages, string groupId)
        {
            List<Message> oldMessages = GetAllByGroup(groupId).ToList();
            oldMessages.AddRange(newMessages);
            WriteMessagesToFile(oldMessages, groupId);
        }

        public void DeleteById(string messageId, string groupId)
        {
            List<Message> messages = GetAllByGroup(groupId).ToList();
            messages.Remove(messages.Find(x => x.Id == messageId));
            WriteMessagesToFile(messages, groupId);
        }

        private void WriteMessagesToFile(List<Message> messages, string groupId)
        {
            var fullFilePath = Path.ChangeExtension(Path.Combine(_filePath, groupId), ".json");
            var jsonOptions = new JsonSerializerOptions() { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(messages, jsonOptions);
            System.IO.File.WriteAllText(fullFilePath, jsonString);
        }
    }
}