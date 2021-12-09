using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Karma.Database;
using Karma.Models;

namespace Karma.Repositories
{
    public class DbMessageRepository : DbRepository<Message>, IMessageRepository
    {
        public DbMessageRepository(BaseDbContext context) : base(context)
        {
            entities = _context.Messages;
        }

        public IEnumerable<Message> GetAllByGroup(string groupId)
        {
            var messages =  from m in entities
                            where m.GroupId == groupId
                            select m;
            return messages;
        }

        public IEnumerable<Conversation> GetConversations(int userId)
        {
            var conversations = from c in _context.Conversations
                                where c.UserOneId == userId || c.UserTwoId == userId
                                select c; 
            return conversations;
        }

        public Conversation GetConversation(string groupId)
        {
            var conversation =  from c in _context.Conversations
                                where c.GroupId == groupId
                                select c;
            return conversation.FirstOrDefault();
        }

        public Conversation GetConversation(int listingId)
        {
            var conversation =  from c in _context.Conversations
                                where c.ListingId == listingId
                                select c;
            return conversation.FirstOrDefault();
        }

        public async Task<bool> DeleteConversationAsync(int listingId)
        {
            try
            {
                _context.Conversations.Remove(GetConversation(listingId));
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<Message> GetByLimit(string groupId, int limit)
        {
            var messages =  (from m in entities
                            orderby m.DateSent descending
                            where m.GroupId == groupId
                            select m).Take(limit).Reverse();
            return messages;
        }

        public IEnumerable<Message> GetByLimit(string groupId, int limit, string lastMessageId)
        {
            var query = from m in entities
                        orderby m.DateSent descending
                        where m.GroupId == groupId
                        select m;
            var offset = query.ToList().FindIndex(x => x.Id.ToString().Equals(lastMessageId)) + 1;
            var messages = query.Skip(offset).Take(limit).Reverse();
            return messages;
        }

        public void Add(List<Message> newMessages, string groupId)
        {
            entities.AddRange(newMessages);
            _context.SaveChanges();
        }

        public void CreateConversation(int userOneId, int userTwoId, int listingId, string groupId)
        {
            _context.Conversations.Add(new Conversation{UserOneId = userOneId, UserTwoId = userTwoId, ListingId = listingId, GroupId = groupId });
            _context.SaveChanges();
        }

        public void DeleteById(string messageId, string groupId)
        {
            entities.Remove(entities.Find(Int32.TryParse(messageId, out int x))); //Parse messageId to int in MessageService
            _context.SaveChanges();
        }
    }
}