using System.Collections.Generic;
using Karma.Models;

namespace Karma.Repositories
{
    public interface IMessageRepository
    {
        IEnumerable<Message> GetAllByGroup(string groupId);

        void Add(List<Message> newMessages, string groupId);

        void DeleteById(string messageId, string groupId);
    }
}