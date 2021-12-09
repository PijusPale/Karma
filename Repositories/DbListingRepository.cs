using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
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

        public async Task<IEnumerable<Listing>> GetAllUserListingsAsync(int userId)
        {
            var listings =  from l in entities
                            where l.UserId == userId
                            select l;
            return await Task.Run(() => listings.ToList());
        }

        public async Task<IEnumerable<Listing>> GetRequestedListingsAsync(int userId)
        {
            var listings =  from l in entities
                            where l.RequestedUserIDs.Contains(userId)
                            select l;
            return await Task.Run(() => listings.ToList());
        }

        public IEnumerable<Listing> GetListingsByIDs(List<int> IdList)
        {
            var listings =  from l in entities
                            where IdList.Contains(l.Id)
                            select l;

            return listings;
        }
    }
}