using System.Collections.Generic;
using Karma.Models;

namespace Karma.Repositories
{
    public interface IMessageRepository
    {
        IEnumerable<Message> GetAllByGroup(string groupId);

        IEnumerable<Conversation> GetConversations(int userId);

        IEnumerable<Message> GetByLimit(string groupId, int limit);

        IEnumerable<Message> GetByLimit(string groupId, int limit, string lastMessageId);

        void Add(List<Message> newMessages, string groupId);

        void DeleteById(string messageId, string groupId);
    }
}