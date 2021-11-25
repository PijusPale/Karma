using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Karma.Models;
using Microsoft.Extensions.Logging;

namespace Karma.Repositories
{
    public class ListingRepository : Repository<Listing>, IListingRepository
    {

        public ListingRepository(string filePath, ILogger<ListingRepository> logger) : base(filePath, logger) { }

        public IEnumerable<User> GetAllRequestees(int listingId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Listing>> GetAllUserListingsAsync(int userId)
        {
            List<Listing> listings = (await GetAllAsync()).ToList();
            if (listings == null)
                return null;
            return listings.Where(a => a.UserId == userId);
        }

        public async Task<IEnumerable<Listing>> GetRequestedListingsAsync(int userId)
        {
            List<Listing> listings = (await GetAllAsync()).ToList();
            if (listings == null)
                return null;
            return listings.Where(a => a.RequestedUserIDs.Contains(userId));
        }
    }
}