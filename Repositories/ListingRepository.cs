using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Karma.Models;

namespace Karma.Repositories
{
    public class ListingRepository : Repository<Listing>, IListingRepository
    {

        public ListingRepository(string filePath) : base(filePath) { }

        public async Task<IEnumerable<Listing>> GetAllUserListingsAsync(string userId)
        {
            List<Listing> listings = (await GetAllAsync()).ToList();
            return listings.Where(a => a.OwnerId == userId);
        }

        public async Task<IEnumerable<Listing>> GetRequestedListingsAsync(string userId)
        {
            List<Listing> listings = (await GetAllAsync()).ToList();
            return listings.Where(a => a.RequestedUserIDs.Contains(userId));
        }
    }
}