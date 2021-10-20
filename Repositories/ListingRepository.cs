using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Karma.Models;

namespace Karma.Repositories
{
    public class ListingRepository : Repository<Listing>, IListingRepository
    {

        public ListingRepository(string filePath) : base(filePath) { }

        public IEnumerable<Listing> GetAllUserListings(string userId)
        {
            List<Listing> listings = GetAll().ToList();
            return listings.Where(a => a.OwnerId == userId);
        }

        public IEnumerable<Listing> GetRequestedListings(string userId)
        {
            List<Listing> listings = GetAll().ToList();
            return listings.Where(a => a.RequestedUserIDs.Contains(userId));
        }
    }
}