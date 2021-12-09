using System.Collections.Generic;
using System.Threading.Tasks;
using Karma.Models;

namespace Karma.Repositories
{
    public interface IMessageRepository
    {
        IEnumerable<Message> GetAllByGroup(string groupId);

        IEnumerable<Conversation> GetConversations(int userId);

        Conversation GetConversation(string groupId);

        Conversation GetConversation(int listingId);

        Task<bool> DeleteConversationAsync(int listingId);

        IEnumerable<Message> GetByLimit(string groupId, int limit);

        IEnumerable<Message> GetByLimit(string groupId, int limit, string lastMessageId);

        void Add(List<Message> newMessages, string groupId);

        void CreateConversation(int userOneId, int userTwoId, int listingId, string groupId);

        void DeleteById(string messageId, string groupId);
    }
}