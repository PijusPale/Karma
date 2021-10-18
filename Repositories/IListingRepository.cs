using System.Collections.Generic;
using Karma.Models;

namespace Karma.Repositories
{
    public interface IListingRepository 
    {
        IEnumerable<Listing> GetAllListings();

        IEnumerable<Listing> GetAllUserListings(string userId);

        IEnumerable<Listing> GetRequestedListings(string userId);

        Listing GetListingById(string id);

        void AddListing(Listing listing);

        void DeleteListingById(string id);    

        void UpdateListing(Listing listing);
    }
}