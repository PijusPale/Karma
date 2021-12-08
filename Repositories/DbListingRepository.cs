using System.Collections.Generic;
using System.Threading.Tasks;
using Karma.Database;
using Karma.Models;

namespace Karma.Repositories
{
    public class DbListingRepository : DbRepository<Listing>, IListingRepository
    {
        public DbListingRepository(BaseDbContext context) : base(context)
        {
            entities = _context.Listings;
        }

        public IEnumerable<User> GetAllRequestees(int listingId)
        {
            var listing = entities.Find(listingId);
            _context.Entry(listing).Collection<User>(l => l.Requestees).Load();
            return listing.Requestees;
        }

        public Task<IEnumerable<Listing>> GetAllUserListingsAsync(int userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Listing>> GetRequestedListingsAsync(int userId)
        {
            throw new System.NotImplementedException();
        }
    }
}