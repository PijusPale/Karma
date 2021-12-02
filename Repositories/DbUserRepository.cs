using System.Collections.Generic;
using System.Linq;
using Karma.Database;
using Karma.Models;

namespace Karma.Repositories
{
    public class DbUserRepository : DbRepository<User>, IUserRepository
    {
        public DbUserRepository(BaseDbContext context) : base(context)
        {
            entities = _context.Users;
        }

        public IEnumerable<Listing> GetAllUserListingsByUserId(int userId)
        {
            var user = entities.Find(userId);
            _context.Entry(user).Collection<Listing>(u => u.Listings).Load();
            return user.Listings;
        }

        public IEnumerable<Listing> GetAllRequestedListingsByUserId(int userId)
        {
            var user = entities.Find(userId);
            _context.Entry(user).Collection<Listing>(user => user.RequestedListings).Load();
            return user.RequestedListings;
        }
    }
}