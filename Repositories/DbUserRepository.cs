using System.Collections.Generic;
using System.Linq;
using Karma.data.messages;
using Karma.Models;

namespace Karma.Repositories
{
    public class DbUserRepository: DbRepository<User>, IUserRepository
    {
        public DbUserRepository(BaseDbContext context) : base(context)
        {
            entities = _context.Users;
        }

        public IEnumerable<Listing> GetAllUserListingsByUserId(int userId) {
            var user = entities.Find(userId);
            _context.Entry(user).Collection<Listing>(u => u.Listings).Load();
            return  user.Listings;
        }
    }
}