using System.Collections.Generic;
using System.Threading.Tasks;
using Karma.data.messages;
using Karma.Models;

namespace Karma.Repositories
{
    public class DbListingRepository : DbRepository<Listing>, IListingRepository
    {
        public DbListingRepository(BaseDbContext context) : base(context)
        {
            entities = _context.Listings;
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