using System.Collections.Generic;
using Karma.Models;

namespace Karma.Repositories
{
    public interface IListingRepository : IRepository<Listing>
    {
        IEnumerable<Listing> GetAllUserListings(string userId);
        IEnumerable<Listing> GetRequestedListings(string userId);
    }
}